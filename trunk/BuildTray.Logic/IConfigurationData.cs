using System.Collections.Generic;
using BuildTray.Logic.Entities;

namespace BuildTray.Logic
{
    public interface IConfigurationData
    {
        List<BuildInfo> BuildConfigurations { get; set; }
        int PollingInterval { get; set; }
        string LogDirectory { get; set; }
        string PluginDirectory { get; set; }

        void Save();
    }
}