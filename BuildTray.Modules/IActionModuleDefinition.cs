using System;
using BuildTray.Logic.Entities;
using BuildTray.Modules;
using System.Windows.Forms;

namespace BuildTray.Modules
{
    public interface IActionModuleDefinition
    {
        Action<Build, ITrayController> GetAction();
        string Description { get; }
        string Name { get; }
        Type ConfigurationForm { get; }
    }
}