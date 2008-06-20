using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildTray.Logic
{
    public class ExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
    }
}
