using System;
using System.Collections.Generic;
using System.Linq;

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

            IEnumerable<FailedTest> values = this.GetFailedTestsFromFile();

            if (values == null)
            {
                _loadedTests = true;
                return null;
            }

            if (PreviousBuild == null)
            {
                _loadedTests = true;
                FailedTests = values.ToList();
                return FailedTests;
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
    }
}
