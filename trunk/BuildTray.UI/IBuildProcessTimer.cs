using System;
using BuildTray.Logic.Entities;

namespace BuildTray.UI
{
    public interface IBuildProcessTimer
    {
        void Start();
        void Stop();
        void AddBuild(BuildInfo info);
        void RemoveBuild(BuildInfo remove);
        event EventHandler<BuildDetailEventArgs> BuildStarted;
        event EventHandler<BuildDetailEventArgs> BuildCompleted;
        event EventHandler<BuildDetailEventArgs> BuildIgnored;
    }
}
