using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace OpenTap.Package.PackageInstallHelpers
{
    /// <summary> Locks a file using flock on linux. This essentially works as a mutex.  </summary>
    class PosixFileLock : IDisposable
    {
        int fd;
        public static PosixFileLock Take(string file)
        {
            int fd = PosixNative.open(file, PosixNative.O_RDWR | PosixNative.O_APPEND,
                Convert.ToInt32("666", 8));
            var lockObj = new PosixFileLock() {fd = fd};
            lockObj.Take();
            return lockObj;
        }

        void Take() => PosixNative.flock(fd, PosixNative.LOCK_EX);
        void Release() => PosixNative.close(fd);
        public void Dispose()
        {
            if (fd >= 0)
            {
                Release();
                fd = -1;
            }

        }
    }

    class Win32FileLock : IDisposable
    {
        private Mutex mtx;
        public static Win32FileLock Take(string file)
        {
            var mtx = new Mutex(false, "package_install_lock_" + Path.GetFullPath(file).GetHashCode());
            mtx.WaitOne();
            return new Win32FileLock(){mtx = mtx};
        }

        public void Dispose()
        {
            mtx?.ReleaseMutex();
            mtx?.Dispose();
            mtx = null;
        }
    }

    static class PosixNative
    {
        [DllImport("libc")]
        public static extern int open(string pathname, int flags, int mode);

        [DllImport("libc")]
        public static extern int close(int fd);

        [DllImport("libc")]
        public static extern int flock(int fd, int operation);

        public const int O_CREAT = 64; //00000100;
        public const int O_TRUNC = 512; //00001000;
        public const int O_APPEND = 1024; //00002000;
        public const int O_RDWR = 2; //00000002;
        public const int LOCK_SH = 1;
        public const int LOCK_EX = 2;
    }
}