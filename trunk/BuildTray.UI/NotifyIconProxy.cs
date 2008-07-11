using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BuildTray.Logic;
using BuildTray.Modules;
using BuildTray.UI.Properties;
using NotifyIcon=System.Windows.Forms.NotifyIcon;
using ToolTipIcon=System.Windows.Forms.ToolTipIcon;

namespace BuildTray.UI
{
    public class NotifyIconProxy : INotifyIcon
    {
        private readonly NotifyIcon _icon;

        public NotifyIconProxy()
        {
            _icon = new NotifyIcon();

            //wire up passthrough of events.
            _icon.DoubleClick += (s, e) => DoubleClick.Raise(this, e);
            _icon.BalloonTipClicked += (s, e) => BalloonTipClicked.Raise(this, e);
            _icon.BalloonTipClosed += (s, e) => BalloonTipClosed.Raise(this, e);
            _icon.BalloonTipShown += (s, e) => BalloonTipShown.Raise(this, e);
        }

        private static readonly Dictionary<TrayStatus, Icon> icons = new Dictionary<TrayStatus, Icon>();

        public Icon Icon
        {
            get { return _icon.Icon; }
            set { _icon.Icon = value; }
        }

        public event EventHandler DoubleClick;
        public event EventHandler BalloonTipClicked;
        public event EventHandler BalloonTipClosed;
        public event EventHandler BalloonTipShown;

        public ToolTipIcon BalloonTipIcon
        {
            get { return _icon.BalloonTipIcon; }
            set { _icon.BalloonTipIcon = value; }
        }

        public string BalloonTipTitle
        {
            get { return _icon.BalloonTipTitle; }
            set { _icon.BalloonTipTitle = value; }
        }

        public string BalloonTipText
        {
            get { return _icon.BalloonTipText; }
            set { _icon.BalloonTipText = value; }
        }

        public ContextMenuStrip ContextMenuStrip
        {
            get { return _icon.ContextMenuStrip; }
            set { _icon.ContextMenuStrip = value; }
        }

        public string Text
        {
            get { return _icon.Text; }
            set { _icon.Text = value; }
        }

        public bool Visible
        {
            get { return _icon.Visible; }
            set { _icon.Visible = value; }
        }

        public void ShowBalloonTip(int timeout)
        {
            _icon.ShowBalloonTip(timeout);
        }

        public void ShowBalloonTip(int timeout, string tipTitle, string tipText, ToolTipIcon tipIcon)
        {
            _icon.ShowBalloonTip(timeout, tipTitle, tipText, tipIcon);
        }

        public TrayStatus CurrentStatus()
        {
            return icons.Single(s => s.Value == _icon.Icon).Key;
        }

        public void Success()
        {
            if (!icons.ContainsKey(TrayStatus.Success))
                icons.Add(TrayStatus.Success, Icon.FromHandle(Resources.Success2.GetHicon()));
            _icon.Icon = icons[TrayStatus.Success];
        }

        public void Failure()
        {
            if (!icons.ContainsKey(TrayStatus.Failure))
                icons.Add(TrayStatus.Failure, Icon.FromHandle(Resources.Failure2.GetHicon()));
            _icon.Icon = icons[TrayStatus.Failure];
        }

        public void InProgress()
        {
            var pair = icons.Single(s => s.Value == _icon.Icon);
            switch (pair.Key)
            {
                case TrayStatus.Success:
                    if (!icons.ContainsKey(TrayStatus.SuccessInProgress))
                        icons.Add(TrayStatus.SuccessInProgress, Icon.FromHandle(Resources.SuccessRunning2.GetHicon()));
                    _icon.Icon = icons[TrayStatus.SuccessInProgress];
                    break;
                case TrayStatus.SuccessInProgress:
                case TrayStatus.FailureInProgress:
                    return;
                case TrayStatus.Failure:
                    if (!icons.ContainsKey(TrayStatus.FailureInProgress))
                        icons.Add(TrayStatus.FailureInProgress, Icon.FromHandle(Resources.FailureRunning2.GetHicon()));
                    _icon.Icon = icons[TrayStatus.FailureInProgress];
                    break;
            }


        }
    }

}
