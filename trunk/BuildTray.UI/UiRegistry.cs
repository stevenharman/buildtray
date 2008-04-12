using StructureMap.Configuration.DSL;
using BuildTray.Modules;
using BuildTray.Logic;

namespace BuildTray.UI
{
    public class UiRegistry : Registry
    {
        protected override void configure()
        {
            //Need to do custom logic for creating this so it can be loaded.
            ForRequestedType<IConfigurationData>().TheDefaultIsConcreteType<ConfigurationData>().AsSingletons();


            ForRequestedType<INotifyIcon>().TheDefaultIsConcreteType<NotifyIconProxy>().AsSingletons();
            ForRequestedType<IBuildProcessTimer>().TheDefaultIsConcreteType<BuildProcessTimer>();
            ForRequestedType<ITrayController>().TheDefaultIsConcreteType<TrayController>().AsSingletons();
        }
    }
}
