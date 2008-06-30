using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BuildTray.Logic;
using MessengerSend;

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
                string.Format("{0} : {1}.{2}", fail.FailedBy, fail.ClassName, fail.TestName) == FailedTestList.SelectedItem.ToString());

            outputText.Text = failure.Output;
        }

        public IEnumerable<FailedTest> Failures { get; set; }

        public void RefreshList()
        {
            FailedTestList.Items.Clear();

            FailedTestList.Items.AddRange(GetFilteredFailures());

            recordCount.Text = FailedTestList.Items.Count + " Failed";

            outputText.Text = string.Empty;
        }

        private string[] GetFilteredFailures()
        {
            var failures = Failures;

            if (filterCheck.Checked)
                failures = failures.Where(fail => 
                    !(fail.Output.Contains("The timeout period elapsed prior to completion of the operation or the server")
                    || fail.Output.Contains("deadlocked on lock resources with another process and has been chosen as the deadlock victim")));

            return failures.Select(fail => string.Format("{0} : {1}.{2}", fail.FailedBy, fail.ClassName, fail.TestName)).ToArray();
        }

        public string FailedBy
        {
            get { return failedByLabel.Text; }
            set { failedByLabel.Text = value; }
        }

        private void btnClaim_Click(object sender, EventArgs e)
        {
            btnClaim.Enabled = false;
            MessengerSend.MessengerSend messenger = new MessengerSend.MessengerSend(
                new MSNCredentials("Pheonix", "pheonixbuildnotify@gmail.com", "pheonix"), new MSNConfiguration(2000, 2000));

            try
            {
                messenger.SendMessageToAllContacts(string.Format("{0} is fixing the build", FailedBy));
                messenger.Finish();
            }
            catch (Exception ex)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Failed to send IM");
                builder.AppendLine(ex.ToString());
                EventLog.WriteEntry("BuildTray", builder.ToString());
                return;
            }

            MessageBox.Show("Recipients have been notified");

            btnClaim.Enabled = true;
        }

        private void filterCheck_CheckedChanged(object sender, EventArgs e)
        {
            RefreshList();
        }
    }
}
