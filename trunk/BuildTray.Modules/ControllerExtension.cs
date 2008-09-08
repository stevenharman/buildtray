using System.Linq;
using BuildTray.Logic;
using BuildTray.Logic.Entities;

namespace BuildTray.Modules
{
    public static class ControllerExtension
    {
        public static string GetResponsiblePerson(this ITrayController controller)
        {
            string result = null;
            Build currentBuild = null;
            foreach (var previous in controller.CompletedBuilds.OrderByDescending(bd => bd.BuildNumber))
            {
                if (currentBuild != null)
                    if (previous.Status == BuildStatuses.Passed && currentBuild.Status == BuildStatuses.Failed)
                    {
                        result = currentBuild.RequestedFor;
                        break;
                    }
                currentBuild = previous;
            }

            return result;
        }
    }
}
