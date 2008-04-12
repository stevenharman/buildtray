using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildTray.Logic;
using BuildTray.Modules;
using StructureMap;

namespace BuildTray.UI
{
    public class BuildTrayContainerConfig
    {
        public static void BootstrapStructureMapConfiguration()
        {
            StructureMapConfiguration.ResetAll();
            StructureMapConfiguration.UseDefaultStructureMapConfigFile = false;
            StructureMapConfiguration.ScanAssemblies()
                .IncludeAssemblyContainingType<LogicRegistry>()
                .IncludeAssemblyContainingType<ModuleRegistry>()
                .IncludeAssemblyContainingType<UiRegistry>()
                .IncludeTheCallingAssembly();

            ObjectFactory.Reset();
        }
    }
}
