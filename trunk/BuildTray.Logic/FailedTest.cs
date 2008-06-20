using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildTray.Logic
{
    public class FailedTest
    {
        public string ClassName { get; set; }
        public string TestName { get; set; }
        public string Output { get; set; }
    }
}
