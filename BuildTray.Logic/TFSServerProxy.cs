using System;
using System.Collections.Generic;
using BuildTray.Logic.Entities;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Server;

namespace BuildTray.Logic
{
    public interface ITFSServerProxy
    {
        IList<IBuildDetail> GetBuildDetails(Uri serverUrl, string project, string buildName);
        IList<BuildConfiguration> GetBuildConfigurations(Uri serverUrl);
    }

    public class TFSServerProxy : ITFSServerProxy
    {

        public IList<IBuildDetail> GetBuildDetails(Uri serverUrl, string project, string buildName)
        {
            TeamFoundationServer tfs = TeamFoundationServerFactory.GetServer(serverUrl.ToString());

            IBuildServer buildServer = (IBuildServer)tfs.GetService(typeof(IBuildServer));

            IBuildDefinition definition = buildServer.GetBuildDefinition(project, buildName);

            return definition.QueryBuilds();
        }

        public IList<BuildConfiguration> GetBuildConfigurations(Uri serverUrl)
        {

            List<BuildConfiguration> list = new List<BuildConfiguration>();
            try
            {
                TeamFoundationServer server = TeamFoundationServerFactory.GetServer(serverUrl.ToString());
                ProjectInfo[] infoArray = ((ICommonStructureService)server.GetService(typeof(ICommonStructureService))).ListProjects();
                if (infoArray.Length <= 0)
                {
                    return list;
                }
                IBuildServer service = (IBuildServer)server.GetService(typeof(IBuildServer));
                Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
                IBuildDefinitionSpec[] specArray = new IBuildDefinitionSpec[infoArray.Length];
                for (int i = 0; i < infoArray.Length; i++)
                {
                    ProjectInfo info = infoArray[i];
                    dictionary.Add(info.Name, new List<string>());
                    specArray[i] = service.CreateBuildDefinitionSpec(info.Name);
                }
                foreach (IBuildDefinitionQueryResult result in service.QueryBuildDefinitions(specArray))
                {
                    if (result.Failures.Length == 0)
                    {
                        foreach (IBuildDefinition definition in result.Definitions)
                        {
                            dictionary[definition.TeamProject].Add(definition.Name);
                        }
             
                    }
                }
                foreach (KeyValuePair<string, List<string>> pair in dictionary)
                {
                    if (pair.Value.Count > 0)
                    {
                        foreach(var name in pair.Value)
                            list.Add(new BuildConfiguration { ProjectName = pair.Key, BuildName = name, ServerUrl = serverUrl});
                    }
                }
                dictionary.Clear();
            }
            catch (Exception)
            {
                throw new ApplicationException("Error connecting to the server.");
            }
            return list;
        }
    }
}
