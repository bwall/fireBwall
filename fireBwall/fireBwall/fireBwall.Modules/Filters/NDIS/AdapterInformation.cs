using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using fireBwall.Utils;

namespace fireBwall.Filters.NDIS
{
    public class AdapterInformation
    {
        private NetworkInterface ni;
        public NetworkInterface InterfaceInformation
        {
            get { return ni; }
            set { ni = value; }
        }

        public IPAddr IPv4
        {
            get
            {
                if (ni == null)
                    return null;
                else
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            return new IPAddr(ip.Address);
                        }
                    }
                    return null;
                }
            }
        }

        public IPAddr IPv6
        {
            get
            {
                if (ni == null)
                    return null;
                else
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                        {
                            return new IPAddr(ip.Address);
                        }
                    }
                    return null;
                }
            }
        }

        public string Description
        {
            get
            {
                if (ni == null)
                    return null;
                return ni.Description;
            }
        }

        public string Name
        {
            get
            {
                if (ni == null)
                    return null;
                return ni.Name;
            }
        }

        public string Id
        {
            get
            {
                if (ni == null)
                    return null;
                return ni.Id;
            }
        }

        public IPAddr GatewayIPv4
        {
            get
            {
                foreach (GatewayIPAddressInformation ip in ni.GetIPProperties().GatewayAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        return new IPAddr(ip.Address);
                }
                return null;
            }
        }

        public IPAddr GatewayIPv6
        {
            get
            {
                foreach (GatewayIPAddressInformation ip in ni.GetIPProperties().GatewayAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                        return new IPAddr(ip.Address);
                }
                return null;
            }
        }

        public BandwidthCounter DataIn
        {
            get;
            set;
        }

        public BandwidthCounter DataOut
        {
            get;
            set;
        }

        public string Summary
        {
            get
            {
                string ret = Name + "\t\t" + "In(" + DataIn.ToString() + " | " + DataIn.GetPerSecond() + ")\tOut(" + DataOut.ToString() + " | " + DataOut.GetPerSecond() + ")\r\n";
                ret += "MAC Address:\t" + ni.GetPhysicalAddress().ToString() + "\r\n";
                ret += "IP Addresses:\t" + IPv4 + " \t" + IPv6 + "\r\n";
                if (GatewayIPv4 != null || GatewayIPv6 != null)
                {
                    ret += "Gateway:\t\t";
                    if (GatewayIPv4 != null)
                        ret += GatewayIPv4 + "\t";
                    if (GatewayIPv6 != null)
                        ret += GatewayIPv6;
                }
                ret += "\r\nDNS Addresses:\t";

                foreach (System.Net.IPAddress ip in ni.GetIPProperties().DnsAddresses)
                {
                    ret += ip.ToString() + " \t";
                }
                return ret;
            }
        }

        public INDISFilter Adapter
        {
            get;
            set;
        }
    }
}
