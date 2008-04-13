using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.IO;
using System.Xml.Serialization;
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
            ApplicationDataPath = ApplicationDeployment.IsNetworkDeployed
                                  ? ApplicationDeployment.CurrentDeployment.DataDirectory
                                  : string.Empty;
        }

        public List<BuildInfo> BuildConfigurations { get; set; }
        public int PollingInterval { get; set; }
        public string ApplicationDataPath { get; set; }
        public List<string> ActiveModules { get; set; }

        public void Save()
        {
            //Retrieve the data path for the Click Once deployment, or use the current path.
            //Even if the data path for the application has been changed this will still load from the default location.
            string dataPath = ApplicationDeployment.IsNetworkDeployed
                                  ? ApplicationDeployment.CurrentDeployment.DataDirectory
                                  : string.Empty;

            string fullPath = Path.Combine(dataPath, "BuildTrayConfig.Xml");

            //Save the config file.
            var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write);
            var serializer = new XmlSerializer(typeof (ConfigurationData));
            serializer.Serialize(fileStream, this);
            fileStream.Close();
        }
    }
}
