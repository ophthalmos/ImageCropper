namespace ImageCropper
{
    public partial class Opts : Form
    {
        public int JpegQuality
        {
            get
            {
                if (cbxJPEGQuality.SelectedItem != null)
                {
                    if (int.TryParse(cbxJPEGQuality.SelectedItem.ToString(), out int result)) { return result; }
                }
                return 90; // Fallback
            }
        }
        public int TargetWidth => (int)numericUpDown.Value;
        public string ImageName => tbxImageName.Text;
        public string TargetDir => tbxTargetDir.Text;
        public float SelectedRatio
        {
            get
            {
                if (rBtn1to1.Checked) return 1.0f;
                if (rBtn2to3.Checked) return 2f / 3f;
                if (rBtn3to4.Checked) return 3f / 4f;
                return 0.0f; // Frei
            }
        }

        public bool CloseOption => cbxCloseOption.Checked;

        public Opts(float currentRatio, int currentWidth, string imageName, string saveDir, long quality, bool closeOption)
        {
            InitializeComponent();
            numericUpDown.Value = currentWidth;
            tbxImageName.Text = imageName;
            tbxTargetDir.Text = saveDir;
            if (currentRatio == 0.0f) rBtnFree.Checked = true;  // RadioButtons setzen
            else if (currentRatio == 1.0f) rBtn1to1.Checked = true;
            else if (Math.Abs(currentRatio - (2f / 3f)) < 0.01) rBtn2to3.Checked = true;
            else if (Math.Abs(currentRatio - (3f / 4f)) < 0.01) rBtn3to4.Checked = true;
            int index = -1;
            for (int i = 0; i < cbxJPEGQuality.Items.Count; i++)
            {
                if (cbxJPEGQuality.Items[i]?.ToString() == quality.ToString())
                {
                    index = i;
                    break;
                }
            }
            cbxJPEGQuality.SelectedIndex = (index != -1) ? index : 3; // Standard auf 90 setzen 
            cbxCloseOption.Checked = closeOption;
        }

        private void BtnFolderBrowser_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog fbd = new();
            fbd.Description = "Wählen Sie den Zielordner aus";
            fbd.UseDescriptionForTitle = true; // Zeigt die Beschreibung oben im Dialog an
            if (Directory.Exists(tbxTargetDir.Text)) { fbd.SelectedPath = tbxTargetDir.Text; }
            else { fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); }
            if (fbd.ShowDialog(this) == DialogResult.OK) { tbxTargetDir.Text = fbd.SelectedPath; }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string fileName = tbxImageName.Text.Trim();
            char[] invalidChars = Path.GetInvalidFileNameChars();
            if (string.IsNullOrWhiteSpace(fileName)) { tbxImageName.Text = "Profilbild"; }
            else if (fileName.IndexOfAny(invalidChars) >= 0)
            {
                string illegal = string.Join(" ", invalidChars.Where(c => !char.IsControl(c)));
                Util.MsgTaskDlg(Handle, "Ungültiger Dateiname", $"Der Dateiname enthält ungültige Zeichen.\nFolgende Zeichen sind nicht erlaubt:\n{illegal}");
                DialogResult = DialogResult.None;
                tbxImageName.Focus();
                return;
            }
            else { tbxImageName.Text = fileName; } // getrimmten Namen zurücksetzen
            if (string.IsNullOrWhiteSpace(tbxTargetDir.Text)) { tbxTargetDir.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); }
            if (!Directory.Exists(tbxTargetDir.Text))
            {
                Util.MsgTaskDlg(Handle, "Ungültiger Pfad", "Der gewählte Zielordner existiert nicht. Bitte wählen Sie einen gültigen Pfad.");
                DialogResult = DialogResult.None;
                tbxTargetDir.Focus();
                return;
            }
        }

        private void CbxJPEGQuality_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) { return; }
            ComboBox combo = (ComboBox)sender;
            object? item = combo.Items[e.Index];
            string text = item?.ToString() ?? string.Empty;
            e.DrawBackground();
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            Font font = e.Font ?? combo.Font ?? SystemFonts.DefaultFont;
            using (StringFormat sf = new()) // Text-Ausrichtung auf Zentriert setzen
            {
                sf.Alignment = StringAlignment.Center;     // Horizontal zentriert
                sf.LineAlignment = StringAlignment.Center; // Vertikal zentriert
                using Brush brush = new SolidBrush(e.ForeColor);
                e.Graphics.DrawString(text, font, brush, e.Bounds, sf);
            }
            e.DrawFocusRectangle();  // Fokus-Rechteck zeichnen (die gepunktete Linie)
        }

        private void BtnDefault_Paint(object sender, PaintEventArgs e)
        {
            Button btn = (Button)sender;
            string symbol = "↻";
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            using Font iconFont = new Font("Segoe UI Symbol", 11F, FontStyle.Regular);
            SizeF textSize = e.Graphics.MeasureString(symbol, iconFont);
            float x = (btn.ClientRectangle.Width - textSize.Width) / 2;
            float y = (btn.ClientRectangle.Height - textSize.Height) / 2;
            y -= 1; // Falls es optisch immer noch 1px zu tief wirkt, hier manuell korrigieren:
            e.Graphics.DrawString(symbol, iconFont, Brushes.Black, x, y);
        }

        private void BtnWidthDefault_Click(object sender, EventArgs e) => numericUpDown.Value = 320;

        private void BtnJPEGDefault_Click(object sender, EventArgs e) => cbxJPEGQuality.SelectedIndex = 3; // Standard auf 90 setzen 

        private void BtnNameDefault_Click(object sender, EventArgs e) => tbxImageName.Text = "Profilbild";

    }
}
