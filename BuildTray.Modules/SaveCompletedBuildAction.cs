using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BuildTray.Logic;
using BuildTray.Logic.Entities;
using System.Xml.Serialization;

namespace BuildTray.Modules
{
    public class SaveCompletedBuildAction : IActionModuleDefinition
    {
        private readonly IConfigurationData _configuration;
        public SaveCompletedBuildAction(IConfigurationData configuration)
        {
            _configuration = configuration;
        }

        public Action<Build, ITrayController> GetAction()
        {
            return LogBuild;
        }

        public string Description
        {
            get { return "Ensures that all completed builds are logged to a local directory."; }
        }

        public string Name
        {
            get { return this.GetType().Name; }
        }

        private void LogBuild(Build build, ITrayController controller)
        {
            var serializer = new XmlSerializer(typeof(Build));
            var stream = new FileStream(
                Path.Combine(_configuration.LogDirectory, build.BuildNumber + ".Xml"), 
                FileMode.Create, 
                FileAccess.Write);

            serializer.Serialize(stream, build);

            stream.Close();
            
        }
    }
}
