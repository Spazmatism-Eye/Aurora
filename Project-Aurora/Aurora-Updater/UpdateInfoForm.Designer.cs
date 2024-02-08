namespace Aurora_Updater
{
    partial class UpdateInfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateInfoForm));
            labelCurrentVersion = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            richTextBoxChangelog = new System.Windows.Forms.RichTextBox();
            labelChangelogTitle = new System.Windows.Forms.Label();
            buttonCancel = new System.Windows.Forms.Button();
            buttonInstall = new System.Windows.Forms.Button();
            pictureBoxApplicationLogo = new System.Windows.Forms.PictureBox();
            lblUpdateTitle = new System.Windows.Forms.Label();
            labelUpdateDescription = new System.Windows.Forms.Label();
            linkLabelViewHistory = new System.Windows.Forms.LinkLabel();
            labelUpdateSize = new System.Windows.Forms.Label();
            skipButton = new System.Windows.Forms.Button();
            lblDownloadCount = new System.Windows.Forms.Label();
            lblUpdateTime = new System.Windows.Forms.Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxApplicationLogo).BeginInit();
            SuspendLayout();
            // 
            // labelCurrentVersion
            // 
            labelCurrentVersion.AutoSize = true;
            labelCurrentVersion.Location = new System.Drawing.Point(14, 73);
            labelCurrentVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelCurrentVersion.Name = "labelCurrentVersion";
            labelCurrentVersion.Size = new System.Drawing.Size(131, 15);
            labelCurrentVersion.TabIndex = 0;
            labelCurrentVersion.Text = "Installed Version: #.#.#x";
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel1.Controls.Add(richTextBoxChangelog);
            panel1.Location = new System.Drawing.Point(14, 133);
            panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(653, 352);
            panel1.TabIndex = 2;
            // 
            // richTextBoxChangelog
            // 
            richTextBoxChangelog.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            richTextBoxChangelog.BackColor = System.Drawing.SystemColors.ControlLightLight;
            richTextBoxChangelog.Location = new System.Drawing.Point(0, 0);
            richTextBoxChangelog.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            richTextBoxChangelog.Name = "richTextBoxChangelog";
            richTextBoxChangelog.ReadOnly = true;
            richTextBoxChangelog.Size = new System.Drawing.Size(653, 351);
            richTextBoxChangelog.TabIndex = 1;
            richTextBoxChangelog.Text = "";
            // 
            // labelChangelogTitle
            // 
            labelChangelogTitle.AutoSize = true;
            labelChangelogTitle.Location = new System.Drawing.Point(14, 115);
            labelChangelogTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelChangelogTitle.Name = "labelChangelogTitle";
            labelChangelogTitle.Size = new System.Drawing.Size(68, 15);
            labelChangelogTitle.TabIndex = 0;
            labelChangelogTitle.Text = "Change log";
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonCancel.Location = new System.Drawing.Point(580, 492);
            buttonCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(88, 27);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonInstall
            // 
            buttonInstall.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonInstall.Location = new System.Drawing.Point(388, 492);
            buttonInstall.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonInstall.Name = "buttonInstall";
            buttonInstall.Size = new System.Drawing.Size(88, 27);
            buttonInstall.TabIndex = 4;
            buttonInstall.Text = "Install";
            buttonInstall.UseVisualStyleBackColor = true;
            buttonInstall.Click += buttonInstall_Click;
            // 
            // pictureBoxApplicationLogo
            // 
            pictureBoxApplicationLogo.Image = Properties.Resources.Aurora_updater_logo;
            pictureBoxApplicationLogo.Location = new System.Drawing.Point(14, 14);
            pictureBoxApplicationLogo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBoxApplicationLogo.Name = "pictureBoxApplicationLogo";
            pictureBoxApplicationLogo.Size = new System.Drawing.Size(48, 48);
            pictureBoxApplicationLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pictureBoxApplicationLogo.TabIndex = 5;
            pictureBoxApplicationLogo.TabStop = false;
            // 
            // lblUpdateTitle
            // 
            lblUpdateTitle.AutoSize = true;
            lblUpdateTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblUpdateTitle.Location = new System.Drawing.Point(77, 14);
            lblUpdateTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblUpdateTitle.Name = "lblUpdateTitle";
            lblUpdateTitle.Size = new System.Drawing.Size(261, 20);
            lblUpdateTitle.TabIndex = 6;
            lblUpdateTitle.Text = "New Aurora update is available!";
            // 
            // labelUpdateDescription
            // 
            labelUpdateDescription.AutoSize = true;
            labelUpdateDescription.Location = new System.Drawing.Point(78, 37);
            labelUpdateDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelUpdateDescription.Name = "labelUpdateDescription";
            labelUpdateDescription.Size = new System.Drawing.Size(117, 15);
            labelUpdateDescription.TabIndex = 7;
            labelUpdateDescription.Text = "$UpdateDescription$";
            // 
            // linkLabelViewHistory
            // 
            linkLabelViewHistory.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            linkLabelViewHistory.AutoSize = true;
            linkLabelViewHistory.LinkArea = new System.Windows.Forms.LinkArea(0, 21);
            linkLabelViewHistory.Location = new System.Drawing.Point(14, 497);
            linkLabelViewHistory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            linkLabelViewHistory.Name = "linkLabelViewHistory";
            linkLabelViewHistory.Size = new System.Drawing.Size(125, 15);
            linkLabelViewHistory.TabIndex = 8;
            linkLabelViewHistory.TabStop = true;
            linkLabelViewHistory.Text = "View previous updates";
            linkLabelViewHistory.LinkClicked += linkLabelViewHistory_LinkClicked;
            // 
            // labelUpdateSize
            // 
            labelUpdateSize.AutoSize = true;
            labelUpdateSize.Location = new System.Drawing.Point(13, 90);
            labelUpdateSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelUpdateSize.Name = "labelUpdateSize";
            labelUpdateSize.Size = new System.Drawing.Size(166, 15);
            labelUpdateSize.TabIndex = 9;
            labelUpdateSize.Text = "Update Download Size: ## MB";
            // 
            // skipButton
            // 
            skipButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            skipButton.Location = new System.Drawing.Point(484, 492);
            skipButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            skipButton.Name = "skipButton";
            skipButton.Size = new System.Drawing.Size(88, 27);
            skipButton.TabIndex = 10;
            skipButton.Text = "Skip Version";
            skipButton.UseVisualStyleBackColor = true;
            skipButton.Click += skipButton_Click;
            // 
            // lblDownloadCount
            // 
            lblDownloadCount.AutoSize = true;
            lblDownloadCount.Location = new System.Drawing.Point(200, 90);
            lblDownloadCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblDownloadCount.Name = "lblDownloadCount";
            lblDownloadCount.Size = new System.Drawing.Size(110, 15);
            lblDownloadCount.TabIndex = 11;
            lblDownloadCount.Text = "Download Count: #";
            // 
            // lblUpdateTime
            // 
            lblUpdateTime.AutoSize = true;
            lblUpdateTime.Location = new System.Drawing.Point(200, 73);
            lblUpdateTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblUpdateTime.Name = "lblUpdateTime";
            lblUpdateTime.Size = new System.Drawing.Size(85, 15);
            lblUpdateTime.TabIndex = 11;
            lblUpdateTime.Text = "Release date: #";
            // 
            // UpdateInfoForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(681, 532);
            Controls.Add(lblUpdateTime);
            Controls.Add(labelChangelogTitle);
            Controls.Add(lblDownloadCount);
            Controls.Add(skipButton);
            Controls.Add(labelUpdateSize);
            Controls.Add(linkLabelViewHistory);
            Controls.Add(labelUpdateDescription);
            Controls.Add(lblUpdateTitle);
            Controls.Add(pictureBoxApplicationLogo);
            Controls.Add(buttonInstall);
            Controls.Add(buttonCancel);
            Controls.Add(panel1);
            Controls.Add(labelCurrentVersion);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(697, 571);
            MinimumSize = new System.Drawing.Size(697, 571);
            Name = "UpdateInfoForm";
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Aurora Updater";
            Shown += UpdateInfoForm_Shown;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxApplicationLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelCurrentVersion;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox richTextBoxChangelog;
        private System.Windows.Forms.Label labelChangelogTitle;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonInstall;
        private System.Windows.Forms.PictureBox pictureBoxApplicationLogo;
        private System.Windows.Forms.Label lblUpdateTitle;
        private System.Windows.Forms.Label labelUpdateDescription;
        private System.Windows.Forms.LinkLabel linkLabelViewHistory;
        private System.Windows.Forms.Label labelUpdateSize;
        private System.Windows.Forms.Button skipButton;
        private System.Windows.Forms.Label lblDownloadCount;
        private System.Windows.Forms.Label lblUpdateTime;
    }
}