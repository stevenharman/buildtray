using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using BuildTray.Logic.Entities;
using BuildTray.Logic.XmlLogic;
using Microsoft.TeamFoundation.Build.Client;

namespace BuildTray.Logic
{
    public class IntegrationService : IIntegrationService
    {
        public DateTime GetMaxStartDateForBuildDetailList(IList<IBuildDetail> details)
        {
            return details.Where(build => build.IsIgnored()
                                        || build.Status == BuildStatus.PartiallySucceeded
                                        || build.Status == BuildStatus.Failed
                                        || build.Status == BuildStatus.Succeeded)
                           .Max<IBuildDetail, DateTime?>(build => build.StartTime) ?? DateTime.MinValue;
        }

        public bool IsBuildIgnored(IBuildDetail build)
        {
            if (build.Status == BuildStatus.Failed && build.LogLocation != null)
            {
                var reader = new StreamReader(build.LogLocation);
                var needsIgnored = reader.ReadToEnd().Contains("Done executing task \"RemoveDir\" -- FAILED.");
                reader.Close();
                return needsIgnored;
            }

            return false;
        }

        public TestStatus GetBuildTestStatus(Build build)
        {
            var reader = new StreamReader(build.LogLocation);
            string log = reader.ReadToEnd();
            reader.Close();

            if (log.Contains("Test Run Failed."))
                return TestStatus.Failed;

            if (log.Contains("Test Run Error."))
                return TestStatus.Error;

            return TestStatus.Success;
        }

        public IEnumerable<FailedTest> GetFailedTestsFromFileForBuild(Build build)
        {
            var info = new FileInfo(build.LogLocation);

            string path = Path.Combine(info.DirectoryName ?? string.Empty, "TestResults");

            if (!Directory.Exists(path))
                return null;

            var files = Directory.GetFiles(path, "*.trx");

            if (files.Any())
            {
                string testResultsFile = files[0];
                var fileReader = new StreamReader(testResultsFile);
                var document = new XmlDocument();
                document.LoadXml(fileReader.ReadToEnd());
                fileReader.Close();
                XmlNodeList nodes = document.GetElementsByTagName("UnitTestResult");

                var foundStuff = nodes.OfType<XmlNode>().Where(nd => nd.Attributes.GetNamedItem("outcome").InnerText == "Failed");

                return foundStuff.Select(s => new FailedTest
                {
                    Output = s.ChildNodes.OfType<XmlNode>().FirstOrDefault(nd => nd.Name == "Output").GetTestOutput(),
                    TestName = s.Attributes.GetNamedItem("testName").InnerText,
                    ClassName = document.GetClassNameForTest(s.Attributes.GetNamedItem("testId").InnerText),
                    FailedBy = build.RequestedFor
                });
            }

            return null;
        }
    }
}
