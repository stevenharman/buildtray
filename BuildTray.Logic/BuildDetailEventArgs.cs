using System;
using Microsoft.TeamFoundation.Build.Client;

namespace BuildTray.Logic
{
    public class BuildDetailEventArgs : EventArgs
    {
        public IBuildDetail Build { get; set; }
        public DateTime MostRecentStartDate { get; set; }
    }
}