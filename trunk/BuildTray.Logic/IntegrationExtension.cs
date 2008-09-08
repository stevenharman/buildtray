using System;
using System.Collections.Generic;
using BuildTray.Logic.Entities;
using Microsoft.TeamFoundation.Build.Client;

namespace BuildTray.Logic
{
    public static class IntegrationExtension
    {
        private static IIntegrationService _integrationService;

        public static IIntegrationService IntegrationService
        {
            get
            {
                if (_integrationService == null)
                    _integrationService = new IntegrationService();
                return _integrationService;
            }
            set { _integrationService = value; }
        }

        public static DateTime GetMaxStartDate(this IList<IBuildDetail> details)
        {
            return IntegrationService.GetMaxStartDateForBuildDetailList(details);
        }

        public static bool IsIgnored(this IBuildDetail build)
        {
            return IntegrationService.IsBuildIgnored(build);
        }

        public static TestStatus GetTestStatus(this Build build)
        {
            return IntegrationService.GetBuildTestStatus(build);
        }

        public static IEnumerable<FailedTest> GetFailedTestsFromFile(this Build build)
        {
            return IntegrationService.GetFailedTestsFromFileForBuild(build);
        }
    }
}