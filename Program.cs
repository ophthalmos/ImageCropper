using System.Diagnostics;

namespace ImageCropper;

internal static class Program
{
    [STAThread]
    static void Main(string[] args)
    {   // GUID erhält man durch Eingabe in Developer PowerShell: [Guid]::NewGuid()
        var mutexName = @"Local\ImageCropperRunningMutex"; // Local Mutex to allow multiple users on same machine
        using Mutex singleMutex = new(true, mutexName, out var isNewInstance);
        if (isNewInstance)
        {
            ApplicationConfiguration.Initialize();
            var bildPfad = args.Length > 0 ? args[0] : null;
            Application.Run(new Main(bildPfad));
        }
        else
        {
            Process current = Process.GetCurrentProcess();
            foreach (Process process in Process.GetProcessesByName(current.ProcessName))
            {
                if (process.Id != current.Id)
                {
                    var handle = process.MainWindowHandle;
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