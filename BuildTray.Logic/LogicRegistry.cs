using StructureMap.Configuration.DSL;

namespace BuildTray.Logic
{
    public class LogicRegistry : Registry
    {
        protected override void configure()
        {
            ForRequestedType<ITFSServerProxy>().TheDefaultIsConcreteType<TFSServerProxy>();
            ForRequestedType<IBuildProcessTimer>().TheDefaultIsConcreteType<BuildProcessTimer>();
        }
    }
}
