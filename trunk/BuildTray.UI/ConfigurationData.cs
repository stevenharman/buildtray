using System;
using System.Collections.Generic;
using BuildTray.Logic;
using BuildTray.Logic.Entities;

namespace BuildTray.UI
{
    [Serializable]
    public class ConfigurationData : IConfigurationData
    {
        public ConfigurationData()
        {
            BuildConfigurations = new List<BuildInfo>();
            PollingInterval = 30;
        }

        public List<BuildInfo> BuildConfigurations { get; set; }
        public int PollingInterval { get; set; }
        public string LogDirectory { get; set; }
    }
}
