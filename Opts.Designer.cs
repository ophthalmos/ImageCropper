namespace ImageCropper
{
    partial class Opts
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            gbxRatio = new GroupBox();
            rBtn3to4 = new RadioButton();
            rBtn2to3 = new RadioButton();
            rBtn1to1 = new RadioButton();
            rBtnFree = new RadioButton();
            gbxTargetWidth = new GroupBox();
            btnJPEGDefault = new Button();
            btnWidthDefault = new Button();
            btnNameDefault = new Button();
            btnFolderBrowser = new Button();
            tbxImageName = new TextBox();
            tbxTargetDir = new TextBox();
            cbxJPEGQuality = new ComboBox();
            numericUpDown = new NumericUpDown();
            lblJPEG = new Label();
            lblImageName = new Label();
            lblTargetWidth = new Label();
            btnSave = new Button();
            btnCancel = new Button();
            gbxCloseOption = new GroupBox();
            lblCloseOption = new Label();
            cbxCloseOption = new CheckBox();
            panel = new Panel();
            gbxRatio.SuspendLayout();
            gbxTargetWidth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown).BeginInit();
            gbxCloseOption.SuspendLayout();
            SuspendLayout();
            // 
            // gbxRatio
            // 
            gbxRatio.BackColor = SystemColors.ControlLightLight;
            gbxRatio.Controls.Add(rBtn3to4);
            gbxRatio.Controls.Add(rBtn2to3);
            gbxRatio.Controls.Add(rBtn1to1);
            gbxRatio.Controls.Add(rBtnFree);
            gbxRatio.Font = new Font("Segoe UI", 8F);
            gbxRatio.Location = new Point(12, 12);
            gbxRatio.Name = "gbxRatio";
            gbxRatio.Size = new Size(218, 55);
            gbxRatio.TabIndex = 0;
            gbxRatio.TabStop = false;
            gbxRatio.Text = "Auswahl-Seitenverhältnis";
            // 
            // rBtn3to4
            // 
            rBtn3to4.AutoSize = true;
            rBtn3to4.Font = new Font("Segoe UI", 10F);
            rBtn3to4.Location = new Point(165, 24);
            rBtn3to4.Name = "rBtn3to4";
            rBtn3to4.Size = new Size(46, 23);
            rBtn3to4.TabIndex = 3;
            rBtn3to4.TabStop = true;
            rBtn3to4.Text = "3:4";
            rBtn3to4.UseVisualStyleBackColor = true;
            // 
            // rBtn2to3
            // 
            rBtn2to3.AutoSize = true;
            rBtn2to3.Font = new Font("Segoe UI", 10F);
            rBtn2to3.Location = new Point(113, 24);
            rBtn2to3.Name = "rBtn2to3";
            rBtn2to3.Size = new Size(46, 23);
            rBtn2to3.TabIndex = 2;
            rBtn2to3.TabStop = true;
            rBtn2to3.Text = "2:3";
            rBtn2to3.UseVisualStyleBackColor = true;
            // 
            // rBtn1to1
            // 
            rBtn1to1.AutoSize = true;
            rBtn1to1.Font = new Font("Segoe UI", 10F);
            rBtn1to1.Location = new Point(61, 24);
            rBtn1to1.Name = "rBtn1to1";
            rBtn1to1.Size = new Size(46, 23);
            rBtn1to1.TabIndex = 1;
            rBtn1to1.TabStop = true;
            rBtn1to1.Text = "1:1";
            rBtn1to1.UseVisualStyleBackColor = true;
            // 
            // rBtnFree
            // 
            rBtnFree.AutoSize = true;
            rBtnFree.Checked = true;
            rBtnFree.Font = new Font("Segoe UI", 10F);
            rBtnFree.Location = new Point(6, 24);
            rBtnFree.Name = "rBtnFree";
            rBtnFree.Size = new Size(49, 23);
            rBtnFree.TabIndex = 0;
            rBtnFree.TabStop = true;
            rBtnFree.Text = "Frei";
            rBtnFree.UseVisualStyleBackColor = true;
            // 
            // gbxTargetWidth
            // 
            gbxTargetWidth.BackColor = SystemColors.ControlLightLight;
            gbxTargetWidth.Controls.Add(btnJPEGDefault);
            gbxTargetWidth.Controls.Add(btnWidthDefault);
            gbxTargetWidth.Controls.Add(btnNameDefault);
            gbxTargetWidth.Controls.Add(btnFolderBrowser);
            gbxTargetWidth.Controls.Add(tbxImageName);
            gbxTargetWidth.Controls.Add(tbxTargetDir);
            gbxTargetWidth.Controls.Add(cbxJPEGQuality);
            gbxTargetWidth.Controls.Add(numericUpDown);
            gbxTargetWidth.Controls.Add(lblJPEG);
            gbxTargetWidth.Controls.Add(lblImageName);
            gbxTargetWidth.Controls.Add(lblTargetWidth);
            gbxTargetWidth.Font = new Font("Segoe UI", 8F);
            gbxTargetWidth.Location = new Point(12, 73);
            gbxTargetWidth.Name = "gbxTargetWidth";
            gbxTargetWidth.Size = new Size(217, 152);
            gbxTargetWidth.TabIndex = 1;
            gbxTargetWidth.TabStop = false;
            gbxTargetWidth.Text = "Speichern der Zieldatei";
            // 
            // btnJPEGDefault
            // 
            btnJPEGDefault.FlatAppearance.BorderColor = SystemColors.ControlDark;
            btnJPEGDefault.FlatAppearance.MouseDownBackColor = Color.FromArgb(100, SystemColors.Highlight);
            btnJPEGDefault.FlatAppearance.MouseOverBackColor = SystemColors.GradientActiveCaption;
            btnJPEGDefault.FlatStyle = FlatStyle.Flat;
            btnJPEGDefault.Location = new Point(185, 118);
            btnJPEGDefault.Name = "btnJPEGDefault";
            btnJPEGDefault.Size = new Size(26, 26);
            btnJPEGDefault.TabIndex = 9;
            btnJPEGDefault.UseVisualStyleBackColor = true;
            btnJPEGDefault.Click += BtnJPEGDefault_Click;
            btnJPEGDefault.Paint += BtnDefault_Paint;
            // 
            // btnWidthDefault
            // 
            btnWidthDefault.FlatAppearance.BorderColor = SystemColors.ControlDark;
            btnWidthDefault.FlatAppearance.MouseDownBackColor = Color.FromArgb(100, SystemColors.Highlight);
            btnWidthDefault.FlatAppearance.MouseOverBackColor = SystemColors.GradientActiveCaption;
            btnWidthDefault.FlatStyle = FlatStyle.Flat;
            btnWidthDefault.Location = new Point(185, 24);
            btnWidthDefault.Name = "btnWidthDefault";
            btnWidthDefault.Size = new Size(26, 25);
            btnWidthDefault.TabIndex = 10;
            btnWidthDefault.UseVisualStyleBackColor = true;
            btnWidthDefault.Click += BtnWidthDefault_Click;
            btnWidthDefault.Paint += BtnDefault_Paint;
            // 
            // btnNameDefault
            // 
            btnNameDefault.FlatAppearance.BorderColor = SystemColors.ControlDark;
            btnNameDefault.FlatAppearance.MouseDownBackColor = Color.FromArgb(100, SystemColors.Highlight);
            btnNameDefault.FlatAppearance.MouseOverBackColor = SystemColors.GradientActiveCaption;
            btnNameDefault.FlatStyle = FlatStyle.Flat;
            btnNameDefault.Location = new Point(185, 55);
            btnNameDefault.Name = "btnNameDefault";
            btnNameDefault.Size = new Size(26, 25);
            btnNameDefault.TabIndex = 11;
            btnNameDefault.UseVisualStyleBackColor = true;
            btnNameDefault.Click += BtnNameDefault_Click;
            btnNameDefault.Paint += BtnDefault_Paint;
            // 
            // btnFolderBrowser
            // 
            btnFolderBrowser.FlatAppearance.BorderColor = SystemColors.ControlDark;
            btnFolderBrowser.FlatAppearance.MouseDownBackColor = Color.FromArgb(100, SystemColors.Highlight);
            btnFolderBrowser.FlatAppearance.MouseOverBackColor = SystemColors.GradientActiveCaption;
            btnFolderBrowser.FlatStyle = FlatStyle.Flat;
            btnFolderBrowser.Location = new Point(185, 86);
            btnFolderBrowser.Name = "btnFolderBrowser";
            btnFolderBrowser.Size = new Size(26, 25);
            btnFolderBrowser.TabIndex = 6;
            btnFolderBrowser.Text = "⚙";
            btnFolderBrowser.UseVisualStyleBackColor = true;
            btnFolderBrowser.Click += BtnFolderBrowser_Click;
            // 
            // tbxImageName
            // 
            tbxImageName.Font = new Font("Segoe UI", 10F);
            tbxImageName.Location = new Point(80, 55);
            tbxImageName.Name = "tbxImageName";
            tbxImageName.PlaceholderText = "Profilbild";
            tbxImageName.Size = new Size(106, 25);
            tbxImageName.TabIndex = 2;
            // 
            // tbxTargetDir
            // 
            tbxTargetDir.Font = new Font("Segoe UI", 10F);
            tbxTargetDir.Location = new Point(6, 86);
            tbxTargetDir.Name = "tbxTargetDir";
            tbxTargetDir.PlaceholderText = "Desktop";
            tbxTargetDir.Size = new Size(181, 25);
            tbxTargetDir.TabIndex = 5;
            // 
            // cbxJPEGQuality
            // 
            cbxJPEGQuality.DrawMode = DrawMode.OwnerDrawFixed;
            cbxJPEGQuality.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxJPEGQuality.Font = new Font("Segoe UI", 10F);
            cbxJPEGQuality.FormattingEnabled = true;
            cbxJPEGQuality.Items.AddRange(new object[] { "75", "80", "85", "90", "95" });
            cbxJPEGQuality.Location = new Point(113, 118);
            cbxJPEGQuality.Name = "cbxJPEGQuality";
            cbxJPEGQuality.Size = new Size(73, 26);
            cbxJPEGQuality.TabIndex = 7;
            cbxJPEGQuality.DrawItem += CbxJPEGQuality_DrawItem;
            // 
            // numericUpDown
            // 
            numericUpDown.Font = new Font("Segoe UI", 10F);
            numericUpDown.Location = new Point(113, 24);
            numericUpDown.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericUpDown.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown.Name = "numericUpDown";
            numericUpDown.Size = new Size(73, 25);
            numericUpDown.TabIndex = 0;
            numericUpDown.TextAlign = HorizontalAlignment.Center;
            numericUpDown.Value = new decimal(new int[] { 320, 0, 0, 0 });
            // 
            // lblJPEG
            // 
            lblJPEG.AutoSize = true;
            lblJPEG.Font = new Font("Segoe UI", 10F);
            lblJPEG.Location = new Point(6, 121);
            lblJPEG.Name = "lblJPEG";
            lblJPEG.Size = new Size(97, 19);
            lblJPEG.TabIndex = 8;
            lblJPEG.Text = "JPEG-Qualität:";
            // 
            // lblImageName
            // 
            lblImageName.AutoSize = true;
            lblImageName.Font = new Font("Segoe UI", 10F);
            lblImageName.Location = new Point(6, 58);
            lblImageName.Name = "lblImageName";
            lblImageName.Size = new Size(68, 19);
            lblImageName.TabIndex = 3;
            lblImageName.Text = "Bildname:";
            // 
            // lblTargetWidth
            // 
            lblTargetWidth.AutoSize = true;
            lblTargetWidth.Font = new Font("Segoe UI", 10F);
            lblTargetWidth.Location = new Point(6, 26);
            lblTargetWidth.Name = "lblTargetWidth";
            lblTargetWidth.Size = new Size(101, 19);
            lblTargetWidth.TabIndex = 1;
            lblTargetWidth.Text = "Breite in Pixeln:";
            // 
            // btnSave
            // 
            btnSave.DialogResult = DialogResult.OK;
            btnSave.Location = new Point(8, 317);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(107, 27);
            btnSave.TabIndex = 2;
            btnSave.Text = "Speichern";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += BtnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(125, 317);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(105, 27);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Abbrechen";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbxCloseOption
            // 
            gbxCloseOption.BackColor = SystemColors.ControlLightLight;
            gbxCloseOption.Controls.Add(lblCloseOption);
            gbxCloseOption.Controls.Add(cbxCloseOption);
            gbxCloseOption.Font = new Font("Segoe UI", 8F);
            gbxCloseOption.Location = new Point(12, 231);
            gbxCloseOption.Name = "gbxCloseOption";
            gbxCloseOption.Size = new Size(218, 74);
            gbxCloseOption.TabIndex = 4;
            gbxCloseOption.TabStop = false;
            gbxCloseOption.Text = "Erfolgreich-gespeichert-Dialog";
            // 
            // lblCloseOption
            // 
            lblCloseOption.AutoSize = true;
            lblCloseOption.Font = new Font("Segoe UI", 10F);
            lblCloseOption.Location = new Point(6, 50);
            lblCloseOption.Name = "lblCloseOption";
            lblCloseOption.Size = new Size(202, 19);
            lblCloseOption.TabIndex = 1;
            lblCloseOption.Text = "Andernfalls nur Dialog beenden";
            // 
            // cbxCloseOption
            // 
            cbxCloseOption.AutoSize = true;
            cbxCloseOption.Checked = true;
            cbxCloseOption.CheckState = CheckState.Checked;
            cbxCloseOption.Font = new Font("Segoe UI", 10F);
            cbxCloseOption.Location = new Point(6, 24);
            cbxCloseOption.Name = "cbxCloseOption";
            cbxCloseOption.Size = new Size(204, 23);
            cbxCloseOption.TabIndex = 0;
            cbxCloseOption.Text = "„Beenden“ = Programmende";
            cbxCloseOption.UseVisualStyleBackColor = true;
            // 
            // panel
            // 
            panel.BackColor = SystemColors.ControlLightLight;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Dock = DockStyle.Top;
            panel.Location = new Point(0, 0);
            panel.Name = "panel";
            panel.Size = new Size(240, 311);
            panel.TabIndex = 9;
            // 
            // Opts
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(240, 356);
            ControlBox = false;
            Controls.Add(gbxCloseOption);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(gbxTargetWidth);
            Controls.Add(gbxRatio);
            Controls.Add(panel);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Opts";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Einstellungen";
            gbxRatio.ResumeLayout(false);
            gbxRatio.PerformLayout();
            gbxTargetWidth.ResumeLayout(false);
            gbxTargetWidth.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown).EndInit();
            gbxCloseOption.ResumeLayout(false);
            gbxCloseOption.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbxRatio;
        private RadioButton rBtn2to3;
        private RadioButton rBtn1to1;
        private RadioButton rBtnFree;
        private RadioButton rBtn3to4;
        private GroupBox gbxTargetWidth;
        private Label lblTargetWidth;
        private NumericUpDown numericUpDown;
        private Label lblImageName;
        private TextBox tbxImageName;
        private TextBox tbxTargetDir;
        private Button btnSave;
        private Button btnCancel;
        private Button btnFolderBrowser;
        private Label lblJPEG;
        private ComboBox cbxJPEGQuality;
        private GroupBox gbxCloseOption;
        private CheckBox cbxCloseOption;
        private Label lblCloseOption;
        private Panel panel;
        private Button btnJPEGDefault;
        private Button btnWidthDefault;
        private Button btnNameDefault;
    }
}