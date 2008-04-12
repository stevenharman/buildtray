using System;
using System.IO;
using System.Xml.Serialization;
using StructureMap.Configuration.DSL;
using BuildTray.Modules;
using BuildTray.Logic;
using System.Deployment.Application;

namespace BuildTray.UI
{
    public class UiRegistry : Registry
    {
        protected override void configure()
        {
            //Need to do custom logic for creating this so it can be loaded.
            ForRequestedType<IConfigurationData>().TheDefaultIsConcreteType<ConfigurationData>().AsSingletons();

            BuildInstancesOf<IConfigurationData>().TheDefaultIs(ConstructedBy(() => LoadConfigurationData()));


            ForRequestedType<INotifyIcon>().TheDefaultIsConcreteType<NotifyIconProxy>().AsSingletons();
            ForRequestedType<ITrayController>().TheDefaultIsConcreteType<TrayController>().AsSingletons();
        }

        private IConfigurationData LoadConfigurationData()
        {

            //Retrieve the data path for the Click Once deployment, or use the current path.
            string dataPath = ApplicationDeployment.IsNetworkDeployed 
                ? ApplicationDeployment.CurrentDeployment.DataDirectory 
                : string.Empty;

            string fullPath = Path.Combine(dataPath, "BuildTrayConfig.Xml");

            //If there is a config file we will load it.

            if (File.Exists(fullPath))
            {
                var fileStream = new FileStream(fullPath, FileMode.Open);
                try
                {
                    var serializer = new XmlSerializer(typeof(ConfigurationData));
                    var result = serializer.Deserialize(fileStream) as ConfigurationData;
                    return result;
                }
                catch (Exception)
                {
                    //If we receive an exception we will simply return a blank configuration.
                    return new ConfigurationData();
                }
                finally
                {
                    fileStream.Close();
                }
            }


            //Otherwise we return a new configuration object.
            return new ConfigurationData();
        }
    }
}
