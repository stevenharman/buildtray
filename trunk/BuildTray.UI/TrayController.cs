﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using BuildTray.Logic;
using BuildTray.Logic.Entities;
using BuildTray.Modules;

namespace BuildTray.UI
{
    public class TrayController : ITrayController
    {
        public TrayController(INotifyIcon notifyIcon, IBuildProcessTimer processTimer)
        {
            _notifyIcon = notifyIcon;
            _processTimer = processTimer;
            _completedBuilds = new List<Build>();
        }

        private readonly INotifyIcon _notifyIcon;
        private readonly IBuildProcessTimer _processTimer;
        private Build _currentRunningBuild;
        private readonly List<Build> _completedBuilds;
        private BuildInfo _currentBuildInfo;
        private readonly Dictionary<string, Action<Build, ITrayController>> _completedBuildActions = new Dictionary<string, Action<Build, ITrayController>>();
        private readonly Dictionary<string, Action<Build, ITrayController>> _inProgressBuildActions = new Dictionary<string, Action<Build, ITrayController>>();

        public IEnumerable<string> CompletedBuildActionNames
        {
            get { return _completedBuildActions.Keys; }
        }

        public IEnumerable<string> InProgressBuildActionNames
        {
            get { return _inProgressBuildActions.Keys; }
        }

        public void RemoveCompletedBuildAction(string name)
        {
            _completedBuildActions.Remove(name);
        }

        public void RemoveInProgressBuildAction(string name)
        {
            _inProgressBuildActions.Remove(name);
        }

        public Build MostRecentCompletedBuild
        {
            get
            {
                return _completedBuilds.OrderByDescending(cb => cb.BuildNumber).FirstOrDefault();
            }
        }

        public ReadOnlyCollection<Build> CompletedBuilds
        {
            get { return _completedBuilds.AsReadOnly(); }
        }

        public INotifyIcon NotifyIcon
        {
            get { return _notifyIcon; }
        }

        public void Initialize()
        {
            _notifyIcon.ContextMenuStrip = CreateMenuStrip();
            _notifyIcon.Success();
            _notifyIcon.Visible = true;
            _notifyIcon.DoubleClick += (sender, e) =>
                            {
                                if (!string.IsNullOrEmpty(_notifyIcon.BalloonTipText))
                                    MessageBox.Show(_notifyIcon.BalloonTipText, "Build Failure");
                            };

            _processTimer.BuildCompleted += _processTimer_BuildCompleted;
            _processTimer.BuildStarted += _processTimer_BuildStarted;
            _processTimer.BuildIgnored += _processTimer_BuildIgnored;
            _processTimer.Start();
        }

       

        public void Execute(BuildInfo buildInfo)
        {
            if (buildInfo != _currentBuildInfo)
            {
                _processTimer.RemoveBuild(_currentBuildInfo);
                _currentBuildInfo = buildInfo;
                _processTimer.AddBuild(buildInfo);
            }
        }

        public void AddCompletionModule(string name, Action<Build, ITrayController> action)
        {
            _completedBuildActions[name] = action;
        }

        public void AddInProgressModule(string name, Action<Build, ITrayController> action)
        {
            _inProgressBuildActions[name] = action;
        }

        public void AddCompletedBuild(Build build)
        {
            _completedBuilds.Add(build);
        }

        public void AddRangeCompletedBuild(IEnumerable<Build> builds)
        {
            _completedBuilds.AddRange(builds);
        }

        void _processTimer_BuildIgnored(object sender, BuildDetailEventArgs e)
        {
            //If the build is ignored, we need to reset the status cak to what it should be.
            if (MostRecentCompletedBuild.Status == BuildStatuses.Passed)
                _notifyIcon.Success();
            else
                _notifyIcon.Failure();
        }

        void _processTimer_BuildStarted(object sender, BuildDetailEventArgs e)
        {
            _currentRunningBuild = e.Build.ToBuild();

            _notifyIcon.Text = "Build started " + (DateTime.Now - _currentRunningBuild.StartTime).ToDisplay() +
                               " ago.";

            if (_currentRunningBuild.StartTime >= e.MostRecentStartDate)
            {
                _notifyIcon.InProgress();

                _inProgressBuildActions.Values.Each(action => action(_currentRunningBuild, this));
            }
        }

        void _processTimer_BuildCompleted(object sender, BuildDetailEventArgs e)
        {
            _completedBuilds.Add(e.Build.ToBuild());

            if (_currentRunningBuild != null && _currentRunningBuild.BuildNumber == MostRecentCompletedBuild.BuildNumber)
                _currentRunningBuild = null;

            //If we are getting multiple builds then we don't want to show tool tips on each one.
            if (MostRecentCompletedBuild.StartTime == e.MostRecentStartDate)
            {
                _notifyIcon.Text = "Build completed " + (DateTime.Now - (MostRecentCompletedBuild.FinishTime
                                                                         ?? MostRecentCompletedBuild.StartTime)).
                                                            ToDisplay() + " ago.";

                if (MostRecentCompletedBuild.Status == BuildStatuses.Passed)
                    _notifyIcon.Success();
                else
                    _notifyIcon.Failure();

                _completedBuildActions.Values.Each(action => action(MostRecentCompletedBuild, this));
            }
        }

        public virtual ContextMenuStrip CreateMenuStrip()
        {
            var menuStrip = new ContextMenuStrip();
            var toolStripItem = menuStrip.Items.Add("E&xit");
            toolStripItem.Click += (sender, e) => Application.Exit();
            /*toolStripItem = menuStrip.Items.Add("&Configure Builds");
            toolStripItem.Click += (sender, e) =>
                                       {
                                           if (configurationForm == null || configurationForm.IsDisposed)
                                               configurationForm = new Configuration();
                                           configurationForm.TrayIcon = result;
                                           configurationForm.ShowDialog();
                                           configurationForm.Dispose();
                                       };*/
            return menuStrip;
        }
    }

}