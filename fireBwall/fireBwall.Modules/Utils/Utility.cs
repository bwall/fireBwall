using System;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;

using fireBwall.Filters.NDIS;

namespace fireBwall.Utils
{
    public class Utility
    {
        [DllImport("msvcrt.dll")]        
        public static extern int memcmp(byte[] b1, byte[] b2, int count);

        public static bool ByteArrayEq(byte[] b1, byte[] b2)
        {
            if (b1.Length == b2.Length)
            {
                for (int x = 0; x < b1.Length; x++)
                {
                    if (b1[x] != b2[x])
                        return false;
                }
                return true;
            }
            return false;
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static IPAddr GetLocalIPAddress(INDISFilter adapter)
        {
            IPAddr address = new IPAddr();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            
            foreach (var adapt in nics)
            {
                // if this adapter matches the one we're looking for
                if (adapt.Id.Equals(adapter.GetAdapterInformation().Id))
                {
                    foreach(var i in adapt.GetIPProperties().UnicastAddresses)
                    {
                        if (i.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            address = new IPAddr(i.Address.GetAddressBytes());
                            return address;
                        }
                    }
                }
            }
            return null;
        }
    }
}
