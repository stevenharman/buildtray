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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.recordCount = new System.Windows.Forms.Label();
            this.filterCheck = new System.Windows.Forms.CheckBox();
            this.btnClaim = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(460, 170);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // FailedTestList
            // 
            this.FailedTestList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FailedTestList.FormattingEnabled = true;
            this.FailedTestList.Location = new System.Drawing.Point(12, 27);
            this.FailedTestList.Name = "FailedTestList";
            this.FailedTestList.Size = new System.Drawing.Size(523, 173);
            this.FailedTestList.TabIndex = 1;
            this.FailedTestList.SelectedIndexChanged += new System.EventHandler(this.FailedTestList_SelectedIndexChanged);
            // 
            // outputText
            // 
            this.outputText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.outputText.Location = new System.Drawing.Point(12, 3);
            this.outputText.Multiline = true;
            this.outputText.Name = "outputText";
            this.outputText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputText.Size = new System.Drawing.Size(523, 161);
            this.outputText.TabIndex = 2;
            // 
            // failedByLabel
            // 
            this.failedByLabel.AutoSize = true;
            this.failedByLabel.Location = new System.Drawing.Point(12, 7);
            this.failedByLabel.Name = "failedByLabel";
            this.failedByLabel.Size = new System.Drawing.Size(0, 13);
            this.failedByLabel.TabIndex = 3;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.recordCount);
            this.splitContainer1.Panel1.Controls.Add(this.filterCheck);
            this.splitContainer1.Panel1.Controls.Add(this.FailedTestList);
            this.splitContainer1.Panel1.Controls.Add(this.failedByLabel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnClaim);
            this.splitContainer1.Panel2.Controls.Add(this.outputText);
            this.splitContainer1.Panel2.Controls.Add(this.okButton);
            this.splitContainer1.Size = new System.Drawing.Size(538, 409);
            this.splitContainer1.SplitterDistance = 204;
            this.splitContainer1.TabIndex = 4;
            // 
            // recordCount
            // 
            this.recordCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.recordCount.AutoSize = true;
            this.recordCount.Location = new System.Drawing.Point(283, 7);
            this.recordCount.Name = "recordCount";
            this.recordCount.Size = new System.Drawing.Size(0, 13);
            this.recordCount.TabIndex = 5;
            // 
            // filterCheck
            // 
            this.filterCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.filterCheck.AutoSize = true;
            this.filterCheck.Location = new System.Drawing.Point(357, 7);
            this.filterCheck.Name = "filterCheck";
            this.filterCheck.Size = new System.Drawing.Size(169, 17);
            this.filterCheck.TabIndex = 4;
            this.filterCheck.Text = "Filter Timeouts and Deadlocks";
            this.filterCheck.UseVisualStyleBackColor = true;
            this.filterCheck.CheckedChanged += new System.EventHandler(this.filterCheck_CheckedChanged);
            // 
            // btnClaim
            // 
            this.btnClaim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClaim.Location = new System.Drawing.Point(12, 170);
            this.btnClaim.Name = "btnClaim";
            this.btnClaim.Size = new System.Drawing.Size(75, 23);
            this.btnClaim.TabIndex = 3;
            this.btnClaim.Text = "I\'m On It";
            this.btnClaim.UseVisualStyleBackColor = true;
            this.btnClaim.Click += new System.EventHandler(this.btnClaim_Click);
            // 
            // TestResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 409);
            this.Controls.Add(this.splitContainer1);
            this.Name = "TestResultForm";
            this.Text = "Failed Test Results";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ListBox FailedTestList;
        private System.Windows.Forms.TextBox outputText;
        private System.Windows.Forms.Label failedByLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnClaim;
        private System.Windows.Forms.CheckBox filterCheck;
        private System.Windows.Forms.Label recordCount;
    }
}