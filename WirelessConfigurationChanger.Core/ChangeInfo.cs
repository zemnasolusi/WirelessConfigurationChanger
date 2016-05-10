using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using WirelessConfigurationChanger.Enum;

namespace WirelessConfigurationChanger
{
    public class ChangeInfo : INotifyPropertyChanged
    {
        #region " IpAddress "

        /// <summary>
        /// The <see cref="IpAddress" /> property's name.
        /// </summary>
        public const string IpAddressPropertyName = "IpAddress";

        private ChangeStatus _ipAddress = ChangeStatus.Changing;

        /// <summary>
        /// Sets and gets the IpAddress property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ChangeStatus IpAddress
        {
            get
            {
                return _ipAddress;
            }

            set
            {
                if (_ipAddress == value)
                {
                    return;
                }

                _ipAddress = value;
                RaisePropertyChanged(IpAddressPropertyName);
            }
        }

        #endregion

        #region " Dns "

        /// <summary>
        /// The <see cref="Dns" /> property's name.
        /// </summary>
        public const string DnsPropertyName = "Dns";

        private ChangeStatus _dns = ChangeStatus.Changing;

        /// <summary>
        /// Sets and gets the Dns property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ChangeStatus Dns
        {
            get
            {
                return _dns;
            }

            set
            {
                if (_dns == value)
                {
                    return;
                }

                _dns = value;
                RaisePropertyChanged(DnsPropertyName);
            }
        }

        #endregion

        #region " ChangeStatus "

        /// <summary>
        /// The <see cref="ChangeStatus" /> property's name.
        /// </summary>
        public const string ChangeStatusPropertyName = "ChangeStatus";

        /// <summary>
        /// Sets and gets the ChangeStatus property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ChangeStatus ChangeStatus
        {
            get
            {
                if (this.IpAddress == Enum.ChangeStatus.Failed || this.Dns == Enum.ChangeStatus.Failed)
                    return Enum.ChangeStatus.Failed;

                if (this.IpAddress == Enum.ChangeStatus.Success && this.Dns == Enum.ChangeStatus.Success)
                    return Enum.ChangeStatus.Success;

                return Enum.ChangeStatus.Changing;
            }
        }

        #endregion

        #region " TryCount "

        /// <summary>
        /// The <see cref="TryCount" /> property's name.
        /// </summary>
        public const string TryCountPropertyName = "TryCount";

        private int _tryCount = 0;

        /// <summary>
        /// Sets and gets the TryCount property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int TryCount
        {
            get
            {
                return _tryCount;
            }

            set
            {
                if (_tryCount == value)
                {
                    return;
                }

                _tryCount = value;
                RaisePropertyChanged(TryCountPropertyName);
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
