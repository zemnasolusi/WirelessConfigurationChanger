using GalaSoft.MvvmLight.Messaging;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WirelessConfigurationChanger.Message
{
    public class ShowBalloonTipMessage : MessageBase
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public BalloonIcon Symbol { get; set; }

        public ShowBalloonTipMessage(string title, string text, BalloonIcon symbol)
        {
            this.Title = title;
            this.Text = text;
            this.Symbol = symbol;
        }
    }
}
