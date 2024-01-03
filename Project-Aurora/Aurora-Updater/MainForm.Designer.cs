namespace Aurora_Updater
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            update_progress = new System.Windows.Forms.ProgressBar();
            richtextUpdateLog = new System.Windows.Forms.RichTextBox();
            labelApplicationTitle = new System.Windows.Forms.Label();
            pictureBoxApplicationLogo = new System.Windows.Forms.PictureBox();
            labelUpdateLog = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)pictureBoxApplicationLogo).BeginInit();
            SuspendLayout();
            // 
            // update_progress
            // 
            update_progress.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            update_progress.Location = new System.Drawing.Point(14, 406);
            update_progress.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            update_progress.Name = "update_progress";
            update_progress.Size = new System.Drawing.Size(653, 25);
            update_progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            update_progress.TabIndex = 0;
            // 
            // richtextUpdateLog
            // 
            richtextUpdateLog.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            richtextUpdateLog.Location = new System.Drawing.Point(14, 95);
            richtextUpdateLog.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            richtextUpdateLog.Name = "richtextUpdateLog";
            richtextUpdateLog.ReadOnly = true;
            richtextUpdateLog.Size = new System.Drawing.Size(653, 304);
            richtextUpdateLog.TabIndex = 1;
            richtextUpdateLog.Text = "";
            // 
            // labelApplicationTitle
            // 
            labelApplicationTitle.AutoSize = true;
            labelApplicationTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            labelApplicationTitle.Location = new System.Drawing.Point(77, 14);
            labelApplicationTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelApplicationTitle.Name = "labelApplicationTitle";
            labelApplicationTitle.Size = new System.Drawing.Size(156, 20);
            labelApplicationTitle.TabIndex = 9;
            labelApplicationTitle.Text = "Updating Aurora...";
            // 
            // pictureBoxApplicationLogo
            // 
            pictureBoxApplicationLogo.Image = Properties.Resources.Aurora_updater_logo;
            pictureBoxApplicationLogo.Location = new System.Drawing.Point(14, 14);
            pictureBoxApplicationLogo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBoxApplicationLogo.Name = "pictureBoxApplicationLogo";
            pictureBoxApplicationLogo.Size = new System.Drawing.Size(48, 48);
            pictureBoxApplicationLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pictureBoxApplicationLogo.TabIndex = 8;
            pictureBoxApplicationLogo.TabStop = false;
            // 
            // labelUpdateLog
            // 
            labelUpdateLog.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            labelUpdateLog.AutoSize = true;
            labelUpdateLog.Location = new System.Drawing.Point(10, 76);
            labelUpdateLog.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelUpdateLog.Name = "labelUpdateLog";
            labelUpdateLog.Size = new System.Drawing.Size(82, 15);
            labelUpdateLog.TabIndex = 10;
            labelUpdateLog.Text = "Update details";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(681, 440);
            Controls.Add(richtextUpdateLog);
            Controls.Add(labelUpdateLog);
            Controls.Add(labelApplicationTitle);
            Controls.Add(pictureBoxApplicationLogo);
            Controls.Add(update_progress);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(697, 513);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(697, 398);
            Name = "MainForm";
            RightToLeft = System.Windows.Forms.RightToLeft.No;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Aurora Updater";
            Shown += Form1_Shown;
            ((System.ComponentModel.ISupportInitialize)pictureBoxApplicationLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ProgressBar update_progress;
        private System.Windows.Forms.RichTextBox richtextUpdateLog;
        private System.Windows.Forms.Label labelApplicationTitle;
        private System.Windows.Forms.PictureBox pictureBoxApplicationLogo;
        private System.Windows.Forms.Label labelUpdateLog;
    }
}

