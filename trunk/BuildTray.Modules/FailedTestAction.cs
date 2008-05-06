using System;
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

        public Type ConfigurationForm
        {
            get { return null; }
        }

        private enum TestStatus
        {
            Success,
            Failed,
            Error
        }

        private void ShowFailedTests(Build build, ITrayController controller)
        {
            if (build.Status == BuildStatuses.Failed && !string.IsNullOrEmpty(build.LogLocation))
            {
                var failedTests = new StringBuilder();
                failedTests.AppendLine("Failed by " + GetResponsiblePerson(controller));
                var reader = new StreamReader(build.LogLocation);
                string log = reader.ReadToEnd();
                TestStatus testStatus = TestStatus.Success;
                if (log.Contains("Test Run Failed."))
                    testStatus = TestStatus.Failed;
                else if (log.Contains("Test Run Error."))
                    testStatus = TestStatus.Error;

                if (testStatus != TestStatus.Success)
                {
                    int start = log.IndexAfter("Starting execution...");
                    int length = (testStatus == TestStatus.Failed ? log.IndexOf("Test Run Failed.") : log.IndexOf("Test Run Error.")) - start;
                    var logReader = new StringReader(log.Substring(start, length));
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
            else
            {
                controller.NotifyIcon.BalloonTipText = string.Empty;
            }


        }

        private string GetResponsiblePerson(ITrayController controller)
        {
            string result = null;
            Build currentBuild = null;
            foreach (var previous in controller.CompletedBuilds.OrderByDescending(bd => bd.BuildNumber))
            {
                if (currentBuild != null)
                    if (previous.Status == BuildStatuses.Passed && currentBuild.Status == BuildStatuses.Failed)
                    {
                        result = currentBuild.RequestedFor;
                        break;
                    }
                currentBuild = previous;
            }

            return result;
        }
    }
}
