using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using BuildTray.Logic;
using BuildTray.Logic.Entities;
using BuildTray.UI.Properties;
using Microsoft.TeamFoundation.Build.Client;
using System.Windows.Forms;
using Timer = System.Threading.Timer;
using System.IO;

namespace BuildTray.UI
{
    public class BuildProcessTimer : IBuildProcessTimer
    {
        private readonly ITFSServerProxy _proxy;
        private readonly IList<BuildInfo> _builds;
        private Timer _internalTimer;
        private int _lastBuild;
        private int _currentBuildNumber;
        private bool _isRunning;
        private readonly IConfigurationData _configurationData;

        public int PollingInterval { get; set; }

        public BuildProcessTimer(ITFSServerProxy proxy, IConfigurationData configurationData)
        {
            _configurationData = configurationData;
            _proxy = proxy;
            _builds = new List<BuildInfo>();
            PollingInterval = _configurationData.PollingInterval;
        }

        private void TimerCallback(object data)
        {
            _isRunning = true;
            _builds.Each(bc =>
            {
                IList<IBuildDetail> details = _proxy.GetBuildDetails(bc.ServerUrl, bc.ProjectName, bc.BuildName);

                DateTime? maxStartDate = details.Where(build => build.Status.CanConvert()
                                                           && (build.GetBuildNumber() > _lastBuild)
                                                           && (build.Status == BuildStatus.PartiallySucceeded
                                                               || build.Status == BuildStatus.InProgress
                                                               || build.Status == BuildStatus.Failed 
                                                               || build.Status == BuildStatus.Succeeded)
                                                           ).Max<IBuildDetail, DateTime?>(build => build.StartTime);

                details.Where(build => build.Status.CanConvert()
                            && (build.GetBuildNumber() > _lastBuild))
                    .OrderBy(build => build.StartTime).Each(build =>
                {
                    int buildNumber = build.GetBuildNumber();
                    if (build.Status == BuildStatus.Failed && build.LogLocation != null)
                    {
                        StreamReader reader = new StreamReader(build.LogLocation);
                        if (reader.ReadToEnd().Contains("Done executing task \"RemoveDir\" -- FAILED."))
                        {
                            _isRunning = false;
                            BuildIgnored.Raise(this, new BuildDetailEventArgs { Build = build, MostRecentStartDate = maxStartDate ?? DateTime.MinValue });
                            return;
                        }
                    }
                    switch (build.Status)
                    {
                        case BuildStatus.Failed:
                        case BuildStatus.PartiallySucceeded:
                        case BuildStatus.Succeeded:
                            _lastBuild = buildNumber;
                            BuildCompleted.Raise(this, new BuildDetailEventArgs { Build = build, MostRecentStartDate = maxStartDate ?? DateTime.MinValue });
                            break;
                        case BuildStatus.InProgress:
                            _currentBuildNumber = buildNumber;
                            BuildStarted.Raise(this, new BuildDetailEventArgs { Build = build, MostRecentStartDate = maxStartDate ?? DateTime.MinValue });
                            break;
                    }
                });
            });
            _isRunning = false;
        }

        public void Start()
        {
            _internalTimer = new Timer(TimerCallback, null, new TimeSpan(0, 0, 0, 1), new TimeSpan(0, 0, 0, PollingInterval));
        }

        public void Stop()
        {
            while (_isRunning) { } //Wait until the last callback stops then dispose.
            _internalTimer.Dispose();
        }

        public void AddBuild(BuildInfo info)
        {
            _builds.Add(info);
        }

        public void RemoveBuild(BuildInfo remove)
        {
            _builds.Remove(remove);
        }

        public event EventHandler<BuildDetailEventArgs> BuildStarted;
        public event EventHandler<BuildDetailEventArgs> BuildCompleted;
        public event EventHandler<BuildDetailEventArgs> BuildIgnored;
    }
    /*
        private void RetrieveFixedBuildResponsiblePerson(StringBuilder toolTip)
        {
            Configuration.BuildConfigurations.Each(bc =>
                                                       {
                                                           var build = bc.BuildDetails.Where(bd => bd.Status.CanConvert()).OrderByDescending(bd => bd.GetBuildNumber()).FirstOrDefault();
                                                           var trayStatus = build.Status.ToTrayStatus(bc.Status);
                                                           if (trayStatus == TrayStatus.Success || trayStatus == TrayStatus.SuccessInProgress)
                                                           {
                                                               var previousBuilds =
                                                                   bc.BuildDetails.Where(bd => bd.Status.CanConvert()).OrderByDescending(bd => bd.GetBuildNumber())
                                                                       .Skip(1);
                                                               foreach(var previousBuild in previousBuilds)
                                                               {
                                                                   var previousStatus = previousBuild.Status.ToTrayStatus(bc.Status);
                                                                   if (previousStatus == TrayStatus.Failure || previousStatus == TrayStatus.FailureInProgress)
                                                                   {
                                                                       toolTip.AppendLine("Build " + bc.BuildName + " fixed by " + build.RequestedFor);
                                                                       break;
                                                                   }
                                                                   build = previousBuild;
                                                               }

                                                           }
                                                       });
        }
*/
}
