using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Resources;
using BuildTray.UI.Properties;
using BuildTray.Logic.Entities;
using BuildTray.Logic;
using BuildTray.Modules;

namespace BuildTray.UI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string m_uniqueIdentifier = Application.ExecutablePath.Replace(@"\", "_");
            Mutex m_Mutex = new Mutex(false, m_uniqueIdentifier);
            if (m_Mutex.WaitOne(1, true))
            {
                ApplicationContext context = new ApplicationContext();

                //ConfigurationData data = new ConfigurationData();
                //data.BuildConfigurations.Add(new BuildConfiguration { BuildName = "Phoenix_Release_1.0_UnitTests", ProjectName = "Phoenix", ServerUrl = new Uri("http://vrp-tfs-000:8080") });
                //data.BuildConfigurations.Add(new BuildConfiguration
                //                                 {
                //                                     BuildName = "Phoenix_UnitTests",
                //                                     ProjectName = "Phoenix",
                //                                     ServerUrl = new Uri("http://vrp-tfs-000:8080")
                //                                 });

                var info = new BuildInfo{
                    BuildName = "Phoenix_UnitTests",
                    ProjectName = "Phoenix",
                    ServerUrl = new Uri("http://vrp-tfs-000:8080")
                };

                var timer = new BuildProcessTimer(new TFSServerProxy());
                timer.PollingInterval = 30;
                TrayController controller = new TrayController(new NotifyIconProxy(), timer);
                controller.AddCompletionModule("FailedTestAction", new FailedTestAction().GetAction());
                controller.Initialize();
                controller.Execute(info);
                
                Application.Run(context);
            }
        }
    }
}
