using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Build.Client;

namespace BuildTray.Logic.Entities
{
    public class BuildConfiguration
    {
        public BuildConfiguration()
        {
            LogLocations = new Dictionary<int, string>();
            BuildDetails = new List<IBuildDetail>();
        }

        public string BuildName { get; set; }
        public string ProjectName { get; set; }
        public Uri ServerUrl { get; set; }

        public int LastBuild { get; set; }
        public DateTime LastBuildTime { get; set; }
        public TrayStatus Status { get; set; }
        public Dictionary<int, string> LogLocations { get; set; }
        public List<IBuildDetail> BuildDetails { get; set; }

        public string LastFailureBy { get; set; }
    }
}
