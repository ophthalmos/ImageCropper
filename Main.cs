using ImageMagick;
using System;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace ImageCropper
{
    public partial class Main : Form
    {
        private enum DragHandle { None, TopLeft, Top, TopRight, Left, Right, BottomLeft, Bottom, BottomRight, Inside } // Definition der möglichen Griffpunkte
        private const int HANDLE_SIZE = 16; // Größe der Griffe in Pixeln
        private readonly int _handleSize;
        private Rectangle _selectionRect = Rectangle.Empty;
        private Point _dragStartPoint;
        private DragHandle _currentDragHandle = DragHandle.None;
        private bool _isDragging = false;
        private string? _currentImagePath = null;
        private int _targetWidth = 320;
        private string _lastDirectory = "";
        //private readonly string _settingsFileName = "settings.xml";
        private float _currentAspectRatio = 0.0f; // 0.0f bedeutet "Freie Auswahl"
        private float _defaultAspectRatio = 0.0f; // 0.0f bedeutet "Freie Auswahl"
        private Point _mouseDownPoint; // Wo die Taste gedrückt wurde
        private bool _isNewSelection = false; // Unterscheidung: Bestehender Griff vs. neues Rechteck
        private bool _isLoading = false;
        private string? _saveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private string? _saveImageName = "Profilbild";
        private long _jpegQuality = 90L;
        private bool _closeAfterCropSuccess = true;
        private readonly string? _settingsPath;
        private static readonly string _appPath = Application.ExecutablePath; // EXE-Pfad
        private readonly string _appName = "ImageCropper";

        public Main(string? bildPfad = null)
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
            _handleSize = (int)(HANDLE_SIZE * DeviceDpi / 96.0); // Skalierung für hohe DPI  
            SetupControls();

            if (Util.IsInnoSetupValid(Path.GetDirectoryName(_appPath)!))
            {
                _settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _appName, _appName + ".xml");
            }
            else { _settingsPath = Path.ChangeExtension(_appPath, ".xml"); }
            LoadSettings();

            if (!string.IsNullOrEmpty(bildPfad)) { LoadImageFile(bildPfad); }
        }

        protected override CreateParams CreateParams // Methode gegen Flackern in WinForms; stabilisiert oft auch den Schattenwurf des DWM
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        //private string SettingsPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _appName + ".xml");
        private void SetupControls() => typeof(Control).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, pictureBox, [true]);

        private void LoadSettings()
        {
            if (!File.Exists(_settingsPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_settingsPath)!); // If the folder exists already, the line will be ignored.     
                SaveSettings();
                return;
            }
            try
            {
                XDocument doc = XDocument.Load(_settingsPath);
                var root = doc.Root;
                if (root != null)
                {
                    _targetWidth = (int?)root.Element("TargetWidth") ?? _targetWidth;
                    _currentAspectRatio = _defaultAspectRatio = (float?)root.Element("AspectRatio") ?? _defaultAspectRatio;
                    _saveImageName = (string?)root.Element("ImageName") ?? _saveImageName;
                    _saveDirectory = (string?)root.Element("SaveDirectory") ?? _saveDirectory;
                    _lastDirectory = (string?)root.Element("LastDirectory") ?? "";
                    _jpegQuality = (long?)root.Element("JpegQuality") ?? 90L;
                    _closeAfterCropSuccess = (bool?)root.Element("CloseAfterCropSuccess") ?? true;
                }
                SetAspectRatio(_currentAspectRatio);
            }
            catch { } // Bei Lesefehlern (z.B. defektes XML) einfach nichts tun, Standard lassen
        }

        private void SaveSettings()
        {
            try
            {
                XElement root = new("Settings",
                    new XElement("TargetWidth", _targetWidth),
                    new XElement("AspectRatio", _defaultAspectRatio),
                    new XElement("ImageName", _saveImageName),
                    new XElement("SaveDirectory", _saveDirectory),
                    new XElement("JpegQuality", _jpegQuality),
                    new XElement("LastDirectory", _lastDirectory),
                    new XElement("CloseAfterCropSuccess", _closeAfterCropSuccess)
                );
                new XDocument(root).Save(_settingsPath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _appName + ".xml"));
            }
            catch (Exception ex) { Util.MsgTaskDlg(Handle, "Fehler", ex.Message); }
        }

        private async void LoadImageFile(string imagePath)
        {
            if (_isLoading) { return; } // Verhindert Mehrfachstarts
            try
            {
                if (!File.Exists(imagePath)) { return; }
                _isLoading = true; // "Warten"-Status aktivieren
                menuStrip.Enabled = false;
                Application.UseWaitCursor = true;
                Cursor.Current = Cursors.WaitCursor;
                pictureBox.Image = null; // Altes Bild sofort ausblenden
                pictureBox.Invalidate(); // Paint-Event erzwingen
                Image nextImage = await Task.Run(() => // Bild im Hintergrund laden
                {
                    if (Util.IsHeicFile(imagePath))
                    {
                        using var magickImage = new ImageMagick.MagickImage(imagePath);
                        magickImage.AutoOrient();
                        return magickImage.ToBitmap();
                    }
                    else
                    {
                        using var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                        using var tempImage = Image.FromStream(stream);
                        Util.CorrectExifOrientation(tempImage);
                        return new Bitmap(tempImage);
                    }
                });
                _isLoading = false; // UI-Update (zurück im Haupt-Thread)
                SuspendLayout();
                var oldImage = pictureBox.Image;
                pictureBox.Image = nextImage;
                oldImage?.Dispose();
                _selectionRect = Rectangle.Empty;
                AdjustFormSizeToImage(pictureBox.Image);
                _currentImagePath = imagePath;
                ResumeLayout(true);
            }
            catch (Exception ex)
            {
                pictureBox.Invalidate();
                Util.MsgTaskDlg(Handle, "Fehler beim Laden des Bildes", ex.Message);
            }
            finally
            {
                _isLoading = false;
                menuStrip.Enabled = true;
                Application.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;
            }
        }

        private void SetAspectRatio(float ratio)
        {
            _currentAspectRatio = ratio;
            ratioFreeToolStripMenuItem.Checked = false;
            ratio1to1ToolStripMenuItem.Checked = false;
            ratio2to3ToolStripMenuItem.Checked = false;
            ratio3to4ToolStripMenuItem.Checked = false;
            if (ratio <= 0) ratioFreeToolStripMenuItem.Checked = true;
            else if (Math.Abs(ratio - 1.0f) < 0.01) { ratio1to1ToolStripMenuItem.Checked = true; }
            else if (Math.Abs(ratio - (2f / 3f)) < 0.01) { ratio2to3ToolStripMenuItem.Checked = true; }
            else if (Math.Abs(ratio - (3f / 4f)) < 0.01) { ratio3to4ToolStripMenuItem.Checked = true; }
            if (!_selectionRect.IsEmpty && ratio > 0)
            {
                ApplyAspectRatio(ref _selectionRect, DragHandle.BottomRight);
                _selectionRect = Util.NormalizeRectangle(_selectionRect);
            }
            pictureBox.Invalidate();
        }

        private void ApplyAspectRatio(ref Rectangle rect, DragHandle handle)
        {
            if (_currentAspectRatio <= 0) { return; }
            int right = rect.Right;   // Fixpunkte merken
            int bottom = rect.Bottom; // für die Verankerung
            if ((float)rect.Width / rect.Height > _currentAspectRatio) // Breiter als das Verhältnis -> Höhe anpassen
            {
                rect.Width = (int)Math.Round(rect.Height * _currentAspectRatio); // Zu breit -> Breite reduzieren, Math.Round für präzisere Ergebnisse
                if (handle == DragHandle.TopLeft || handle == DragHandle.BottomLeft || handle == DragHandle.Left) { rect.X = right - rect.Width; }
            }
            else
            {
                rect.Height = (int)Math.Round(rect.Width / _currentAspectRatio);
                if (handle == DragHandle.TopLeft || handle == DragHandle.TopRight || handle == DragHandle.Top) { rect.Y = bottom - rect.Height; }
            }
            rect.X = Math.Max(0, rect.X); // Innerhalb der PictureBox halten
            rect.Y = Math.Max(0, rect.Y); // Innerhalb der PictureBox halten
            if (rect.Right > pictureBox.Width) { rect.Width = pictureBox.Width - rect.X; }
            if (rect.Bottom > pictureBox.Height) { rect.Height = pictureBox.Height - rect.Y; }
        }

        private void PictureBox_MouseDown(object? sender, MouseEventArgs e)
        {
            if (pictureBox.Image == null || e.Button != MouseButtons.Left) return;
            _mouseDownPoint = e.Location;
            _currentDragHandle = HitTest(e.Location);
            if (_currentDragHandle != DragHandle.None)  // Wir haben einen Griff oder das Innere getroffen
            {
                _isDragging = true;
                _dragStartPoint = e.Location;
                _isNewSelection = false;
            }
            else // Klick außerhalb der Auswahl: Auswahl sofort aufheben
            {
                _selectionRect = Rectangle.Empty;
                _isNewSelection = true;
                _isDragging = false;
                pictureBox.Invalidate(); // Zeichnet das Bild ohne Rahmen neu
            }
        }

        private void PictureBox_MouseMove(object? sender, MouseEventArgs e)
        {
            if (pictureBox.Image == null) { return; }
            if (!_isDragging && _isNewSelection && e.Button == MouseButtons.Left)
            {
                if (Math.Abs(e.X - _mouseDownPoint.X) > SystemInformation.DragSize.Width || Math.Abs(e.Y - _mouseDownPoint.Y) > SystemInformation.DragSize.Height) // Mindestbewegung prüfen
                {
                    _isDragging = true;
                    _selectionRect = new Rectangle(_mouseDownPoint, new Size(0, 0));
                    _currentDragHandle = DragHandle.BottomRight;
                    _dragStartPoint = _mouseDownPoint;
                    SetCursor(_currentDragHandle);
                }
            }
            if (!_isDragging)
            {
                SetCursor(HitTest(e.Location));
                return;
            }
            Rectangle newRect = _selectionRect; // B) Das eigentliche Ziehen/Vergrößern
            switch (_currentDragHandle) // Berechnung basierend darauf, welcher Griff gezogen wird
            {
                case DragHandle.TopLeft:
                    newRect.X = e.X; newRect.Y = e.Y;
                    newRect.Width = _selectionRect.Right - e.X;
                    newRect.Height = _selectionRect.Bottom - e.Y;
                    break;
                case DragHandle.TopRight:
                    newRect.Y = e.Y;
                    newRect.Width = e.X - _selectionRect.X;
                    newRect.Height = _selectionRect.Bottom - e.Y;
                    break;
                case DragHandle.BottomLeft:
                    newRect.X = e.X;
                    newRect.Width = _selectionRect.Right - e.X;
                    newRect.Height = e.Y - _selectionRect.Y;
                    break;
                case DragHandle.BottomRight:
                    newRect.Width = e.X - _selectionRect.X;
                    newRect.Height = e.Y - _selectionRect.Y;
                    break;
                case DragHandle.Top:
                    newRect.Y = e.Y; newRect.Height = _selectionRect.Bottom - e.Y; break;
                case DragHandle.Bottom:
                    newRect.Height = e.Y - _selectionRect.Y; break;
                case DragHandle.Left:
                    newRect.X = e.X; newRect.Width = _selectionRect.Right - e.X; break;
                case DragHandle.Right:
                    newRect.Width = e.X - _selectionRect.X; break;
                case DragHandle.Inside: // Verschieben der ganzen Box
                    int dx = e.X - _dragStartPoint.X;
                    int dy = e.Y - _dragStartPoint.Y;
                    newRect.Offset(dx, dy);
                    _dragStartPoint = e.Location; // Reset für nächstes Delta
                    break;
            }
            if (_currentAspectRatio > 0 && _currentDragHandle != DragHandle.Inside) { ApplyAspectRatio(ref newRect, _currentDragHandle); } // Seitenverhältnis erzwingen
            newRect = Util.NormalizeRectangle(newRect); // Normalisieren (falls Breite/Höhe negativ wurde durch Ziehen über die Gegenseite hinaus)
            if (newRect != _selectionRect)
            {
                _selectionRect = newRect;
                pictureBox.Invalidate();
            }
        }

        private void PictureBox_MouseUp(object? sender, MouseEventArgs e)
        {
            _isDragging = false;
            _isNewSelection = false;
            _currentDragHandle = DragHandle.None;
            pictureBox.Cursor = Cursors.Default;
            clipContextMenuItem.Enabled = cropContextMenuItem.Enabled = cancelContextMenuItem.Enabled = cropToolStripMenuItem.Enabled = _selectionRect.Width > 0 && _selectionRect.Height > 0;
            pictureBox.Invalidate();
        }

        private void PictureBox_Paint(object? sender, PaintEventArgs e)
        {
            if (_isLoading)
            {
                string text = "Bitte warten…"; // ⌛
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                using Font font = new("Segoe UI", 24);
                Size textSize = TextRenderer.MeasureText(text, font);
                int x = (pictureBox.Width - textSize.Width) / 2;
                int y = (pictureBox.Height - textSize.Height) / 2;
                e.Graphics.DrawString(text, font, Brushes.White, x, y);
            }
            else if (pictureBox.Image == null) { toolStripStatusSize.Text = ""; }
            else if (_selectionRect.Width <= 0 || _selectionRect.Height <= 0) { toolStripStatusSize.Text = $"{pictureBox.Image.Width} × {pictureBox.Image.Height} Px"; }
            else
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.None;
                using (Brush shadowBrush = new SolidBrush(Color.FromArgb(120, Color.Black)))  // "Dunkle Maske" ohne Region-Objekt zeichnen
                {
                    g.FillRectangle(shadowBrush, 0, 0, pictureBox.Width, _selectionRect.Y); // Oben
                    g.FillRectangle(shadowBrush, 0, _selectionRect.Bottom, pictureBox.Width, pictureBox.Height - _selectionRect.Bottom); // Unten
                    g.FillRectangle(shadowBrush, 0, _selectionRect.Y, _selectionRect.X, _selectionRect.Height); // Links
                    g.FillRectangle(shadowBrush, _selectionRect.Right, _selectionRect.Y, pictureBox.Width - _selectionRect.Right, _selectionRect.Height); // Rechts 
                }
                using (Pen whitePen = new(Color.White, 1)) { g.DrawRectangle(whitePen, _selectionRect); }
                using (Pen antPen = new(Color.Black, 1))
                {
                    antPen.DashStyle = DashStyle.Dash;
                    g.DrawRectangle(antPen, _selectionRect);
                }
                DrawHandle(e.Graphics, DragHandle.TopLeft);
                DrawHandle(e.Graphics, DragHandle.TopRight);
                DrawHandle(e.Graphics, DragHandle.BottomLeft);
                DrawHandle(e.Graphics, DragHandle.BottomRight);
                if (_currentAspectRatio <= 0)  // Diese Griffe nur zeichnen, wenn das Verhältnis frei ist
                {
                    DrawHandle(e.Graphics, DragHandle.Top);
                    DrawHandle(e.Graphics, DragHandle.Bottom);
                    DrawHandle(e.Graphics, DragHandle.Left);
                    DrawHandle(e.Graphics, DragHandle.Right);
                }
                Rectangle realRect = Util.TranslateSelectionToImageCoordinates(_selectionRect, pictureBox);
                toolStripStatusSize.Text = $"{realRect.Width}×{realRect.Height} ({_selectionRect.Width}×{_selectionRect.Height}) Px";
            }
        }

        private void DrawHandle(Graphics g, DragHandle handle)
        {
            Rectangle r = GetHandleRect(handle);
            Rectangle shadow = r;
            shadow.Offset(1, 1);
            g.FillRectangle(new SolidBrush(Color.FromArgb(50, Color.Black)), shadow);
            g.FillRectangle(Brushes.White, r);
            using Pen p = new(Color.DodgerBlue, 1); // Blaue Umrandung statt Schwarz
            g.DrawRectangle(p, r);
        }

        private DragHandle HitTest(Point mouseLoc)
        {
            if (_selectionRect.IsEmpty) return DragHandle.None;
            if (GetHandleRect(DragHandle.TopLeft).Contains(mouseLoc)) { return DragHandle.TopLeft; }
            if (GetHandleRect(DragHandle.TopRight).Contains(mouseLoc)) { return DragHandle.TopRight; }
            if (GetHandleRect(DragHandle.BottomLeft).Contains(mouseLoc)) { return DragHandle.BottomLeft; }
            if (GetHandleRect(DragHandle.BottomRight).Contains(mouseLoc)) { return DragHandle.BottomRight; }
            if (GetHandleRect(DragHandle.Top).Contains(mouseLoc)) { return DragHandle.Top; }
            if (GetHandleRect(DragHandle.Bottom).Contains(mouseLoc)) { return DragHandle.Bottom; }
            if (GetHandleRect(DragHandle.Left).Contains(mouseLoc)) { return DragHandle.Left; }
            if (GetHandleRect(DragHandle.Right).Contains(mouseLoc)) { return DragHandle.Right; }
            if (_selectionRect.Contains(mouseLoc)) { return DragHandle.Inside; }
            return DragHandle.None;
        }

        private Rectangle GetHandleRect(DragHandle handle)
        {
            Rectangle r = _selectionRect;
            int hs2 = _handleSize / 2;
            return handle switch
            {
                DragHandle.TopLeft => new Rectangle(r.X - hs2, r.Y - hs2, _handleSize, _handleSize),
                DragHandle.TopRight => new Rectangle(r.Right - hs2, r.Y - hs2, _handleSize, _handleSize),
                DragHandle.BottomLeft => new Rectangle(r.X - hs2, r.Bottom - hs2, _handleSize, _handleSize),
                DragHandle.BottomRight => new Rectangle(r.Right - hs2, r.Bottom - hs2, _handleSize, _handleSize),
                DragHandle.Top => new Rectangle(r.X + r.Width / 2 - hs2, r.Y - hs2, _handleSize, _handleSize),
                DragHandle.Bottom => new Rectangle(r.X + r.Width / 2 - hs2, r.Bottom - hs2, _handleSize, _handleSize),
                DragHandle.Left => new Rectangle(r.X - hs2, r.Y + r.Height / 2 - hs2, _handleSize, _handleSize),
                DragHandle.Right => new Rectangle(r.Right - hs2, r.Y + r.Height / 2 - hs2, _handleSize, _handleSize),
                _ => Rectangle.Empty,
            };
        }

        private void SetCursor(DragHandle handle)
        {
            pictureBox.Cursor = handle switch
            {
                DragHandle.TopLeft or DragHandle.BottomRight => Cursors.SizeNWSE,
                DragHandle.TopRight or DragHandle.BottomLeft => Cursors.SizeNESW,
                DragHandle.Top or DragHandle.Bottom => Cursors.SizeNS,
                DragHandle.Left or DragHandle.Right => Cursors.SizeWE,
                DragHandle.Inside => Cursors.SizeAll,
                _ => Cursors.Default,
            };
        }

        private void CancelSelection()
        {
            if (!_selectionRect.IsEmpty)
            {
                _selectionRect = Rectangle.Empty;
                _isDragging = false;
                _isNewSelection = false;
                clipContextMenuItem.Enabled = cropContextMenuItem.Enabled = cancelContextMenuItem.Enabled = cropToolStripMenuItem.Enabled = false;
                pictureBox.Invalidate();
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new()
            {
                Filter = "Bilddateien|*.jpg;*.jpeg;*.heic;*.png;*.bmp;*.gif|" +
                         "JPEG-Dateien (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                         "HEIC-Fotos (*.heic)|*.heic|" +
                         "PNG-Dateien (*.png)|*.png|" +
                         "Alle Dateien (*.*)|*.*"
            };
            if (!string.IsNullOrEmpty(_lastDirectory) && Directory.Exists(_lastDirectory)) { ofd.InitialDirectory = _lastDirectory; }
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _lastDirectory = Path.GetDirectoryName(ofd.FileName) ?? string.Empty;
                SaveSettings(); // Den Pfad sofort in der XML-Datei sichern
                LoadImageFile(ofd.FileName);
            }
        }

        private void CopySelectionToClipboard()
        {
            // Validierung wie in deiner Crop-Methode
            if (pictureBox.Image == null || _selectionRect.Width <= 1 || _selectionRect.Height <= 1)
            {
                Util.MsgTaskDlg(Handle, "Keine Auswahl", "Bitte wählen Sie zuerst einen Bereich aus.", TaskDialogIcon.Warning);
                return;
            }
            try
            {
                Rectangle sourceRect = Util.TranslateSelectionToImageCoordinates(_selectionRect, pictureBox);
                Bitmap originalImage = (Bitmap)pictureBox.Image;
                using Bitmap croppedImage = new Bitmap(sourceRect.Width, sourceRect.Height);
                using (Graphics g = Graphics.FromImage(croppedImage))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.DrawImage(originalImage, new Rectangle(0, 0, croppedImage.Width, croppedImage.Height), sourceRect, GraphicsUnit.Pixel);
                }
                Clipboard.SetImage(croppedImage);
                toolStripStatusSize.Text = "Auswahl in die Zwischenablage kopiert.";
            }
            catch (Exception ex) { Util.MsgTaskDlg(Handle, "Fehler beim Kopieren", ex.Message, TaskDialogIcon.Error); }
        }

        private void CropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null || _selectionRect.Width <= 1 || _selectionRect.Height <= 1)
            {
                Util.MsgTaskDlg(Handle, "Keine Auswahl", "Bitte wählen Sie zuerst einen Bereich aus.", TaskDialogIcon.Warning);
                return;
            }
            try
            {
                Rectangle sourceRect = Util.TranslateSelectionToImageCoordinates(_selectionRect, pictureBox);
                Bitmap originalImage = (Bitmap)pictureBox.Image;
                if (sourceRect.Width <= 0 || sourceRect.Height <= 0) { throw new Exception("Auswahl ist ungültig."); }
                int targetWidth = sourceRect.Width; // Zielgröße berechnen (Skalierung auf max 320px Breite)
                int targetHeight = sourceRect.Height;
                if (targetWidth > _targetWidth)
                {
                    float ratio = (float)sourceRect.Height / sourceRect.Width;
                    targetWidth = _targetWidth;
                    targetHeight = (int)(targetWidth * ratio);
                }
                Bitmap croppedImage = new(targetWidth, targetHeight);
                using (Graphics g = Graphics.FromImage(croppedImage))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.DrawImage(originalImage, new Rectangle(0, 0, targetWidth, targetHeight), sourceRect, GraphicsUnit.Pixel); // Quelle -> Ziel (0, 0, targetWidth, targetHeight)
                }
                string savePath = Path.Combine(_saveDirectory ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop), _saveImageName + ".jpg");
                ImageCodecInfo? jpgEncoder = Util.GetEncoder(ImageFormat.Jpeg);
                if (jpgEncoder == null) { croppedImage.Save(savePath, ImageFormat.Jpeg); }
                else
                {
                    EncoderParameters myEncoderParameters = new(1);
                    myEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, _jpegQuality);
                    croppedImage.Save(savePath, jpgEncoder, myEncoderParameters);
                    myEncoderParameters.Dispose(); // EncoderParameter freigeben
                }
                bool savedToClipboard = true;
                try { Clipboard.SetDataObject(savePath, true, 5, 10); }
                catch (Exception) { savedToClipboard = false; }
                var oldImage = pictureBox.Image;
                pictureBox.Image = croppedImage;
                pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                oldImage.Dispose();
                _selectionRect = Rectangle.Empty;
                pictureBox.Invalidate();
                StringBuilder sb = new(20);
                Util.StrFormatByteSize(new FileInfo(savePath).Length, sb, sb.Capacity);
                var btnClose = new TaskDialogButton("Beenden");
                var page = new TaskDialogPage()
                {
                    Caption = _appName,
                    Heading = $"{_saveImageName} gespeichert",
                    Text = $"<a href=\"{_saveDirectory}\">{savePath}</a> ({sb})" + (savedToClipboard ? "\n\nDer Pfad wurde in die Zwischenablage kopiert." : ""),
                    Icon = TaskDialogIcon.ShieldSuccessGreenBar,
                    EnableLinks = true,
                    AllowCancel = true,
                    SizeToContent = true,
                    Verification = _currentImagePath != null ? new TaskDialogVerificationCheckBox() { Text = "Originalbild löschen" } : "",
                    Buttons = { btnClose, TaskDialogButton.Cancel },
                    Expander = _currentImagePath != null ? new TaskDialogExpander($"Optional wird das Originalbild beim Beenden automatisch gelöscht.\n{_currentImagePath}") : null
                };
                page.Created += (s, e) =>
                {
                    IntPtr hwnd = Util.GetActiveWindow();  // Handle des TaskDialog
                    if (hwnd != IntPtr.Zero)  // aktuelle Position minus ein Viertel der Form-Höhe
                    {
                        if (Util.GetWindowRect(hwnd, out Util.RECT rect)) { Util.MoveWindow(hwnd, rect.Left, rect.Top - (Height / 4), rect.Right - rect.Left, rect.Bottom - rect.Top, true); }
                    }
                };
                page.LinkClicked += (s, args) =>
                {
                    try
                    {
                        string dopusrt = @"C:\Program Files\GPSoftware\Directory Opus\dopusrt.exe";
                        using Process process = new();
                        process.StartInfo.UseShellExecute = false;
                        if (File.Exists(dopusrt))
                        {
                            process.StartInfo.FileName = dopusrt;
                            process.StartInfo.Arguments = $"/cmd Go \"{savePath}\"";
                            process.Start();
                        }
                        else
                        {
                            process.StartInfo.FileName = "explorer.exe";
                            process.StartInfo.UseShellExecute = true;
                            process.StartInfo.Arguments = $"/select,\"{savePath}\"";
                            process.Start();
                        }
                    }
                    catch (Exception ex) { Util.MsgTaskDlg(Handle, "Fehler", "Datei konnte nicht im Dateimanager geöffnet werden: " + ex.Message); }
                };
                page.Verification.Checked = _currentImagePath != null;
                if (TaskDialog.ShowDialog(this, page) == btnClose)
                {
                    if (page.Verification.Checked && _currentImagePath != null && File.Exists(_currentImagePath))
                    {
                        try { File.Delete(_currentImagePath); }
                        catch (Exception ex) { Util.MsgTaskDlg(Handle, "Das Originalbild konnte nicht gelöscht werden.", ex.Message); }
                    }
                    if (_closeAfterCropSuccess)
                    {
                        Close();
                        return;
                    }
                }
                pictureBox.Image?.Dispose();
                pictureBox.Image = null;
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                _currentImagePath = null;
                CancelSelection(); //cropContextMenuItem.Enabled = cancelContextMenuItem.Enabled = cropToolStripMenuItem.Enabled = false;
            }
            catch (Exception ex) { Util.MsgTaskDlg(Handle, "Fehler", ex.Message); }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (_currentImagePath == null && Clipboard.ContainsImage())
            {
                Image? clipboardImage = Clipboard.GetImage();
                if (clipboardImage != null && pictureBox.Image == null)
                {
                    AdjustFormSizeToImage(clipboardImage);
                    pictureBox.Image = clipboardImage;
                }
            }
        }

        private void AdjustFormSizeToImage(Image image)
        {
            if (image == null) { return; }
            Screen currentScreen = Screen.FromControl(this); // Bildschirm ermitteln, auf dem sich die Form aktuell befindet (Wichtig für Multi-Monitor!)
            Rectangle workingArea = currentScreen.WorkingArea;
            int originalWidth = image.Width;
            int originalHeight = image.Height;
            int horizontalChrome = Width - pictureBox.Width; // Differenz zwischen Form-Breite und PictureBox-Breite
            int verticalChrome = Height - pictureBox.Height;
            int maxImageWidth = workingArea.Width - horizontalChrome; // Verfügbarer Platz für das Bild
            int maxImageHeight = workingArea.Height - verticalChrome;
            int targetImageWidth = originalWidth; // Skalierte Bildgröße (initial unverändert)
            int targetImageHeight = originalHeight;
            float scaleX = (float)maxImageWidth / originalWidth; // Skalierungsfaktor (Verhältnis von Max-Platz zu Bildgröße)
            float scaleY = (float)maxImageHeight / originalHeight;
            if (scaleX < 1.0f || scaleY < 1.0f) // Bild ist zu groß für den verfügbaren Platz
            {
                float scaleFactor = Math.Min(scaleX, scaleY); // Den kleineren Skalierungsfaktor verwenden, um in beide Dimensionen zu passen
                targetImageWidth = (int)(originalWidth * scaleFactor);
                targetImageHeight = (int)(originalHeight * scaleFactor);
            }
            Width = targetImageWidth + horizontalChrome; // Neue Form-Größe setzen
            Height = targetImageHeight + verticalChrome;
            int newX = workingArea.Left + (workingArea.Width - Width) / 2; // Form zentrieren   
            int newY = workingArea.Top + (workingArea.Height - Height) / 2;
            Location = new Point(newX, newY); // Position anwenden
        }

        private void Main_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data!.GetData(DataFormats.FileDrop)!;
            if (files.Length > 0) LoadImageFile(files[0]);
        }

        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data!.GetDataPresent(DataFormats.FileDrop)) { e.Effect = DragDropEffects.Copy; }
        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e) => ShowAboutDialog();

        private void ShowAboutDialog()
        {
            var myVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "0.1";
            var buildDate = Util.GetBuildDate();
            TaskDialogPage page = new()
            {
                Caption = "Über ImageCropper",
                Heading = $"ImageCropper v{myVersion} ({buildDate:d})",
                Text = "© 2025 Dr. W. Happe. Alle Rechte vorbehalten.",
                Icon = new(Properties.Resources.icon32),
                AllowCancel = true
            };
            TaskDialogCommandLinkButton btnCustom = new("Den Einstellungsdialog öffnen", $"Die aktuelle Zielbreite beträgt: {_targetWidth} Pixel");
            TaskDialogButton btnClose = TaskDialogButton.Close;
            page.Buttons.Add(btnClose);
            page.Buttons.Add(btnCustom);
            TaskDialogButton result = TaskDialog.ShowDialog(this, page);
            if (result == btnCustom) { OpenSettings(); }
        }

        private void OpenSettings()
        {
            using Opts dlg = new(_defaultAspectRatio, _targetWidth, _saveImageName ?? "Profilbild", _saveDirectory ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop), (int)_jpegQuality, _closeAfterCropSuccess);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                _targetWidth = dlg.TargetWidth;
                _defaultAspectRatio = dlg.SelectedRatio;
                _saveImageName = dlg.ImageName;
                _saveDirectory = dlg.TargetDir;
                _jpegQuality = dlg.JpegQuality;
                _closeAfterCropSuccess = dlg.CloseOption;
                SaveSettings();
                SetAspectRatio(_defaultAspectRatio);
            }
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;  // verhindern, dass Enter andere Controls (wie Buttons) auslöst
                e.SuppressKeyPress = true;
                if (cropToolStripMenuItem.Enabled) { CropToolStripMenuItem_Click(this, EventArgs.Empty); }
                else if (pictureBox.Image == null) { OpenToolStripMenuItem_Click(this, EventArgs.Empty); }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                CancelSelection();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F1)
            {
                ShowAboutDialog();
                e.Handled = true;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.N: OpenToolStripMenuItem_Click(this, EventArgs.Empty); return true;
                case Keys.Control | Keys.S: CropToolStripMenuItem_Click(this, EventArgs.Empty); return true;
                case Keys.Control | Keys.NumPad1:
                case Keys.Control | Keys.D1: SetAspectRatio(1.0f); return true;   // 1:1
                case Keys.Control | Keys.NumPad2:
                case Keys.Control | Keys.D2: SetAspectRatio(2f / 3f); return true; // 2:3
                case Keys.Control | Keys.NumPad3:
                case Keys.Control | Keys.D3: SetAspectRatio(3f / 4f); return true; // 3:4
                case Keys.Control | Keys.NumPad0:
                case Keys.Control | Keys.D0: SetAspectRatio(0.0f); return true;   // Frei
                case Keys.F2: OpenSettings(); return true;
                case Keys.F3: OpenSettingsFile(); return true;
                case Keys.Control | Keys.C: CopySelectionToClipboard(); return true;
            }
            if (_selectionRect != Rectangle.Empty)
            {
                int step = (keyData & Keys.Shift) == Keys.Shift ? 10 : 1;  // Basis-Schrittweite (1px), mit Shift (10px)
                Keys keyCode = keyData & Keys.KeyCode;
                bool isControl = (keyData & Keys.Control) == Keys.Control;
                switch (keyCode)
                {
                    case Keys.Left:
                        if (isControl) _selectionRect.Width -= step; else _selectionRect.X -= step;
                        break;
                    case Keys.Right:
                        if (isControl) _selectionRect.Width += step; else _selectionRect.X += step;
                        break;
                    case Keys.Up:
                        if (isControl) _selectionRect.Height -= step; else _selectionRect.Y -= step;
                        break;
                    case Keys.Down:
                        if (isControl) _selectionRect.Height += step; else _selectionRect.Y += step;
                        break;
                    default:
                        return base.ProcessCmdKey(ref msg, keyData);
                }
                _selectionRect.Width = Math.Max(1, _selectionRect.Width); // Mindestbreite-Validierung
                _selectionRect.Height = Math.Max(1, _selectionRect.Height); // Mindesthöhe-Validierung
                _selectionRect.X = Math.Clamp(_selectionRect.X, 0, pictureBox.Width - _selectionRect.Width); // Links/Rechts-Begrenzung
                _selectionRect.Y = Math.Clamp(_selectionRect.Y, 0, pictureBox.Height - _selectionRect.Height); // Oben/Unten-Begrenzung
                pictureBox.Invalidate(); // Zeichnet Auswahl und Status-Label neu
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Util.WM_SYSCOMMAND)
            {
                int command = m.WParam.ToInt32() & 0xFFF0;
                if (command == Util.SC_RESTORE)
                {
                    BeginInvoke(new Action(() => { AdjustFormSizeToImage(pictureBox.Image!); })); // Verzögertes Anpassen nach Wiederherstellung
                }
            }
            base.WndProc(ref m);
        }

        private void OpenSettingsFile()
        {
            try
            {
                if (File.Exists(_settingsPath))
                {
                    ProcessStartInfo psi = new()
                    {
                        FileName = _settingsPath,
                        UseShellExecute = true // Öffnet die Datei mit dem Standardprogramm
                    };
                    Process.Start(psi);
                }
                else { Util.MsgTaskDlg(Handle, "Hinweis", "Die Einstellungsdatei wurde noch nicht erstellt."); }
            }
            catch (Exception ex) { Util.MsgTaskDlg(Handle, "Fehler", $"Datei konnte nicht geöffnet werden: {ex.Message}"); }
        }

        private void CropContextMenuItem_Click(object sender, EventArgs e) => CropToolStripMenuItem_Click(this, EventArgs.Empty);

        private void CancelContextMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectionRect != Rectangle.Empty)
            {
                _selectionRect = Rectangle.Empty;
                pictureBox.Invalidate(); // PictureBox neu zeichnen, um Auswahlrahmen zu löschen
            }
        }

        private void Ratio1to1ToolStripMenuItem_Click(object sender, EventArgs e) => SetAspectRatio(1.0f);
        private void Ratio2to3ToolStripMenuItem_Click(object sender, EventArgs e) => SetAspectRatio(2f / 3f);
        private void Ratio3to4ToolStripMenuItem_Click(object sender, EventArgs e) => SetAspectRatio(3f / 4f);
        private void RationFreeToolStripMenuItem_Click(object sender, EventArgs e) => SetAspectRatio(0.0f);

        private void ClipContextMenuItem_Click(object sender, EventArgs e) => CopySelectionToClipboard();
    }
}