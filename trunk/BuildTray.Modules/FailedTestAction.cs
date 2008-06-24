using System;
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
                var failedTests = new StringBuilder();
                failedTests.AppendLine("Failed by " + GetResponsiblePerson(controller));
                controller.ResponsibleForFailure = GetResponsiblePerson(controller);
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
                    var info = new FileInfo(build.LogLocation);
                    var files = Directory.GetFiles(Path.Combine(info.DirectoryName ?? string.Empty, "TestResults"), "*.trx");

                    if (files.Any())
                    {
                        string testResultsFile = files[0];
                        StreamReader fileReader = new StreamReader(testResultsFile);
                        var document = new XmlDocument();
                        document.LoadXml(fileReader.ReadToEnd());
                        fileReader.Close();
                        XmlNodeList nodes = document.GetElementsByTagName("UnitTestResult");

                        var foundStuff = nodes.OfType<XmlNode>().Where(nd => nd.Attributes.GetNamedItem("outcome").InnerText == "Failed");

                        var values = foundStuff.Select(s => new FailedTest
                        {
                            Output = GetOutput(s.ChildNodes.OfType<XmlNode>().FirstOrDefault(nd => nd.Name == "Output")),
                            TestName = s.Attributes.GetNamedItem("testName").InnerText,
                            ClassName = GetClassName(document, s.Attributes.GetNamedItem("testId").InnerText)
                        });

                        controller.FailedTests = values;
                    }
                }
                else
                    controller.FailedTests = null;
                

                controller.NotifyIcon.BalloonTipText = failedTests.ToString();
                controller.NotifyIcon.ShowBalloonTip(20);
            }
            else
            {
                controller.NotifyIcon.BalloonTipText = string.Empty;
                controller.FailedTests = null;
            }


        }

        private string GetOutput(XmlNode node)
        {
            var debugTraceNode = node.ChildNodes.OfType<XmlNode>().FirstOrDefault(nd => nd.Name == "StdOut");
            var errorInfoNode = node.ChildNodes.OfType<XmlNode>().FirstOrDefault(nd => nd.Name == "ErrorInfo");

            StringBuilder builder = new StringBuilder();
            if (debugTraceNode != null)
            {
                builder.AppendLine("Error Info:");
                builder.AppendLine(debugTraceNode.InnerText);
                builder.AppendLine();
            }
            if (errorInfoNode != null)
            {
                builder.AppendLine("Stack Trace:");
                builder.AppendLine(errorInfoNode.InnerText);
            }

            return builder.ToString();
        }

        private string GetClassName(XmlDocument document, string testId)
        {
            var nodes = document.GetElementsByTagName("UnitTest");
            var node = nodes.OfType<XmlNode>().FirstOrDefault(nd => nd.Attributes.GetNamedItem("id").InnerText == testId);
            var methodNode = node.ChildNodes.OfType<XmlNode>().FirstOrDefault(nd => nd.Name == "TestMethod");
            string className = methodNode.Attributes.GetNamedItem("className").InnerText;
            int length = className.IndexOf(",");
            return className.Substring(0, length);

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
