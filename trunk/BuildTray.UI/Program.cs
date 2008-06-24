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
using System.Diagnostics;
using System.Text;

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
            //if (m_Mutex.WaitOne(1, true))
                if(true)
            {
                ApplicationContext context = new ApplicationContext();

                BuildTrayContainerConfig.BootstrapStructureMapConfiguration();

                var info = new BuildInfo{
                    BuildName = "Phoenix_UnitTests",
                    ProjectName = "Phoenix",
                    ServerUrl = new Uri("http://vrp-tfs-000.g2techinc.com:8080")
                };

                var configurationData = ObjectFactory.GetInstance<IConfigurationData>();
                //configurationData.Save();

                configurationData.PollingInterval = 30;
                ITrayController controller = ObjectFactory.GetInstance<ITrayController>();

                controller.AddCompletionModule("FailedTestAction", new FailedTestAction().GetAction());
                controller.AddCompletionModule("PlaySoundAction", ObjectFactory.GetInstance<PlaySoundAction>().GetAction());
                controller.Initialize();
                controller.Execute(info);

                Application.ThreadException += Application_ThreadException;

                Application.Run(context);
            }
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (!EventLog.SourceExists("BuildTray"))
                EventLog.CreateEventSource("BuildTray", "Application");

            var builder = new StringBuilder();
            builder.AppendLine(e.Exception.Message);
            builder.AppendLine();
            builder.AppendLine(e.Exception.StackTrace);

            EventLog.WriteEntry("BuildTray", builder.ToString());
        }
    }
}
