using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WirelessConfigurationChanger.Message
{
    public class StatusMessage : MessageBase
    {
        public string Status { get; set; }

        public StatusMessage(string status)
        {
            this.Status = status;
        }
    }
}
