using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.TeamFoundation.Build.Client;

namespace BuildTray.Logic.Entities
{
    [Serializable]
    public class Build
    {
        public BuildStatuses Status { get; set; }
        public string RequestedFor { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public int BuildNumber { get; set; }
        public string LogLocation { get; set; }
        public string DropLocation { get; set; }
        public IList<FailedTest> FailedTests { get; set; }
        public Build PreviousBuild { get; set; }
        private bool _loadedTests;

        public IList<FailedTest> GetFailedTests()
        {
            if (Status != BuildStatuses.Failed || LogLocation == null)
                return null;

            if (FailedTests != null || _loadedTests)
                return FailedTests;

            var info = new FileInfo(LogLocation);

            string path = Path.Combine(info.DirectoryName ?? string.Empty, "TestResults");
            if (!Directory.Exists(path))
                return null;

            var files = Directory.GetFiles(path, "*.trx");

            IEnumerable<FailedTest> values = FailedTests;
            if (values == null)
            {
                if (files.Any())
                {
                    string testResultsFile = files[0];
                    StreamReader fileReader = new StreamReader(testResultsFile);
                    var document = new XmlDocument();
                    document.LoadXml(fileReader.ReadToEnd());
                    fileReader.Close();
                    XmlNodeList nodes = document.GetElementsByTagName("UnitTestResult");

                    var foundStuff = nodes.OfType<XmlNode>().Where(nd => nd.Attributes.GetNamedItem("outcome").InnerText == "Failed");

                    values = foundStuff.Select(s => new FailedTest
                    {
                        Output = GetOutput(s.ChildNodes.OfType<XmlNode>().FirstOrDefault(nd => nd.Name == "Output")),
                        TestName = s.Attributes.GetNamedItem("testName").InnerText,
                        ClassName = GetClassName(document, s.Attributes.GetNamedItem("testId").InnerText),
                        FailedBy = RequestedFor
                    });
                }

                var failedTests = PreviousBuild.GetFailedTests() ?? new List<FailedTest>();

                IList<FailedTest> newFailedTests = values.Except(failedTests).ToList();
                IList<FailedTest> fixedTests = failedTests.Except(values).ToList();

                
                var tests = failedTests.Intersect(values).ToList();
                var newTests = values.Intersect(failedTests).ToList();

                tests.Each(t => failedTests.Remove(t));
                newTests.Each(failedTests.Add);

                newTests.Each(t => t.FailedBy = tests.Single(nt => nt.Equals(t)).FailedBy);

                newFailedTests.Each(failedTests.Add);
                fixedTests.Each(ft => failedTests.Remove(ft));

                FailedTests = failedTests;

                _loadedTests = true;

                return failedTests;
            }

            return null;
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
    }
}
