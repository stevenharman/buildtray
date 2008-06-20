namespace BuildTray.UI
{
    partial class TestResultForm
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
            this.okButton = new System.Windows.Forms.Button();
            this.FailedTestList = new System.Windows.Forms.ListBox();
            this.outputText = new System.Windows.Forms.TextBox();
            this.failedByLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(451, 374);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // FailedTestList
            // 
            this.FailedTestList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FailedTestList.FormattingEnabled = true;
            this.FailedTestList.Location = new System.Drawing.Point(8, 34);
            this.FailedTestList.Name = "FailedTestList";
            this.FailedTestList.Size = new System.Drawing.Size(518, 186);
            this.FailedTestList.TabIndex = 1;
            this.FailedTestList.SelectedIndexChanged += new System.EventHandler(this.FailedTestList_SelectedIndexChanged);
            // 
            // outputText
            // 
            this.outputText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.outputText.Location = new System.Drawing.Point(8, 228);
            this.outputText.Multiline = true;
            this.outputText.Name = "outputText";
            this.outputText.Size = new System.Drawing.Size(518, 140);
            this.outputText.TabIndex = 2;
            // 
            // failedByLabel
            // 
            this.failedByLabel.AutoSize = true;
            this.failedByLabel.Location = new System.Drawing.Point(5, 9);
            this.failedByLabel.Name = "failedByLabel";
            this.failedByLabel.Size = new System.Drawing.Size(0, 13);
            this.failedByLabel.TabIndex = 3;
            // 
            // TestResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 409);
            this.Controls.Add(this.failedByLabel);
            this.Controls.Add(this.outputText);
            this.Controls.Add(this.FailedTestList);
            this.Controls.Add(this.okButton);
            this.Name = "TestResultForm";
            this.Text = "TestResultForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ListBox FailedTestList;
        private System.Windows.Forms.TextBox outputText;
        private System.Windows.Forms.Label failedByLabel;
    }
}