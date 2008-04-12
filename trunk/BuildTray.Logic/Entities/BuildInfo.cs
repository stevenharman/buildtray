using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildTray.Logic.Entities
{
    public class BuildInfo
    {
        public string BuildName { get; set; }
        public string ProjectName { get; set; }
        public Uri ServerUrl { get; set; }
    }
}
