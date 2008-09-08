using System;
using System.Collections.Generic;
using System.Linq;
using BuildTray.Logic.Entities;
using Microsoft.TeamFoundation.Build.Client;
using Timer = System.Threading.Timer;

namespace BuildTray.Logic
{
    public class BuildProcessTimer : IBuildProcessTimer
    {
        private readonly ITFSServerProxy _proxy;
        private readonly IList<BuildInfo> _builds;
        private Timer _internalTimer;
        private int _lastBuild;
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
            try
            {
                _isRunning = true;
                _builds.Each(bc =>
                                 {
                                     IList<IBuildDetail> details = _proxy.GetBuildDetails(bc.ServerUrl, bc.ProjectName, bc.BuildName)
                                         .Where(build => build.Status.CanConvert() && (build.GetBuildNumber() > _lastBuild)).ToList();

                                     DateTime maxStartDate = details.GetMaxStartDate();

                                     details.OrderBy(build => build.StartTime).Each(build =>
                                                                                     {
                                                                                         if (build.IsIgnored())
                                                                                         {
                                                                                             BuildIgnored.Raise(this, new BuildDetailEventArgs { Build = build, MostRecentStartDate = maxStartDate });
                                                                                         }
                                                                                         else
                                                                                         {
                                                                                             int buildNumber = build.GetBuildNumber();

                                                                                             switch (build.Status)
                                                                                             {
                                                                                                 case BuildStatus.Failed:
                                                                                                 case BuildStatus.PartiallySucceeded:
                                                                                                 case BuildStatus.Succeeded:
                                                                                                     _lastBuild = buildNumber;
                                                                                                     BuildCompleted.Raise(this, new BuildDetailEventArgs { Build = build, MostRecentStartDate = maxStartDate });
                                                                                                     break;
                                                                                                 case BuildStatus.InProgress:
                                                                                                     BuildStarted.Raise(this, new BuildDetailEventArgs { Build = build, MostRecentStartDate = maxStartDate });
                                                                                                     break;
                                                                                             }
                                                                                         }
                                                                                     });
                                 });
                _isRunning = false;
            }
            catch (Exception ex)
            {
                ThreadException.Raise(this, new ExceptionEventArgs { Exception = ex });
            }
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
        public event EventHandler<ExceptionEventArgs> ThreadException;
    }
}