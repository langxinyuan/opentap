using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace OpenTap
{
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
}