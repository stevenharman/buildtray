using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BuildTray.Logic.Entities;
using BuildTray.Logic;

namespace BuildTray.Modules
{
    public class FailedTestAction : IActionModuleDefinition
    {
        public Action<Build, ITrayController> GetAction()
        {
            return ShowFailedTests;
        }

        public string Description
        {
            get { return "Shows the balloon tip with all failed tests for the last build."; }
        }

        public string Name
        {
            get { return "FailedTestAction"; }
        }

        private void ShowFailedTests(Build build, ITrayController controller)
        {
            if (build.Status == BuildStatuses.Failed && !string.IsNullOrEmpty(build.LogLocation))
            {
                StringBuilder failedTests = new StringBuilder();
                failedTests.AppendLine("Failed by " + build.RequestedFor);
                StreamReader reader = new StreamReader(build.LogLocation);
                string log = reader.ReadToEnd();
                if (log.Contains("Test Run Failed."))
                {
                    int start = log.IndexAfter("Starting execution...");
                    int length = log.IndexOf("Test Run Failed.") - start;
                    StringReader logReader = new StringReader(log.Substring(start, length));
                    while (logReader.Peek() != -1)
                    {
                        string currentLine = logReader.ReadLine();
                        if (currentLine.Contains(" Failed "))
                            failedTests.AppendLine(" - " + currentLine.Replace(" Failed ", "").Trim() + " failed.");
                    }
                    logReader.Close();
                }
                reader.Close();

                controller.NotifyIcon.BalloonTipText = failedTests.ToString();
                controller.NotifyIcon.ShowBalloonTip(20);
            }


        }
    }
}
