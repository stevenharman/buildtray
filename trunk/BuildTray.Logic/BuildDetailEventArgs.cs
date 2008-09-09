using System;
using Microsoft.TeamFoundation.Build.Client;
using BuildTray.Logic.Entities;

namespace BuildTray.Logic
{
    public class BuildDetailEventArgs : EventArgs
    {
        public IBuildDetail Build { get; set; }
        public DateTime MostRecentStartDate { get; set; }
        public BuildInfo BuildInfo { get; set; }
    }
}