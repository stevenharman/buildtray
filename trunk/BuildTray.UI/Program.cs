using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Resources;
using BuildTray.UI.Properties;
using BuildTray.Logic.Entities;
using BuildTray.Logic;
using BuildTray.Modules;
using StructureMap;

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

                BuildTrayContainerConfig.BootstrapStructureMapConfiguration();

                var info = new BuildInfo{
                    BuildName = "Phoenix_UnitTests",
                    ProjectName = "Phoenix",
                    ServerUrl = new Uri("http://vrp-tfs-000:8080")
                };

                //var configurationData = ObjectFactory.GetInstance<IConfigurationData>();
                //configurationData.Save();

                ObjectFactory.GetInstance<IConfigurationData>().PollingInterval = 30;
                ITrayController controller = ObjectFactory.GetInstance<ITrayController>();

                controller.AddCompletionModule("FailedTestAction", new FailedTestAction().GetAction());
                controller.Initialize();
                controller.Execute(info);
                
                Application.Run(context);
            }
        }
    }
}
