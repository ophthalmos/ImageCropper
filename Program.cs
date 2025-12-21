using System.Diagnostics;

namespace ImageCropper
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {   // GUID erhält man durch Eingabe in Developer PowerShell: [Guid]::NewGuid()
            string mutexName = @"Local\ImageCropper_{42e29052-2991-4964-878a-514fd1309b69}"; // Local Mutex to allow multiple users on same machine
            using Mutex singleMutex = new(true, mutexName, out var isNewInstance);
            if (isNewInstance)
            {
                ApplicationConfiguration.Initialize();
                string? bildPfad = args.Length > 0 ? args[0] : null;
                Application.Run(new Main(bildPfad));
            }
            else
            {
                Process current = Process.GetCurrentProcess();
                foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {
                        IntPtr handle = process.MainWindowHandle;
                        if (handle != IntPtr.Zero)
                        {
                            Util.ShowWindow(handle, Util.SW_RESTORE);
                            Util.SetForegroundWindow(handle);
                        }
                        break;
                    }
                }
            }
            GC.KeepAlive(singleMutex); // Ensure the mutex is not released prematurely  
        }
    }
}