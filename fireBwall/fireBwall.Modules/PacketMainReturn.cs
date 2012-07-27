using System;
using System.Runtime.InteropServices;
using System.Net;

namespace fireBwall.Modules
{
    /// <summary>
    /// Flags used to describe how the module handled the packet
    /// </summary>
    [Flags]
    public enum PacketMainReturnType
    {
        Error = 1,          //Reports an error in the packet processing
        Drop = 1 << 1,           //Drops the packet
        Allow = 1 << 2,          //Allows the packet to be passed on to the next module
        Edited = 1 << 3,        
        Log = 1 << 4,	        //Logs the packet
        Popup = 1 << 5,
        LogPacket = 1 << 6      //Requests that the Packet be logged in the Pcap logs
    }
}
