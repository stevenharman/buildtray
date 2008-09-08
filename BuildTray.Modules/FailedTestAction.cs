using System;
using System.Linq;
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

        private static void ShowFailedTests(Build build, ITrayController controller)
        {
            if (build.Status == BuildStatuses.Failed && !string.IsNullOrEmpty(build.LogLocation))
            {
                controller.FailedTests = build.GetTestStatus() != TestStatus.Success ? build.GetFailedTests() : null;

                string failedBy = controller.FailedTests != null 
                                      ? string.Join(", ", controller.FailedTests.Select(ft => ft.FailedBy).Distinct().ToArray()) 
                                      : controller.GetResponsiblePerson();

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
    }
}
