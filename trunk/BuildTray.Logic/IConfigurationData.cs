using System.Collections.Generic;
using BuildTray.Logic.Entities;

namespace BuildTray.Logic
{
    public interface IConfigurationData
    {
        List<string> ActiveModules { get; set; }
        List<BuildInfo> BuildConfigurations { get; set; }
        int PollingInterval { get; set; }
        string ApplicationDataPath { get; set; }

        void Save();
    }
}