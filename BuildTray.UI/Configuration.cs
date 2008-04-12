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
        private ITFSServerProxy _proxy;

        public Configuration(ITFSServerProxy proxy)
        {
            InitializeComponent();

            _proxy = proxy;
        }

        public NotifyIcon TrayIcon { get; set; }
    }
}
