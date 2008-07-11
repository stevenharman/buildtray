using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
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
                var reader = new StreamReader(build.LogLocation);
                string log = reader.ReadToEnd();
                reader.Close();
                TestStatus testStatus = TestStatus.Success;
                if (log.Contains("Test Run Failed."))
                    testStatus = TestStatus.Failed;
                else if (log.Contains("Test Run Error."))
                    testStatus = TestStatus.Error;

                if (testStatus != TestStatus.Success)
                {
                    controller.FailedTests = build.GetFailedTests();
                }
                else
                    controller.FailedTests = null;

                string failedBy;
                if (controller.FailedTests != null)
                    failedBy = string.Join(", ", controller.FailedTests.Select(ft => ft.FailedBy).Distinct().ToArray());
                else
                    failedBy = GetResponsiblePerson(controller);

                controller.NotifyIcon.BalloonTipText = "Failed by " + failedBy;
                controller.NotifyIcon.ShowBalloonTip(20);
                controller.ResponsibleForFailure = failedBy;
            }
            else
            {
                controller.NotifyIcon.BalloonTipText = string.Empty;
                controller.FailedTests = null;
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
