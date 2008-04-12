using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    }
}
