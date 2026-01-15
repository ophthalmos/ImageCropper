using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageCropper;

internal class Util
{
    public const int WM_SYSCOMMAND = 0x0112;
    public const int SC_RESTORE = 0xF120;

    [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
    public static extern long StrFormatByteSize(long fileSize, [Out] StringBuilder buffer, int bufferSize);

    [DllImport("user32.dll")]
    public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    [DllImport("user32.dll")]
    public static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    public const int SW_RESTORE = 9;

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    public static DateTime GetBuildDate()
    { //s. <SourceRevisionId>build$([System.DateTime]::UtcNow.ToString("yyyyMMddHHmmss"))</SourceRevisionId> in ClipMenu.csproj
        const string BuildVersionMetadataPrefix = "+build";
        var attribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        if (attribute?.InformationalVersion != null)
        {
            var value = attribute.InformationalVersion;
            var index = value.IndexOf(BuildVersionMetadataPrefix);
            if (index > 0)
            {
                value = value[(index + BuildVersionMetadataPrefix.Length)..];
                if (DateTime.TryParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result)) { return result; }
            }
        }
        return default;
    }

    public static bool IsHeicFile(string filePath)
    {
        if (!File.Exists(filePath)) return false;
        var buffer = new byte[12];
        try
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                if (fs.Length < 12) return false;
                fs.Read(buffer, 0, 12);
            }
            var signature = Encoding.ASCII.GetString(buffer, 4, 8); // HEIC Signaturen liegen ab Byte 4
            return signature.StartsWith("ftypheic") ||
                   signature.StartsWith("ftypmif1") ||
                   signature.StartsWith("ftyphevc");
        }
        catch { return false; }
    }

    public static Image CorrectExifOrientation(Image image)
    {
        const int ExifOrientationId = 0x0112;
        if (image.PropertyIdList.Contains(ExifOrientationId))
        {
            var propItem = image.GetPropertyItem(ExifOrientationId);
            if (propItem?.Value != null && propItem.Value.Length >= 2)
            {
                var orientation = BitConverter.ToUInt16(propItem.Value, 0);
                switch (orientation)
                {
                    case 1: break;
                    case 2: image.RotateFlip(RotateFlipType.RotateNoneFlipX); break;
                    case 3: image.RotateFlip(RotateFlipType.Rotate180FlipNone); break;
                    case 4: image.RotateFlip(RotateFlipType.Rotate180FlipX); break;
                    case 5: image.RotateFlip(RotateFlipType.Rotate90FlipX); break;
                    case 6: image.RotateFlip(RotateFlipType.Rotate90FlipNone); break;
                    case 7: image.RotateFlip(RotateFlipType.Rotate270FlipX); break;
                    case 8: image.RotateFlip(RotateFlipType.Rotate270FlipNone); break;
                }
                image.RemovePropertyItem(ExifOrientationId);
            }
        }
        return image;
    }

    public static ImageCodecInfo? GetEncoder(ImageFormat format)
    {
        var codecs = ImageCodecInfo.GetImageEncoders();
        foreach (var codec in codecs)
        {
            if (codec.FormatID == format.Guid) { return codec; }
        }
        return null;
    }

    public static void MsgTaskDlg(nint hwnd, string heading, string message, TaskDialogIcon? icon = null)
    {
        TaskDialog.ShowDialog(hwnd, new TaskDialogPage() { Caption = Application.ProductName, SizeToContent = true, Heading = heading, Text = message, Icon = icon ?? TaskDialogIcon.None, AllowCancel = true, Buttons = { TaskDialogButton.OK } });
    }

    public static void ErrTaskDlg(nint? hwnd, Exception error)
    {
        TaskDialogPage page = new()
        {
            Caption = Application.ProductName,
            Heading = error.GetType().ToString(),
            Text = error.Message,
            Icon = TaskDialogIcon.Error,
            Buttons = { TaskDialogButton.OK },
            Expander = new TaskDialogExpander()
            {
                Text = error.ToString(),
                CollapsedButtonText = "Technische Details anzeigen",
                ExpandedButtonText = "Details ausblenden",
                Position = TaskDialogExpanderPosition.AfterFootnote
            }
        };
        TaskDialog.ShowDialog(hwnd ?? IntPtr.Zero, page);
    }

    internal static void StartFile(nint handle, string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                ProcessStartInfo psi = new(filePath) { UseShellExecute = true, WorkingDirectory = Path.GetDirectoryName(filePath) };
                Process.Start(psi);
            }
            else { MsgTaskDlg(handle, "Datei nicht gefunden!", "'" + filePath + "' fehlt.", TaskDialogIcon.ShieldWarningYellowBar); }
        }
        catch (Exception ex) when (ex is Win32Exception || ex is InvalidOperationException) { ErrTaskDlg(handle, ex); }
    }

    public static Rectangle TranslateSelectionToImageCoordinates(Rectangle selection, PictureBox pb)
    {
        if (pb.Image == null || pb.SizeMode != PictureBoxSizeMode.Zoom) { return selection; } // Oder Fehlerbehandlung
        var img = pb.Image;
        var imageRatio = (float)img.Width / img.Height;
        var containerRatio = (float)pb.Width / pb.Height;
        float scaleFactor;
        float offsetX = 0;
        float offsetY = 0;

        if (containerRatio > imageRatio)
        {
            scaleFactor = (float)pb.Height / img.Height;
            offsetX = (pb.Width - (img.Width * scaleFactor)) / 2f;
        }
        else
        {
            scaleFactor = (float)pb.Width / img.Width;
            offsetY = (pb.Height - (img.Height * scaleFactor)) / 2f;
        }
        var trueX = (int)Math.Max(0, (selection.X - offsetX) / scaleFactor);
        var trueY = (int)Math.Max(0, (selection.Y - offsetY) / scaleFactor);
        var trueWidth = (int)Math.Round(selection.Width / scaleFactor);
        var trueHeight = (int)Math.Round(selection.Height / scaleFactor);
        var maxWidth = img.Width - trueX;
        var maxHeight = img.Height - trueY;
        return new Rectangle(trueX, trueY, Math.Min(trueWidth, maxWidth), Math.Min(trueHeight, maxHeight));
    }

    public static Rectangle NormalizeRectangle(Rectangle r) // Stellt sicher, dass Width/Height positiv sind, falls man über die Kanten gezogen hat.
    {
        var x = r.Width < 0 ? r.X + r.Width : r.X;
        var y = r.Height < 0 ? r.Y + r.Height : r.Y;
        var w = Math.Abs(r.Width);
        var h = Math.Abs(r.Height);
        return new Rectangle(x, y, w, h);
    }

    public static bool IsLikelyMtpTempFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) { return false; }
        var isInCacheFolder = filePath.Contains("INetCache", StringComparison.OrdinalIgnoreCase) || filePath.Contains(@"AppData\Local", StringComparison.OrdinalIgnoreCase); // …\AppData\Local\Microsoft\Windows\INetCache\IE\OWZ2Q89F
        if (!isInCacheFolder) { return false; }
        var fileNameNoExt = Path.GetFileNameWithoutExtension(filePath);

        // Regex Erklärung:
        // \s?       -> Optionales Leerzeichen
        // ( ... )   -> Gruppe für Alternativen
        // \(\d+\)   -> Runde Klammern mit Zahl: (12)
        // |         -> ODER
        // \[\d+\]   -> Eckige Klammern mit Zahl: [12]
        // $         -> Muss am Ende des Dateinamens stehen
        var hasNumberSuffix = Regex.IsMatch(fileNameNoExt, @"\s?(\(\d+\)|\[\d+\])$");
        return hasNumberSuffix;
    }

    //public static bool HasWritePermission(string path)
    //{
    //    try
    //    {
    //        string testFile = Path.Combine(path, Path.GetRandomFileName());
    //        using FileStream fs = File.Create(testFile, 1, FileOptions.DeleteOnClose);
    //        return true;
    //    }
    //    catch { return false; }
    //}


    //public static bool IsInnoSetupValid(string assemblyLocation)
    //{
    //    var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\ImageCropper_is1");
    //    if (key == null) { return false; }
    //    var value = (string?)key.GetValue("UninstallString");
    //    if (value == null) { return false; }
    //    else if (Debugger.IsAttached) { return true; } // run by Visual Studio
    //    else { return assemblyLocation.Equals(RemoveFromEnd(value.Trim('"'), "\\unins000.exe"), StringComparison.Ordinal); } // "C:\Program Files\ClipMenu\unins000.exe"
    //}

    //private static string RemoveFromEnd(string str, string toRemove) => str.EndsWith(toRemove) ? str[..^toRemove.Length] : str;

}
