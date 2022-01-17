using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using OpenTap.Cli;
using OpenTap.Diagnostic;

namespace OpenTap
{
    /// <summary>
    /// <see cref="TapSerializer"/> does not correctly serialize / deserialize <see cref="Event"/> presumably because it is a struct
    /// </summary>
    internal static class SerializationHelper
    {
        private static XmlSerializer serializer = new XmlSerializer(typeof(Event));

        public static byte[] EventToBytes(Event evt)
        {
            var ms = new MemoryStream();
            serializer.Serialize(ms, evt);

            return ms.ToArray();
        }

        public static Event StreamToEvent(Stream stream)
        {
            return (Event)serializer.Deserialize(stream);
        }
    }

    internal static class PipeReader
    {
        /// <summary>
        /// Reads a message by interpreting the first byte as the message length
        /// and the concatenating reads until it encounters a terminating message of length '0'.
        /// </summary>
        /// <param name="pipe"></param>
        /// <returns></returns>
        public static MemoryStream ReadMessage(this PipeStream pipe)
        {
            int getLength()
            {
                var buffer = new byte[2];
                var numRead = 0;

                while (numRead < 2)
                {
                    numRead += pipe.Read(buffer, numRead, 2 - numRead);
                    if (numRead == 0) return 0;
                }

                var lower = buffer[0];
                var upper = buffer[1];

                return lower | (upper << 8);
            }

            var messageLength = getLength();
            var buffers = new List<byte[]>();

            while (messageLength > 0)
            {
                var buf = new byte[messageLength];
                var numRead = 0;
                while (numRead < messageLength)
                {
                    numRead += pipe.Read(buf, numRead, messageLength - numRead);
                }

                buffers.Add(buf);

                messageLength = getLength();
            }

            var ms = new MemoryStream();
            var index = 0;

            foreach (var buf2 in buffers)
            {
                ms.Write(buf2, index, buf2.Length);
                index += buf2.Length;
            }

            ms.Seek(0, SeekOrigin.Begin);

            return ms;
        }

        private static object writeLock = new object();

        /// <summary>
        /// Sends a message by splitting it into messages of length UInt16.MaxValue
        /// </summary>
        /// <param name="pipe"></param>
        /// <param name="message"></param>
        public static void WriteMessage(this PipeStream pipe, string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            pipe.WriteMessage(bytes);
        }

        public static void WriteMessage(this PipeStream pipe, byte[] bytes)
        {
            lock (writeLock)
            {
                var sent = 0;
                var remaining = bytes.Length;
                while (remaining > 0)
                {
                    var length = Math.Min(remaining, UInt16.MaxValue);
                    remaining -= length;
                    // first send the message length
                    // lower byte
                    pipe.WriteByte((byte)(length & 255));
                    // higher byte
                    pipe.WriteByte((byte)(length / 256));
                    // then write the message
                    pipe.Write(bytes, sent, length);

                    sent += length;
                }

                // the message is terminated by 0 indicating a 0 length message
                pipe.WriteByte(0);
                pipe.WriteByte(0);
                pipe.Flush();
            }
        }


    }

    [Browsable(false)]
    internal class ProcessCliAction : ICliAction
    {
        [CommandLineArgument("PipeHandle")] public string PipeHandle { get; set; }

        public int Execute(CancellationToken cancellationToken)
        {
            var client = new NamedPipeClientStream(".", PipeHandle, PipeDirection.InOut);
            var listener = new EventTraceListener();

            listener.MessageLogged += evts =>
            {
                foreach (var evt in evts)
                {
                    if (client.IsConnected == false) return;
                    // The log will be flushed before the client is disposed
                    client.WriteMessage(SerializationHelper.EventToBytes(evt));
                }
            };

            try
            {
                client.Connect();
                var msg = client.ReadMessage();
                var steps = (ITestStep)new TapSerializer().Deserialize(msg);
                var plan = new TestPlan();
                plan.ChildTestSteps.Add(steps);

                Log.AddListener(listener);

                return (int)plan.Execute(Array.Empty<IResultListener>()).Verdict;
            }
            finally
            {
                Log.RemoveListener(listener);
                client.Dispose();
            }
        }
    }

