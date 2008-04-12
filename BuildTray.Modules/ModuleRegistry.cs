using StructureMap.Configuration.DSL;

namespace BuildTray.Modules
{
    public class ModuleRegistry : Registry
    {
        protected override void configure()
        {
            ForRequestedType<IPluginManager>().TheDefaultIsConcreteType<PluginManager>().AsSingletons();
        }
    }
}
