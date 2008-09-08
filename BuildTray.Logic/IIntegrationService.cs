using System;
using System.Collections.Generic;
using BuildTray.Logic.Entities;
using Microsoft.TeamFoundation.Build.Client;

namespace BuildTray.Logic
{
    public interface IIntegrationService
    {
        DateTime GetMaxStartDateForBuildDetailList(IList<IBuildDetail> details);
        bool IsBuildIgnored(IBuildDetail build);
        TestStatus GetBuildTestStatus(Build build);
        IEnumerable<FailedTest> GetFailedTestsFromFileForBuild(Build build);
    }
}