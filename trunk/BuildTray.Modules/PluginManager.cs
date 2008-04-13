using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BuildTray.Logic;
using StructureMap;

namespace BuildTray.Modules
{
    public interface IPluginManager
    {
        string PluginDirectory { get; }
        void Refresh();
    }

    public class PluginManager : IPluginManager
    {
        private static readonly List<IActionModuleDefinition> _modules = new List<IActionModuleDefinition>();

        public string PluginDirectory
        {
            get { return Path.Combine(_config.ApplicationDataPath, "Plugins"); }
        }

        private readonly IConfigurationData _config;

        public PluginManager(IConfigurationData config)
        {
            _config = config;   
        }

        public void Refresh()
        {
            _modules.Clear();
            _modules.AddRange(GetCustomActions(typeof (IActionModuleDefinition).Assembly));

            if (!string.IsNullOrEmpty(PluginDirectory) && Directory.Exists(PluginDirectory))
            {
                string[] files = Directory.GetFiles(PluginDirectory, "*.dll");

                files.Each(mod => _modules.AddRange(GetCustomActions(Assembly.LoadFrom(mod))));
            }
        }

        private IEnumerable<IActionModuleDefinition> GetCustomActions(Assembly assembly)
        {
            var results = new List<IActionModuleDefinition>();
            IEnumerable<Type> actionTypes = assembly.GetTypes()
                .Where(tp => tp.GetInterfaces().FirstOrDefault(n => n.Name == "IActionModuleDefinition") != null);
            
            actionTypes.Each(tp => results.Add((IActionModuleDefinition) ObjectFactory.GetInstance(tp)));
            return results;
        }
    }
}