    /// <summary>
    /// This is an abstraction for running child processes with support for elevation.
    /// It executes a test step (which can have child test steps) in a new process
    /// It supports subscribing to log events from the child process, and forwarding the logs directly.
    /// </summary>
    internal class ProcessHelper : EventTraceListener
    {
        public ProcessHelper(bool forwardLogs)
        {
            if (forwardLogs)
                MessageLogged += evts =>
                {
                    foreach (var evt in evts)
                    {
                        LogLine(evt);
                    }
                };
        }

        public static bool IsAdmin()
        {
            if (OperatingSystem.Current == OperatingSystem.Windows)
            {
                using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
                {
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    return principal.IsInRole(WindowsBuiltInRole.Administrator);
                }
            }
            else // assume UNIX
            {
                // id -u should print '0' if running as sudo or the current user is root
                var pInfo = new ProcessStartInfo()
                {
                    FileName = "id",
                    Arguments = "-u",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                };

                using (var p = Process.Start(pInfo))
                {
                    p.BeginOutputReadLine();
                    var output = p.StandardOutput.ReadToEnd().Trim();
                    if (int.TryParse(output, out var id) && id == 0)
                        return true;
                    return false;
                }
            }
        }

        private static string getTap()
        {
            var currentProcess = Assembly.GetExecutingAssembly().Location;
            if (string.IsNullOrWhiteSpace(currentProcess) == false &&
                string.Equals(Path.GetFileNameWithoutExtension(currentProcess), "tap",
                    StringComparison.CurrentCultureIgnoreCase))
                return currentProcess;

            if (OperatingSystem.Current == OperatingSystem.Windows)
                return Path.Combine(ExecutorClient.ExeDir, "tap.exe");
            return Path.Combine(ExecutorClient.ExeDir, "tap");
        }

        private static void LogLine(Event evt)
        {
            var childLog = Log.CreateSource("Subprocess: " + evt.Source);
            var message = evt.Message;

            switch (evt.EventType)
            {
                case 10:
                    childLog.Error(message);
                    break;
                case 20:
                    childLog.Warning(message);
                    break;
                case 30:
                    childLog.Info(message);
                    break;
                case 40:
                    childLog.Debug(message);
                    break;
            }
        }

        private static TraceSource log = Log.CreateSource(nameof(ProcessHelper));
        internal Process LastProcessHandle = null;

        public Verdict Run(ITestStep step, bool elevate, CancellationToken token)
        {
            var handle = Guid.NewGuid().ToString();
            var pInfo = new ProcessStartInfo(getTap())
            {
                Arguments = $"{nameof(ProcessCliAction)} --PipeHandle \"{handle}\"",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = true
            };

            if (elevate)
            {
                if (OperatingSystem.Current == OperatingSystem.Linux)
                {
                    pInfo.Arguments = $"\"{pInfo.FileName}\" {pInfo.Arguments}";
                    pInfo.FileName = "sudo";
                }
                else
                {
                    pInfo.Verb = "runas";
                }
            }

            using (var p = Process.Start(pInfo))
            {
                if (p == null) throw new Exception($"Failed to spawn process.");
                LastProcessHandle = p;

                // Ensure the process is cleaned up
                TapThread.Current.AbortToken.Register(() =>
                {
                    if (p.HasExited) return;

                    try
                    {
                        // process.Kill may throw if it has already exited.
                        p.Kill();
                    }
                    catch (Exception ex)
                    {
                        log.Warning("Caught exception when killing process. {0}", ex.Message);
                    }
                });

                try
                {
                    var server = new NamedPipeServerStream(handle, PipeDirection.InOut, 1);
                    server.WaitForConnection();
                    var stepString = new TapSerializer().SerializeToString(step);
                    server.WriteMessage(stepString);

                    while (server.IsConnected)
                    {
                        if (token.IsCancellationRequested)
                            throw new OperationCanceledException($"Process cancelled by the user.");

                        var msg = server.ReadMessage();
                        if (msg.Length == 0) continue;
                        var evt = SerializationHelper.StreamToEvent(msg);
                        this.TraceEvents(new[] { evt });
                    }

                    var processExitTask = Task.Run(() => p.WaitForExit(), token);
                    var tokenCancelledTask = Task.Run(() => token.WaitHandle.WaitOne(), token);

                    Task.WaitAny(processExitTask, tokenCancelledTask);
                    if (token.IsCancellationRequested)
                    {
                        throw new OperationCanceledException($"Process cancelled by the user.");
                    }

                    return (Verdict)p.ExitCode;
                }
                finally
                {
                    if (p.HasExited == false)
                    {
                        p.Kill();
                    }
                }
            }
        }
    }
}
