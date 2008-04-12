using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BuildTray.Logic;

namespace BuildTray.UI
{
    public partial class Configuration : Form
    {
        private TFSServerProxy proxy = new TFSServerProxy();

        public Configuration()
        {
            InitializeComponent();
        }

        public NotifyIcon TrayIcon { get; set; }
    }
}
