using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BuildTray.Logic;

namespace BuildTray.UI
{
    public partial class TestResultForm : Form
    {
        public TestResultForm()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FailedTestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FailedTestList.SelectedItem == null)
            {
                outputText.Text = string.Empty;
                return;
            }
            
            var failure = Failures.FirstOrDefault(fail => 
                string.Format("{0}.{1}", fail.ClassName, fail.TestName) == FailedTestList.SelectedItem.ToString());

            outputText.Text = failure.Output;
        }

        public IEnumerable<FailedTest> Failures { get; set; }

        public void RefreshList()
        {
            FailedTestList.Items.Clear();
            FailedTestList.Items.AddRange(Failures.Select(fail => string.Format("{0}.{1}", fail.ClassName, fail.TestName)).ToArray());
            outputText.Text = string.Empty;
        }

        public string FailedBy
        {
            get { return failedByLabel.Text; }
            set { failedByLabel.Text = value; }
        }

    }
}
