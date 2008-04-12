using System;
using System.Drawing;
using System.Windows.Forms;
using BuildTray.Logic;
using ToolTipIcon=System.Windows.Forms.ToolTipIcon;

namespace BuildTray.Modules
{
    public interface INotifyIcon
    {
        Icon Icon { get; set; }
        event EventHandler DoubleClick;
        event EventHandler BalloonTipClicked;
        event EventHandler BalloonTipClosed;
        event EventHandler BalloonTipShown;
        ToolTipIcon BalloonTipIcon { get; set; }
        string BalloonTipTitle { get; set; }
        string BalloonTipText { get; set; }
        ContextMenuStrip ContextMenuStrip { get; set; }
        string Text { get; set; }
        bool Visible { get; set; }
        void ShowBalloonTip(int timeout);
        void ShowBalloonTip(int timeout, string tipTitle, string tipText, ToolTipIcon tipIcon);
        void InProgress();
        void Failure();
        void Success();
        TrayStatus CurrentStatus();
    }
}