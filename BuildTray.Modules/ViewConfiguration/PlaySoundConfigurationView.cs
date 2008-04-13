using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using BuildTray.Logic;

namespace BuildTray.Modules.ViewConfiguration
{
    public partial class PlaySoundConfigurationView : Form
    {
        private readonly IConfigurationData _configurationData;
        private string _modulePath;
        public PlaySoundConfigurationView(IConfigurationData configurationData)
        {
            InitializeComponent();

            _configurationData = configurationData;
        }


        private void PlaySoundConfigurationView_Load(object sender, EventArgs e)
        {
            _modulePath = Path.Combine(_configurationData.ApplicationDataPath, "PlaySoundModule");
            playFailureButton.Enabled = File.Exists(Path.Combine(_modulePath, "FailedBuild.Wav"));
            playMultipleFailureButton.Enabled = File.Exists(Path.Combine(_modulePath, "FailedBuildAgain.Wav"));
            playSuccessButton.Enabled = File.Exists(Path.Combine(_modulePath, "PassedBuild.Wav"));
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(_modulePath))
                Directory.CreateDirectory(_modulePath);

            File.Copy(successFileText.Text, Path.Combine(_modulePath, "PassedBuild.Wav"));
            File.Copy(failureFileText.Text, Path.Combine(_modulePath, "FailedBuild.Wav"));
            File.Copy(multipleFailureFileText.Text, Path.Combine(_modulePath, "FailedBuildAgain.Wav"));
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ChooseSoundFile(TextBox box)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "Sound Files | *.Wav";
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
                box.Text = dialog.FileName;
        }

        private void browseSuccessButton_Click(object sender, EventArgs e)
        {
            ChooseSoundFile(successFileText);
        }

        private void browseFailureButton_Click(object sender, EventArgs e)
        {
            ChooseSoundFile(failureFileText);
        }

        private void browseMultipleFailuresButton_Click(object sender, EventArgs e)
        {
            ChooseSoundFile(multipleFailureFileText);
        }

        private void playMultipleFailureButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(multipleFailureFileText.Text) && File.Exists(multipleFailureFileText.Text))
                PlaySound(multipleFailureFileText.Text);
            else if (File.Exists(Path.Combine(_modulePath, "FailedBuildAgain.Wav")))
                PlaySound(Path.Combine(_modulePath, "FailedBuildAgain.Wav"));
            else
                MessageBox.Show("You must select a sound to play first.", "No sound selected");
        }

        private void playFailureButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(failureFileText.Text) && File.Exists(failureFileText.Text))
                PlaySound(failureFileText.Text);
            else if (File.Exists(Path.Combine(_modulePath, "FailedBuild.Wav")))
                PlaySound(Path.Combine(_modulePath, "FailedBuild.Wav"));
            else
                MessageBox.Show("You must select a sound to play first.", "No sound selected");
        }

        private void playSuccessButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(successFileText.Text) && File.Exists(successFileText.Text))
                PlaySound(successFileText.Text);
            else if (File.Exists(Path.Combine(_modulePath, "PassedBuild.Wav")))
                PlaySound(Path.Combine(_modulePath, "PassedBuild.Wav"));
            else
                MessageBox.Show("You must select a sound to play first.", "No sound selected");
        }

        public virtual void PlaySound(string soundFile)
        {
            try
            {
                PlaySound(soundFile, IntPtr.Zero, 0x1);
            }
            catch (Exception)
            {
            }

        }

        [DllImport("winmm.dll")]
        private static extern bool PlaySound(string pszName, IntPtr hModule, int dwFlags);

    }
}
