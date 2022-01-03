using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;

namespace Providers
{
    public class Network
    {
        public static string GetIPAddress()
        {
            string ip = new WebClient().DownloadString("https://api.ipify.org") ?? HttpContext.Current.Request.UserHostAddress;
            return ip;
        }


        public static string GetMACAddress()
        {
            String sMacAddress = string.Empty;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet && nic.OperationalStatus == OperationalStatus.Up)
                {
                    sMacAddress = nic.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }
    }    
}