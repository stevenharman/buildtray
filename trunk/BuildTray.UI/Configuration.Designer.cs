namespace BuildTray.UI
{
    partial class Configuration
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
            this.selectedBuilds = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // selectedBuilds
            // 
            this.selectedBuilds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedBuilds.Location = new System.Drawing.Point(193, 53);
            this.selectedBuilds.Name = "selectedBuilds";
            this.selectedBuilds.Size = new System.Drawing.Size(544, 255);
            this.selectedBuilds.TabIndex = 0;
            this.selectedBuilds.UseCompatibleStateImageBehavior = false;
            // 
            // Configuration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 342);
            this.Controls.Add(this.selectedBuilds);
            this.Name = "Configuration";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView selectedBuilds;
    }
}

