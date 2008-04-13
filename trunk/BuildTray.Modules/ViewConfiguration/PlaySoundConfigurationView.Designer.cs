namespace BuildTray.Modules.ViewConfiguration
{
    partial class PlaySoundConfigurationView
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.browseSuccessButton = new System.Windows.Forms.Button();
            this.browseFailureButton = new System.Windows.Forms.Button();
            this.browseMultipleFailuresButton = new System.Windows.Forms.Button();
            this.successFileText = new System.Windows.Forms.TextBox();
            this.failureFileText = new System.Windows.Forms.TextBox();
            this.multipleFailureFileText = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.playSuccessButton = new System.Windows.Forms.Button();
            this.playFailureButton = new System.Windows.Forms.Button();
            this.playMultipleFailureButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Success:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Failure:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Multiple Failures:";
            // 
            // browseSuccessButton
            // 
            this.browseSuccessButton.Location = new System.Drawing.Point(248, 12);
            this.browseSuccessButton.Name = "browseSuccessButton";
            this.browseSuccessButton.Size = new System.Drawing.Size(25, 23);
            this.browseSuccessButton.TabIndex = 2;
            this.browseSuccessButton.Text = "...";
            this.browseSuccessButton.UseVisualStyleBackColor = true;
            this.browseSuccessButton.Click += new System.EventHandler(this.browseSuccessButton_Click);
            // 
            // browseFailureButton
            // 
            this.browseFailureButton.Location = new System.Drawing.Point(248, 41);
            this.browseFailureButton.Name = "browseFailureButton";
            this.browseFailureButton.Size = new System.Drawing.Size(25, 23);
            this.browseFailureButton.TabIndex = 6;
            this.browseFailureButton.Text = "...";
            this.browseFailureButton.UseVisualStyleBackColor = true;
            this.browseFailureButton.Click += new System.EventHandler(this.browseFailureButton_Click);
            // 
            // browseMultipleFailuresButton
            // 
            this.browseMultipleFailuresButton.Location = new System.Drawing.Point(248, 70);
            this.browseMultipleFailuresButton.Name = "browseMultipleFailuresButton";
            this.browseMultipleFailuresButton.Size = new System.Drawing.Size(25, 23);
            this.browseMultipleFailuresButton.TabIndex = 10;
            this.browseMultipleFailuresButton.Text = "...";
            this.browseMultipleFailuresButton.UseVisualStyleBackColor = true;
            this.browseMultipleFailuresButton.Click += new System.EventHandler(this.browseMultipleFailuresButton_Click);
            // 
            // successFileText
            // 
            this.successFileText.Location = new System.Drawing.Point(86, 12);
            this.successFileText.Name = "successFileText";
            this.successFileText.ReadOnly = true;
            this.successFileText.Size = new System.Drawing.Size(158, 20);
            this.successFileText.TabIndex = 1;
            // 
            // failureFileText
            // 
            this.failureFileText.Location = new System.Drawing.Point(86, 41);
            this.failureFileText.Name = "failureFileText";
            this.failureFileText.ReadOnly = true;
            this.failureFileText.Size = new System.Drawing.Size(158, 20);
            this.failureFileText.TabIndex = 5;
            // 
            // multipleFailureFileText
            // 
            this.multipleFailureFileText.Location = new System.Drawing.Point(86, 73);
            this.multipleFailureFileText.Name = "multipleFailureFileText";
            this.multipleFailureFileText.ReadOnly = true;
            this.multipleFailureFileText.Size = new System.Drawing.Size(158, 20);
            this.multipleFailureFileText.TabIndex = 9;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(147, 100);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 12;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(228, 100);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 13;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // playSuccessButton
            // 
            this.playSuccessButton.Enabled = false;
            this.playSuccessButton.Location = new System.Drawing.Point(279, 12);
            this.playSuccessButton.Name = "playSuccessButton";
            this.playSuccessButton.Size = new System.Drawing.Size(24, 23);
            this.playSuccessButton.TabIndex = 3;
            this.playSuccessButton.Text = ">";
            this.playSuccessButton.UseVisualStyleBackColor = true;
            this.playSuccessButton.Click += new System.EventHandler(this.playSuccessButton_Click);
            // 
            // playFailureButton
            // 
            this.playFailureButton.Enabled = false;
            this.playFailureButton.Location = new System.Drawing.Point(279, 41);
            this.playFailureButton.Name = "playFailureButton";
            this.playFailureButton.Size = new System.Drawing.Size(24, 23);
            this.playFailureButton.TabIndex = 7;
            this.playFailureButton.Text = ">";
            this.playFailureButton.UseVisualStyleBackColor = true;
            this.playFailureButton.Click += new System.EventHandler(this.playFailureButton_Click);
            // 
            // playMultipleFailureButton
            // 
            this.playMultipleFailureButton.Enabled = false;
            this.playMultipleFailureButton.Location = new System.Drawing.Point(279, 71);
            this.playMultipleFailureButton.Name = "playMultipleFailureButton";
            this.playMultipleFailureButton.Size = new System.Drawing.Size(24, 23);
            this.playMultipleFailureButton.TabIndex = 11;
            this.playMultipleFailureButton.Text = ">";
            this.playMultipleFailureButton.UseVisualStyleBackColor = true;
            this.playMultipleFailureButton.Click += new System.EventHandler(this.playMultipleFailureButton_Click);
            // 
            // PlaySoundConfigurationView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 135);
            this.Controls.Add(this.playMultipleFailureButton);
            this.Controls.Add(this.playFailureButton);
            this.Controls.Add(this.playSuccessButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.multipleFailureFileText);
            this.Controls.Add(this.failureFileText);
            this.Controls.Add(this.successFileText);
            this.Controls.Add(this.browseMultipleFailuresButton);
            this.Controls.Add(this.browseFailureButton);
            this.Controls.Add(this.browseSuccessButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "PlaySoundConfigurationView";
            this.Text = "Sound Module Configuration";
            this.Load += new System.EventHandler(this.PlaySoundConfigurationView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button browseSuccessButton;
        private System.Windows.Forms.Button browseFailureButton;
        private System.Windows.Forms.Button browseMultipleFailuresButton;
        private System.Windows.Forms.TextBox successFileText;
        private System.Windows.Forms.TextBox failureFileText;
        private System.Windows.Forms.TextBox multipleFailureFileText;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button playSuccessButton;
        private System.Windows.Forms.Button playFailureButton;
        private System.Windows.Forms.Button playMultipleFailureButton;

    }
}