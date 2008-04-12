using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildTray.Logic;
using BuildTray.Logic.Entities;

namespace BuildTray.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TFSServerProxy proxy = new TFSServerProxy();
            IList<BuildConfiguration> builds = proxy.GetBuildConfigurations(new Uri("http://vrp-tfs-000:8080"));
            foreach(var build in builds)
                Console.WriteLine(build.ProjectName + " - " + build.BuildName);
            Console.ReadKey();

            /*TeamFoundationServer tfs = new TeamFoundationServer(TfsUrl);
        
        IBuildServer buildServer = (IBuildServer)tfs.GetService(typeof(IBuildServer));
        
        buildQueue = buildServer.CreateQueuedBuildsView(TeamProject);
        
        IBuildDefinition definition = buildServer.GetBuildDefinition("Phoenix", "Phoenix_UnitTests");
        IBuildDetail[] details = definition.QueryBuilds();*/
        }
    }
}
