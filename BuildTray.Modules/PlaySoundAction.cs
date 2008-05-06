using System;
using System.Linq;
using System.Runtime.InteropServices;
using BuildTray.Logic;
using BuildTray.Logic.Entities;
using BuildTray.Modules;
using BuildTray.Modules.ViewConfiguration;
using System.IO;

namespace BuildTray.Modules
{
    public class PlaySoundAction : IActionModuleDefinition
    {
        [DllImport("winmm.dll")]
        private static extern bool PlaySound(string pszName, IntPtr hModule, int dwFlags);

        private readonly IConfigurationData _configData;

        public PlaySoundAction(IConfigurationData config)
        {
            _configData = config;
        }

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

        public Type ConfigurationForm
        {
            get { return typeof(PlaySoundConfigurationView); }
        }

        private void Play(Build build, ITrayController controller)
        {
           // if (string.IsNullOrEmpty(_configData.ApplicationDataPath))
                //return;

            string soundPath = Path.Combine(_configData.ApplicationDataPath, "PlaySoundModule");
            
            //Temporary Hack
            soundPath = @"C:\Sounds";
            
            if (!Directory.Exists(soundPath))
                return;

            Build previousBuild = controller.CompletedBuilds
                .OrderByDescending(cb => cb.BuildNumber)
                .Where(cb => cb.BuildNumber != build.BuildNumber)
                .FirstOrDefault();

            if (build.Status == BuildStatuses.Passed &&
                (previousBuild == null || previousBuild.Status == BuildStatuses.Failed))
            {
                PlaySound(Path.Combine(soundPath, "PassedBuild.Wav"));
            }
            else if (build.Status == BuildStatuses.Failed &&
                     (previousBuild == null || previousBuild.Status == BuildStatuses.Passed))
            {
                PlaySound(Path.Combine(soundPath, "FailedBuild.Wav"));
            }
            else if (build.Status == BuildStatuses.Failed &&
                     (previousBuild == null || previousBuild.Status == BuildStatuses.Failed))
            {
                PlaySound(Path.Combine(soundPath, "FailedBuildAgain.Wav"));
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