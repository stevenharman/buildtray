using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using BuildTray.Logic;
using BuildTray.Logic.Entities;
using BuildTray.Modules;

namespace BuildTray.Modules
{
    public class PlaySoundAction : IActionModuleDefinition
    {
        [DllImport("winmm.dll")]
        private static extern bool PlaySound(string pszName, IntPtr hModule, int dwFlags);

        public Action<Build, ITrayController> GetAction()
        {
            return Play;
        }

        public string Description
        {
            get { return "Module used to play sounds when builds succeed and fail."; }
        }

        public string Name
        {
            get { return this.GetType().Name; }
        }

        private void Play(Build build, ITrayController controller)
        {
            Build previousBuild = controller.CompletedBuilds
                .OrderByDescending(cb => cb.BuildNumber)
                .Where(cb => cb.BuildNumber != build.BuildNumber)
                .FirstOrDefault();

            if (build.Status == BuildStatuses.Passed &&
                (previousBuild == null || previousBuild.Status == BuildStatuses.Failed))
            {
                PlaySound("PassedBuild.Wav");
            }
            else if (build.Status == BuildStatuses.Failed &&
                     (previousBuild == null || previousBuild.Status == BuildStatuses.Passed))
            {
                PlaySound("FailedBuild.Wav");
            }
            else if (build.Status == BuildStatuses.Failed &&
                     (previousBuild == null || previousBuild.Status == BuildStatuses.Failed))
            {
                PlaySound("FailedBuildAgain.Wav");
            }
        }

        public virtual void PlaySound(string soundFile)
        {
            try
            {
                PlaySound(soundFile, IntPtr.Zero, 0x1);
            }
            catch (Exception)
            {
            }
             
        }
    }
}