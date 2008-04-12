using System;
using Microsoft.TeamFoundation.Build.Client;

namespace BuildTray.UI
{
    public class BuildDetailEventArgs : EventArgs
    {
        public IBuildDetail Build { get; set; }
    }

}
