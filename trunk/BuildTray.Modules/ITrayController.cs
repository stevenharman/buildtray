using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using BuildTray.Logic.Entities;

namespace BuildTray.Modules
{
    public interface ITrayController
    {
        Build MostRecentCompletedBuild { get; }
        ReadOnlyCollection<Build> CompletedBuilds { get; }
        INotifyIcon NotifyIcon { get; }
        void Initialize();
        void Execute(BuildInfo buildInfo);
        void AddCompletionModule(string name, Action<Build, ITrayController> action);
        void AddInProgressModule(string name, Action<Build, ITrayController> action);
        void AddCompletedBuild(Build build);
        void AddRangeCompletedBuild(IEnumerable<Build> builds);
        ContextMenuStrip CreateMenuStrip();
        IEnumerable<string> CompletedBuildActionNames { get; }
        IEnumerable<string> InProgressBuildActionNames { get; }
        void RemoveInProgressBuildAction(string name);
        void RemoveCompletedBuildAction(string name);
    }
}