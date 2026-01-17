namespace ImageCropper
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            menuStrip = new MenuStrip();
            openToolStripMenuItem = new ToolStripMenuItem();
            cropToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            standardToolStripMenuItem = new ToolStripMenuItem();
            statusStrip = new StatusStrip();
            toolStripStatusSize = new ToolStripStatusLabel();
            pictureBox = new PictureBox();
            contextMenuStrip = new ContextMenuStrip(components);
            clipContextMenuItem = new ToolStripMenuItem();
            cropContextMenuItem = new ToolStripMenuItem();
            cancelContextMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            ratio1to1ToolStripMenuItem = new ToolStripMenuItem();
            ratio2to3ToolStripMenuItem = new ToolStripMenuItem();
            ratio3to4ToolStripMenuItem = new ToolStripMenuItem();
            ratioFreeToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog = new OpenFileDialog();
            toolTip = new ToolTip(components);
            menuStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            contextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Font = new Font("Segoe UI", 10F);
            menuStrip.ImageScalingSize = new Size(24, 24);
            menuStrip.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem, cropToolStripMenuItem, helpToolStripMenuItem, settingsToolStripMenuItem, standardToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.ShowItemToolTips = true;
            menuStrip.Size = new Size(460, 32);
            menuStrip.TabIndex = 0;
            menuStrip.ClientSizeChanged += MenuStrip_ClientSizeChanged;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Font = new Font("Segoe UI", 10F);
            openToolStripMenuItem.Image = Properties.Resources.image_import;
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(134, 28);
            openToolStripMenuItem.Text = "Bilddatei laden";
            openToolStripMenuItem.ToolTipText = "Strg+N";
            openToolStripMenuItem.Click += OpenToolStripMenuItem_Click;
            // 
            // cropToolStripMenuItem
            // 
            cropToolStripMenuItem.Enabled = false;
            cropToolStripMenuItem.Font = new Font("Segoe UI", 10F);
            cropToolStripMenuItem.Image = Properties.Resources.image_export;
            cropToolStripMenuItem.Name = "cropToolStripMenuItem";
            cropToolStripMenuItem.Size = new Size(212, 28);
            cropToolStripMenuItem.Text = "Zuschneiden und speichern";
            cropToolStripMenuItem.ToolTipText = "Strg+S";
            cropToolStripMenuItem.Click += CropToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.Alignment = ToolStripItemAlignment.Right;
            helpToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            helpToolStripMenuItem.Font = new Font("Segoe UI", 10F);
            helpToolStripMenuItem.Image = Properties.Resources.Help_24x;
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(36, 28);
            helpToolStripMenuItem.Text = "Hilfe";
            helpToolStripMenuItem.ToolTipText = "F1: About-Dialog\r\nF2: Einstellungen\r\nF3: Settings.xml";
            helpToolStripMenuItem.Click += HelpToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Alignment = ToolStripItemAlignment.Right;
            settingsToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            settingsToolStripMenuItem.Image = Properties.Resources.settings_24;
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(36, 28);
            settingsToolStripMenuItem.Text = "Einstellungen";
            settingsToolStripMenuItem.ToolTipText = "Einstellungen";
            settingsToolStripMenuItem.Click += SettingsToolStripMenuItem_Click;
            // 
            // standardToolStripMenuItem
            // 
            standardToolStripMenuItem.Alignment = ToolStripItemAlignment.Right;
            standardToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            standardToolStripMenuItem.Enabled = false;
            standardToolStripMenuItem.Image = Properties.Resources.picture_24;
            standardToolStripMenuItem.Name = "standardToolStripMenuItem";
            standardToolStripMenuItem.Size = new Size(36, 28);
            standardToolStripMenuItem.Text = "Bildbearbeitung";
            standardToolStripMenuItem.ToolTipText = "In Bildbearbeitungsprogramm laden";
            standardToolStripMenuItem.Click += StandardToolStripMenuItem_Click;
            // 
            // statusStrip
            // 
            statusStrip.Font = new Font("Segoe UI", 10F);
            statusStrip.GripStyle = ToolStripGripStyle.Visible;
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusSize });
            statusStrip.Location = new Point(0, 492);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(460, 22);
            statusStrip.TabIndex = 1;
            // 
            // toolStripStatusSize
            // 
            toolStripStatusSize.Name = "toolStripStatusSize";
            toolStripStatusSize.Size = new Size(445, 17);
            toolStripStatusSize.Spring = true;
            // 
            // pictureBox
            // 
            pictureBox.BackColor = SystemColors.ControlDark;
            pictureBox.ContextMenuStrip = contextMenuStrip;
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.Location = new Point(0, 32);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(460, 460);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabIndex = 2;
            pictureBox.TabStop = false;
            pictureBox.SizeChanged += PictureBox_SizeChanged;
            pictureBox.Paint += PictureBox_Paint;
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseUp += PictureBox_MouseUp;
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { clipContextMenuItem, cropContextMenuItem, cancelContextMenuItem, toolStripSeparator1, ratio1to1ToolStripMenuItem, ratio2to3ToolStripMenuItem, ratio3to4ToolStripMenuItem, ratioFreeToolStripMenuItem });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new Size(233, 164);
            // 
            // clipContextMenuItem
            // 
            clipContextMenuItem.Enabled = false;
            clipContextMenuItem.Image = Properties.Resources.clipboard_image;
            clipContextMenuItem.Name = "clipContextMenuItem";
            clipContextMenuItem.ShortcutKeyDisplayString = "Strg+C";
            clipContextMenuItem.Size = new Size(232, 22);
            clipContextMenuItem.Text = "Ausschnitt kopieren";
            clipContextMenuItem.Click += ClipContextMenuItem_Click;
            // 
            // cropContextMenuItem
            // 
            cropContextMenuItem.Enabled = false;
            cropContextMenuItem.Image = Properties.Resources.image_exp16;
            cropContextMenuItem.Name = "cropContextMenuItem";
            cropContextMenuItem.ShortcutKeyDisplayString = "Enter";
            cropContextMenuItem.Size = new Size(232, 22);
            cropContextMenuItem.Text = "Ausschnitt speichern";
            cropContextMenuItem.Click += CropContextMenuItem_Click;
            // 
            // cancelContextMenuItem
            // 
            cancelContextMenuItem.Enabled = false;
            cancelContextMenuItem.Image = Properties.Resources.cancel_16LG;
            cancelContextMenuItem.Name = "cancelContextMenuItem";
            cancelContextMenuItem.ShortcutKeyDisplayString = "Escape";
            cancelContextMenuItem.Size = new Size(232, 22);
            cancelContextMenuItem.Text = "Selektion aufheben";
            cancelContextMenuItem.Click += CancelContextMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(229, 6);
            // 
            // ratio1to1ToolStripMenuItem
            // 
            ratio1to1ToolStripMenuItem.CheckOnClick = true;
            ratio1to1ToolStripMenuItem.Name = "ratio1to1ToolStripMenuItem";
            ratio1to1ToolStripMenuItem.ShortcutKeyDisplayString = "Strg+1";
            ratio1to1ToolStripMenuItem.Size = new Size(232, 22);
            ratio1to1ToolStripMenuItem.Text = "Seitenverhältnis 1:1";
            ratio1to1ToolStripMenuItem.Click += Ratio1to1ToolStripMenuItem_Click;
            // 
            // ratio2to3ToolStripMenuItem
            // 
            ratio2to3ToolStripMenuItem.CheckOnClick = true;
            ratio2to3ToolStripMenuItem.Name = "ratio2to3ToolStripMenuItem";
            ratio2to3ToolStripMenuItem.ShortcutKeyDisplayString = "Strg+2";
            ratio2to3ToolStripMenuItem.Size = new Size(232, 22);
            ratio2to3ToolStripMenuItem.Text = "Seitenverhältnis 2:3";
            ratio2to3ToolStripMenuItem.Click += Ratio2to3ToolStripMenuItem_Click;
            // 
            // ratio3to4ToolStripMenuItem
            // 
            ratio3to4ToolStripMenuItem.CheckOnClick = true;
            ratio3to4ToolStripMenuItem.Name = "ratio3to4ToolStripMenuItem";
            ratio3to4ToolStripMenuItem.ShortcutKeyDisplayString = "Strg+3";
            ratio3to4ToolStripMenuItem.Size = new Size(232, 22);
            ratio3to4ToolStripMenuItem.Text = "Seitenverhältnis 3:4";
            ratio3to4ToolStripMenuItem.Click += Ratio3to4ToolStripMenuItem_Click;
            // 
            // ratioFreeToolStripMenuItem
            // 
            ratioFreeToolStripMenuItem.Checked = true;
            ratioFreeToolStripMenuItem.CheckOnClick = true;
            ratioFreeToolStripMenuItem.CheckState = CheckState.Checked;
            ratioFreeToolStripMenuItem.Name = "ratioFreeToolStripMenuItem";
            ratioFreeToolStripMenuItem.ShortcutKeyDisplayString = "Strg+0";
            ratioFreeToolStripMenuItem.Size = new Size(232, 22);
            ratioFreeToolStripMenuItem.Text = "Freies Seitenverhältnis";
            ratioFreeToolStripMenuItem.Click += RationFreeToolStripMenuItem_Click;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "Image";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowPreview = true;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(460, 514);
            Controls.Add(pictureBox);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MainMenuStrip = menuStrip;
            MinimumSize = new Size(476, 300);
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ImageCropper";
            Load += Main_Load;
            DragDrop += Main_DragDrop;
            DragEnter += Main_DragEnter;
            KeyDown += Main_KeyDown;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private StatusStrip statusStrip;
        private PictureBox pictureBox;
        private OpenFileDialog openFileDialog;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem cropToolStripMenuItem;
        private ToolStripStatusLabel toolStripStatusSize;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem cropContextMenuItem;
        private ToolStripMenuItem cancelContextMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem ratio1to1ToolStripMenuItem;
        private ToolStripMenuItem ratio2to3ToolStripMenuItem;
        private ToolStripMenuItem ratio3to4ToolStripMenuItem;
        private ToolStripMenuItem ratioFreeToolStripMenuItem;
        private ToolTip toolTip;
        private ToolStripMenuItem clipContextMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem standardToolStripMenuItem;
    }
}
