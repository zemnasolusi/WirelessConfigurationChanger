using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;

namespace WirelessConfigurationChanger
{
    public class NetworkAdapterManager
    {
        private NetworkInterface _ni;
        private ManagementClass _mc;
        private ManagementObjectCollection _moc;
        private ManagementObject _mo;

        public NetworkAdapterManager(NetworkInterface ni)
        {
            _ni = ni;
            _mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            _moc = _mc.GetInstances();
            _mo = _moc.Cast<ManagementObject>().SingleOrDefault(a => (bool)a["ipEnabled"] && a["SettingID"].Equals(ni.Id));

            if (_mo == null)
                throw new NullReferenceException("Can\'t find network adapter");
        }

        public IEnumerable<NetworkInterface> GetAllNetworkInterface()
        {
            var adapters = NetworkInterface.GetAllNetworkInterfaces()
                .Where(a => a.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                       a.NetworkInterfaceType == NetworkInterfaceType.Ethernet);

            return adapters;
        }

        public bool IsDhcpEnabled()
        {
            return (bool)_mo["DHCPEnabled"];
        }

        public bool EnableDhcp()
        {
            var enableDhcp = _mo.InvokeMethod("EnableDHCP", null, null);

            var ret = (UInt32)enableDhcp["returnValue"];
            return ret < 2;
        }

        public bool IsDnsServerAuto()
        {
            var key = Registry.LocalMachine
                .OpenSubKey("SYSTEM")
                .OpenSubKey("CurrentControlSet")
                .OpenSubKey("Services")
                .OpenSubKey("Tcpip")
                .OpenSubKey("Parameters")
                .OpenSubKey("Interfaces")
                .OpenSubKey(_ni.Id);

            return string.IsNullOrEmpty(key.GetValue("NameServer").ToString());
        }

        public IPAddress GetIPAddress()
        {
            var addresses = (string[])_mo["IPAddress"];
            return IPAddress.Parse(addresses.First());
        }

        public IPAddress GetSubnetMask()
        {
            var addresses = (string[])_mo["IPSubnet"];
            return IPAddress.Parse(addresses.First());
        }

        public bool SetIPAddress(IPAddress ipAddress, IPAddress subnetMask)
        {
            using (var newIp = _mo.GetMethodParameters("EnableStatic"))
            {
                newIp["IPAddress"] = new[] { ipAddress };
                newIp["SubnetMask"] = new[] { subnetMask };

                var setIp = _mo.InvokeMethod("EnableStatic", newIp, null);

                var ret = (UInt32)setIp["returnValue"];
                return ret < 2;
            }
        }

        public IPAddress GetGateway()
        {
            var addresses = (string[])_mo["DefaultIPGateway"];
            if (addresses != null && addresses.Count() > 0)
                return IPAddress.Parse(addresses.First());
            else
                return null;
        }

        public bool SetGatewy(IPAddress address)
        {
            using (var newGateway = _mo.GetMethodParameters("SetGateways"))
            {
                newGateway["DefaultIPGateway"] = new[] { address };
                newGateway["GatewayCostMetric"] = new[] { 1 };

                var setGateway = _mo.InvokeMethod("SetGateways", newGateway, null);

                var ret = (UInt32)setGateway["returnValue"];
                return ret < 2;
            }
        }

        public IEnumerable<IPAddress> GetNameServers()
        {
            var dnses = (string[])_mo["DNSServerSearchOrder"];

            var results = new List<IPAddress>();

            if (dnses != null)
            {
                foreach (var dns in dnses)
                {
                    results.Add(IPAddress.Parse(dns));
                }
            }

            return results;
        }

        public bool SetNameServers(IList<IPAddress> addresses)
        {
            using (var newDNS = _mo.GetMethodParameters("SetDNSServerSearchOrder"))
            {
                newDNS["DNSServerSearchOrder"] = addresses.ToArray();
                var setDNS = _mo.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);

                var ret = (UInt32)setDNS["returnValue"];
                return ret < 2;
            }
        }
    }
}
