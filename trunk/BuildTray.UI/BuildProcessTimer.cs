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
using Timer=System.Threading.Timer;
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

        public int PollingInterval { get; set; }

        public BuildProcessTimer(ITFSServerProxy proxy)
        {
            _proxy = proxy;
            _builds = new List<BuildInfo>();
            PollingInterval = 30;
        }

        private void TimerCallback(object data)
        {
            _isRunning = true;
             _builds.Each(bc =>
             {
                 IList<IBuildDetail> details = _proxy.GetBuildDetails(bc.ServerUrl, bc.ProjectName, bc.BuildName);
                 int maxBuild = details.Where(build => build.Status.CanConvert()
                                                            && (build.GetBuildNumber() > _lastBuild)
                                                            && (build.Status == BuildStatus.PartiallySucceeded
                                                                || build.Status == BuildStatus.InProgress
                                                                || build.Status == BuildStatus.Failed)
                                                            ).Max(build => build.GetBuildNumber());

                 details.Where(build => build.Status.CanConvert()
                             && (build.GetBuildNumber() > _lastBuild))
                     .OrderBy(build => build.GetBuildNumber()).Each(build =>
                 {
                     int buildNumber = build.GetBuildNumber();
                     bool useBuild = true;
                     if (build.Status == BuildStatus.Failed && build.LogLocation != null)
                     {
                         StreamReader reader = new StreamReader(build.LogLocation);
                         if (reader.ReadToEnd().Contains("Done executing task \"RemoveDir\" -- FAILED."))
                             useBuild = false;
                     }
                     if (useBuild)
                     {
                         switch( build.Status)
                         {
                             case BuildStatus.Failed:
                             case BuildStatus.PartiallySucceeded:
                             case BuildStatus.Succeeded:
                                 _lastBuild = buildNumber;
                                 if (maxBuild == buildNumber)
                                    BuildCompleted.Raise(this, new BuildDetailEventArgs { Build = build });
                                 break;
                             case BuildStatus.InProgress:
                                 if (buildNumber > _currentBuildNumber)
                                 {
                                     _currentBuildNumber = buildNumber;
                                     if (maxBuild == buildNumber)
                                        BuildStarted.Raise(this, new BuildDetailEventArgs { Build = build });
                                 }
                                 break;
                         }
                     }
                 });
             });
            _isRunning = false;
        }

        public void Start()
        {
            _internalTimer = new Timer(TimerCallback, null, new TimeSpan(0, 0, 0, 1), new TimeSpan(0, 0, 0, PollingInterval) );
        }

        public void Stop()
        {
            while (_isRunning) {} //Wait until the last callback stops then dispose.
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
    }
    /*
    public class BuildStatusProcess
    {
        [DllImport("winmm.dll")]
        private static extern bool PlaySound(string pszName, IntPtr hModule, int dwFlags);

        private readonly TFSServerProxy _proxy;
        private readonly Timer _internalTimer;
        private readonly NotifyIcon _trayIcon;

        public BuildStatusProcess(ConfigurationData configuration, NotifyIcon trayIcon)
        {
            _proxy = new TFSServerProxy();
            Configuration = configuration;
            _trayIcon = trayIcon;
            _trayIcon.DoubleClick += (sender, e) =>
                                   {
                                       if (!string.IsNullOrEmpty(_trayIcon.BalloonTipText))
                                            MessageBox.Show(_trayIcon.BalloonTipText, "Last Status Update");
                                   }; 
            _internalTimer = new Timer(TimerCallback, null, new TimeSpan(0,0,0,1), new TimeSpan(0, 0, 0, configuration.PollingTimer));
        }

        private void ProcessBuilds()
        {
            Configuration.BuildConfigurations.Each(bc =>
            {
                IList<IBuildDetail> details = _proxy.GetBuildDetails(bc.ServerUrl, bc.ProjectName, bc.BuildName);
                details
                    .Where(build => build.Status.CanConvert()
                            && (build.GetBuildNumber() >= bc.LastBuild
                            || build.Status == BuildStatus.InProgress))
                    .OrderBy(build => build.GetBuildNumber()).Each(build =>
                {
                    int buildNumber = build.GetBuildNumber();
                    bool useBuild = true;
                    if (build.Status == BuildStatus.Failed && build.LogLocation != null)
                    {
                        StreamReader reader = new StreamReader(build.LogLocation);
                        if (reader.ReadToEnd().Contains("Done executing task \"RemoveDir\" -- FAILED."))
                            useBuild = false;
                    }
                    if (build.Status.CanConvert() && useBuild)
                    {
                        if (bc.BuildDetails.FirstOrDefault(bd => bd.BuildNumber == build.BuildNumber) == null)
                            bc.BuildDetails.Add(build);
                        bc.Status = build.Status.ToTrayStatus(bc.Status).Value;
                        bc.LastBuild = buildNumber;
                        bc.LogLocations[bc.LastBuild] = build.LogLocation;


                        if (bc.Status == TrayStatus.FailureInProgress || bc.Status == TrayStatus.SuccessInProgress)
                            bc.LastBuildTime = build.StartTime;
                        else
                            bc.LastBuildTime = build.FinishTime;
                    }
                });
            });
        }

        private TrayStatus EvaluateStatus()
        {
            TrayStatus status = TrayStatus.Success;
            Configuration.BuildConfigurations.Each(bc => status = status.Combine(bc.Status));
            PlayBuildSound(_trayIcon.CurrentStatus(), status);
            if (status == TrayStatus.Success || status == TrayStatus.SuccessInProgress)
                _trayIcon.Success();
            else if (status == TrayStatus.Failure || status == TrayStatus.FailureInProgress)
                _trayIcon.Failure();
            if (status == TrayStatus.FailureInProgress || status == TrayStatus.SuccessInProgress)
                _trayIcon.InProgress();
            return status;
        }

        private void PlayBuildSound(TrayStatus previousStatus, TrayStatus currentStatus)
        {
            try
            {
                if (previousStatus == TrayStatus.FailureInProgress && currentStatus == TrayStatus.Success)
                    PlaySound("PassedBuild.Wav", IntPtr.Zero, 0x1);
                else if (previousStatus == TrayStatus.SuccessInProgress && currentStatus == TrayStatus.Failure)
                    PlaySound("FailedBuild.Wav", IntPtr.Zero, 0x1);
                else if (previousStatus == TrayStatus.FailureInProgress && currentStatus == TrayStatus.Failure)
                    PlaySound("FailedBuildAgain.Wav", IntPtr.Zero, 0x1);
            }
            catch
            {
            }

        }

        private void TimerCallback(object data)
        {
            ProcessBuilds();

            TrayStatus status = EvaluateStatus();

            CalculateBuildTime(status);

            StringBuilder toolTip = new StringBuilder();

            if (status == TrayStatus.Failure || status == TrayStatus.FailureInProgress)
            {
                RetrieveFailureResponsiblePerson();
                RetrieveFailedTests(toolTip);
            }
            else
                RetrieveFixedBuildResponsiblePerson(toolTip);
            
            SetTrayToolTip(toolTip);
        }

        private void SetTrayToolTip(StringBuilder toolTip)
        {
            if (_trayIcon.BalloonTipText != toolTip.ToString())
            {
                if (!string.IsNullOrEmpty(toolTip.ToString()))
                {
                    _trayIcon.BalloonTipText = toolTip.ToString();
                    _trayIcon.ShowBalloonTip(20);
                }
            }
        }

        private void RetrieveFailedTests(StringBuilder toolTip)
        {
            Configuration.BuildConfigurations.Each(bc =>
                                                       {
                                                           if (bc.Status != TrayStatus.Success &&
                                                               bc.Status != TrayStatus.SuccessInProgress)
                                                           {
                                                               toolTip.AppendLine(bc.BuildName + " Failed by " + bc.LastFailureBy + ".");
                                                               string logLocation = bc.LogLocations.Keys.OrderByDescending(k => k).Where(k => !string.IsNullOrEmpty(bc.LogLocations[k])).Select(k => bc.LogLocations[k]).FirstOrDefault();
                                                               StreamReader reader = new StreamReader(logLocation);
                                                               string log = reader.ReadToEnd();
                                                               if (log.Contains("Test Run Failed."))
                                                               {
                                                                   int start = log.IndexAfter("Starting execution...");
                                                                   int length = log.IndexOf("Test Run Failed.") - start;
                                                                   StringReader logReader = new StringReader(log.Substring(start, length));
                                                                   while (logReader.Peek() != -1)
                                                                   {
                                                                       string currentLine = logReader.ReadLine();
                                                                       if (currentLine.Contains(" Failed "))
                                                                           toolTip.AppendLine(" - " + currentLine.Replace(" Failed ", "").Trim() + " failed.");
                                                                   }
                                                                   logReader.Close();
                                                               }
                                                               reader.Close();
                                                           }
                                                       });
        }

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

        private void RetrieveFailureResponsiblePerson()
        {
            Configuration.BuildConfigurations.Each(bc =>
                                                       {
                                                           string failedBy = null;
                                                           foreach(var build in bc.BuildDetails.OrderByDescending(bd => bd.GetBuildNumber()))
                                                           {
                                                               if (build.Status.CanConvert())
                                                               {
                                                                   var trayStatus = build.Status.ToTrayStatus(bc.Status);
                                                                   if (trayStatus == TrayStatus.Success || trayStatus == TrayStatus.SuccessInProgress)
                                                                       break;
                                                                   failedBy = build.RequestedFor;
                                                               }
                                                           }
                                                           bc.LastFailureBy = failedBy;
                                                       });
        }

        private void CalculateBuildTime(TrayStatus status)
        {
            var timeSpan = (DateTime.Now - Configuration.BuildConfigurations.Max(bc => bc.LastBuildTime));
            if (status == TrayStatus.FailureInProgress || status == TrayStatus.SuccessInProgress)
                _trayIcon.Text = "Build Started: " + timeSpan.ToDisplay() + " ago.";
            else
                _trayIcon.Text = "Last Build: " + timeSpan.ToDisplay() + " ago.";
        }


        public ConfigurationData Configuration { get; private set; }

        public Timer internalTimer
        {
            get { return _internalTimer; }
        }
    }
*/
}
