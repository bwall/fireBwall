using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace fireBwall.Filters.NDIS
{
    public class Ndisapi
    {
        public const int ADAPTER_LIST_SIZE = 32;
        public const int ADAPTER_NAME_SIZE = 256;
        public const uint DATA_LINK_LAYER_VALID = 0x00000001;
        public const int ETHER_ADDR_LENGTH = 6;
        public const uint ETH_802_3 = 0x00000001;
        public const uint ETH_802_3_DEST_ADDRESS = 0x00000002;
        public const uint ETH_802_3_PROTOCOL = 0x00000004;
        public const uint ETH_802_3_SRC_ADDRESS = 0x00000001;
        public const uint FILTER_PACKET_DROP = 0x00000002;
        public const uint FILTER_PACKET_PASS = 0x00000001;
        public const uint FILTER_PACKET_REDIRECT = 0x00000003;
        public const uint IPV4 = 0x00000001;
        public const uint IP_RANGE_V4_TYPE = 0x00000002;
        public const uint IP_SUBNET_V4_TYPE = 0x00000001;
        public const uint IP_V4_FILTER_DEST_ADDRESS = 0x00000002;
        public const uint IP_V4_FILTER_PROTOCOL = 0x00000004;
        public const uint IP_V4_FILTER_SRC_ADDRESS = 0x00000001;
        public const int MAX_ETHER_FRAME = 1514;
        public const uint MSTCP_FLAG_FILTER_DIRECT = 0x00000010;
        public const uint MSTCP_FLAG_LOOPBACK_BLOCK = 0x00000040;
        public const uint MSTCP_FLAG_LOOPBACK_FILTER = 0x00000020;
        public const uint MSTCP_FLAG_RECV_LISTEN = 0x00000008;
        public const uint MSTCP_FLAG_RECV_TUNNEL = 0x00000002;
        public const uint MSTCP_FLAG_SENT_LISTEN = 0x00000004;
        public const uint MSTCP_FLAG_SENT_TUNNEL = 0x00000001;
        public const String NDISRD_DRIVER_NAME = "NDISRD";
        public const uint NETWORK_LAYER_VALID = 0x00000002;
        public const int RAS_LINKS_MAX = 256;
        public const int RAS_LINK_BUFFER_LENGTH = 1024;
        public const uint TCPUDP = 0x00000001;
        public const uint TCPUDP_DEST_PORT = 0x00000002;
        public const uint TCPUDP_SRC_PORT = 0x00000001;
        public const uint TRANSPORT_LAYER_VALID = 0x00000004;

        [DllImport("ndisapi.dll")]
        public static extern void CloseFilterDriver(IntPtr hOpen);

        public static unsafe string ConvertAdapterName(byte* bAdapterName, uint dwPlatformId, uint dwMajorVersion)
        {
            byte[] szAdapterName = new byte[256];
            bool success = false;
            string res = null;
            fixed (byte* pbFriendlyName = szAdapterName)
            {
                if (dwPlatformId == 2/*VER_PLATFORM_WIN32_NT*/)
                {
                    if (dwMajorVersion > 4)
                    {
                        // Windows 2000 or XP
                        success = Ndisapi.ConvertWindows2000AdapterName(bAdapterName, pbFriendlyName, (uint)szAdapterName.Length);
                    }
                    else if (dwMajorVersion == 4)
                    {
                        // Windows NT 4.0
                        success = Ndisapi.ConvertWindowsNTAdapterName(bAdapterName, pbFriendlyName, (uint)szAdapterName.Length);
                    }
                }
                else
                {
                    // Windows 9x/ME
                    success = Ndisapi.ConvertWindows9xAdapterName(bAdapterName, pbFriendlyName, (uint)szAdapterName.Length);
                }
                if (success)
                {
                    int zero_index = 0;
                    while (zero_index < 256 && szAdapterName[zero_index] != 0)
                        ++zero_index;
                    res = System.Text.Encoding.ASCII.GetString(szAdapterName, 0, zero_index);
                }
            }
            return res;
        }

        public static unsafe string ConvertAdapterName(byte[] bAdapterName, int iNameStart, uint dwPlatformId, uint dwMajorVersion)
        {
            fixed (byte* tmp = &bAdapterName[iNameStart])
            {
                return ConvertAdapterName(tmp, dwPlatformId, dwMajorVersion);
            }
        }

        [DllImport("ndisapi.dll")]
        public static extern unsafe bool ConvertWindows2000AdapterName(byte* szAdapterName, byte* szUserFriendlyName, uint
dwUserFriendlyNameLength);

        [DllImport("ndisapi.dll")]
        public static extern unsafe bool ConvertWindows9xAdapterName(byte* szAdapterName, byte* szUserFriendlyName, uint
dwUserFriendlyNameLength);

        [DllImport("ndisapi.dll")]
        public static extern unsafe bool ConvertWindowsNTAdapterName(byte* szAdapterName, byte* szUserFriendlyName, uint
dwUserFriendlyNameLength);

        [DllImport("ndisapi.dll")]
        public static extern bool FlushAdapterPacketQueue(IntPtr hOpen, IntPtr hAdapter);

        [DllImport("ndisapi.dll")]
        public static extern bool GetAdapterMode(IntPtr hOpen, ref ADAPTER_MODE Mode);

        [DllImport("ndisapi.dll")]
        public static extern bool GetAdapterPacketQueueSize(IntPtr hOpen, IntPtr hAdapter, ref uint dwSize);

        [DllImport("ndisapi.dll")]
        public static extern uint GetAdaptersStartupMode();

        [DllImport("ndisapi.dll")]
        public static extern uint GetBytesReturned(IntPtr hOpen);

        [DllImport("ndisapi.dll")]
        public static extern uint GetDriverVersion(IntPtr hOpen);

        [DllImport("ndisapi.dll")]
        public static extern bool GetHwPacketFilter(IntPtr hOpen, IntPtr hAdapter, ref uint pFilter);

        [DllImport("ndisapi.dll")]
        public static extern uint GetMTUDecrement();

        [DllImport("ndisapi.dll")]
        public static extern bool GetPacketFilterTable(IntPtr hOpen, ref STATIC_FILTER_TABLE pFilterList);

        [DllImport("ndisapi.dll")]
        public static extern bool GetPacketFilterTableResetStats(IntPtr hOpen, ref STATIC_FILTER_TABLE pFilterList);

        [DllImport("ndisapi.dll")]
        public static extern bool GetPacketFilterTableSize(IntPtr hOpen, ref uint pdwTableSize);

        [DllImport("ndisapi.dll")]
        public static extern bool GetRasLinks(IntPtr hOpen, IntPtr hAdapter, IntPtr pLinks);

        [DllImport("ndisapi.dll")]
        public static extern bool GetTcpipBoundAdaptersInfo(IntPtr hOpen, ref TCP_AdapterList Adapters);

        [DllImport("ndisapi.dll")]
        public static extern bool IsDriverLoaded(IntPtr hOpen);

        [DllImport("ndisapi.dll")]
        public static extern bool NdisrdRequest(IntPtr hOpen, ref PACKET_OID_DATA OidData, bool Set);

        public static IntPtr OpenFilterDriver(String FileName)
        {
            Encoding e = Encoding.GetEncoding("ISO-8859-1");
            byte[] szNdisrd = e.GetBytes(FileName);

            return OpenFilterDriver(szNdisrd);
        }

        [DllImport("ndisapi.dll")]
        public static extern IntPtr OpenFilterDriver(byte[] pszFileName);

        [DllImport("ndisapi.dll")]
        public static extern bool ReadPacket(IntPtr hOpen, ref ETH_REQUEST Packet);

        [DllImport("ndisapi.dll")]
        public static extern bool ReadPackets(IntPtr hOpen, ref ETH_M_REQUEST Packets);

        [DllImport("ndisapi.dll")]
        public static extern bool ResetPacketFilterTable(IntPtr hOpen);

        [DllImport("ndisapi.dll")]
        public static extern bool SendPacketToAdapter(IntPtr hOpen, ref ETH_REQUEST Packet);

        [DllImport("ndisapi.dll")]
        public static extern bool SendPacketToMstcp(IntPtr hOpen, ref ETH_REQUEST Packet);

        [DllImport("ndisapi.dll")]
        public static extern bool SendPacketsToAdapter(IntPtr hOpen, ref ETH_M_REQUEST Packets);

        [DllImport("ndisapi.dll")]
        public static extern bool SendPacketsToMstcp(IntPtr hOpen, ref ETH_M_REQUEST Packets);

        [DllImport("ndisapi.dll")]
        public static extern bool SetAdapterListChangeEvent(IntPtr hOpen, SafeWaitHandle hWin32Event);

        [DllImport("ndisapi.dll")]
        public static extern bool SetAdapterMode(IntPtr hOpen, ref ADAPTER_MODE Mode);

        [DllImport("ndisapi.dll")]
        public static extern bool SetAdaptersStartupMode(uint dwStartupMode);

        [DllImport("ndisapi.dll")]
        public static extern bool SetHwPacketFilter(IntPtr hOpen, IntPtr hAdapter, uint Filter);

        [DllImport("ndisapi.dll")]
        public static extern bool SetMTUDecrement(uint dwMTUDecrement);

        [DllImport("ndisapi.dll")]
        public static extern bool SetPacketEvent(IntPtr hOpen, IntPtr hAdapter, SafeWaitHandle hWin32Event);

        [DllImport("ndisapi.dll")]
        public static extern bool SetPacketFilterTable(IntPtr hOpen, ref STATIC_FILTER_TABLE pFilterList);

        [DllImport("ndisapi.dll")]
        public static extern bool SetWANEvent(IntPtr hOpen, SafeWaitHandle hWin32Event);
    }

    /* Specify here packed structures for data exchange with driver */
    /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/
    //
    // TCP_AdapterList structure used for requesting information about currently bound TCPIP adapters
    //
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TCP_AdapterList
    {
        public uint m_nAdapterCount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Ndisapi.ADAPTER_LIST_SIZE * Ndisapi.ADAPTER_NAME_SIZE)]
        public byte[] m_szAdapterNameList;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Ndisapi.ADAPTER_LIST_SIZE)]
        public IntPtr[] m_nAdapterHandle;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Ndisapi.ADAPTER_LIST_SIZE)]
        public uint[] m_nAdapterMediumList;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Ndisapi.ADAPTER_LIST_SIZE * Ndisapi.ETHER_ADDR_LENGTH)]
        public byte[] m_czCurrentAddress;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Ndisapi.ADAPTER_LIST_SIZE)]
        public ushort[] m_usMTU;
    }


    //
    // NDISRD_ETH_Packet is a container for INTERMEDIATE_BUFFER pointer
    // This structure can be extended in the future versions
    //
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NDISRD_ETH_Packet
    {
        public IntPtr Buffer;
    }

    //
    // ETH_REQUEST used for passing the "read packet" request to driver
    //
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ETH_REQUEST
    {
        public IntPtr hAdapterHandle;
        public NDISRD_ETH_Packet EthPacket;
    }

    //
    // ETH_M_REQUEST used for passing the blocks of packets to and from the driver
    //
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ETH_M_REQUEST
    {
        public IntPtr hAdapterHandle;
        public uint dwPacketsNumber;
        public uint dwPacketsSuccess;

        /// // For easier marshalling used the fixed size of the array, change if you plan to read packets by other chunks
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)] // For easier marshalling used the fixed size of the array, change if you plan to read packets by other chunks
        public NDISRD_ETH_Packet[] EthPacket;
    }

    //
    // ADAPTER_MODE used for setting adapter mode
    //
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ADAPTER_MODE
    {
        public IntPtr hAdapterHandle;
        public uint dwFlags;
    }

    //
    // PACKET_OID_DATA used for passing NDIS_REQUEST to driver
    //
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PACKET_OID_DATA
    {
        public IntPtr hAdapterHandle;
        public uint Oid;
        public uint Length;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1)]
        public byte[] Data;
    }

    //
    // Getting WAN links definitions
    //
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RAS_LINK_INFO
    {
        public uint LinkSpeed;
        public uint MaximumTotalSize;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Ndisapi.ETHER_ADDR_LENGTH)]
        public byte[] RemoteAddress;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Ndisapi.ETHER_ADDR_LENGTH)]
        public byte[] LocalAddress;
        public uint ProtocolBufferLength;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Ndisapi.RAS_LINK_BUFFER_LENGTH)]
        public byte[] ProtocolBuffer;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RAS_LINKS
    {
        public uint nNumberOfLinks;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Ndisapi.RAS_LINKS_MAX)]
        RAS_LINK_INFO[] RasLinks;
    }

    //
    // Packet filter definitions
    //

    //
    // Ethernet 802.3 filter type
    //

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ETH_802_3_FILTER
    {
        public uint m_ValidFields;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Ndisapi.ETHER_ADDR_LENGTH)]
        public byte[] m_SrcAddress;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Ndisapi.ETHER_ADDR_LENGTH)]
        public byte[] m_DestAddress;
        public ushort m_Protocol;
        public ushort Padding;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IP_SUBNET_V4
    {
        public uint m_Ip;
        public uint m_IpMask;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IP_RANGE_V4
    {
        public uint m_StartIp;
        public uint m_EndIp;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct IP_ADDRESS_V4
    {
        [System.Runtime.InteropServices.FieldOffset(0)]
        public uint m_AddressType;

        [System.Runtime.InteropServices.FieldOffset(4)]
        public IP_SUBNET_V4 m_IpSubnet;

        [System.Runtime.InteropServices.FieldOffset(4)]
        public IP_RANGE_V4 m_IpRange;
    }

    //
    // IP version 4 filter type
    //

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IP_V4_FILTER
    {
        public uint m_ValidFields;
        public IP_ADDRESS_V4 m_SrcAddress;
        public IP_ADDRESS_V4 m_DestAddress;
        public byte m_Protocol;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Padding;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PORT_RANGE
    {
        public ushort m_StartRange;
        public ushort m_EndRange;
    }

    //
    // TCP & UDP filter
    //

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TCPUDP_FILTER
    {
        public uint m_ValidFields;
        public PORT_RANGE m_SourcePort;
        public PORT_RANGE m_DestPort;
    }

    //
    // Represents data link layer (OSI-7) filter level
    //

    // Weird, but I can't create the same binary structure with Explicit
    // "because it contains an object field at offset 4 that is incorrectly aligned or overlapped by a non-object field"
    // It wants the second field to be at 8 offset, while Sequental places it on 4 bytes offset

    /*[StructLayout(LayoutKind.Explicit, Pack=8)]
    public struct DATA_LINK_LAYER_FILTER
    {
            [FieldOffset(0)]
            public uint            m_dwUnionSelector;
            [FieldOffset(4)]
        public ETH_802_3_FILTER m_Eth8023Filter;
}*/

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DATA_LINK_LAYER_FILTER
    {
        public uint m_dwUnionSelector;
        public ETH_802_3_FILTER m_Eth8023Filter;
    }

    //
    // Represents network layer (OSI-7) filter level
    //

    /*[StructLayout(LayoutKind.Explicit, Pack=8)]
    public struct NETWORK_LAYER_FILTER
    {
            [System.Runtime.InteropServices.FieldOffset(0)]
            public uint        m_dwUnionSelector;
            [System.Runtime.InteropServices.FieldOffset(4)]
            public IP_V4_FILTER m_IPv4;
    }*/

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NETWORK_LAYER_FILTER
    {
        public uint m_dwUnionSelector;
        public IP_V4_FILTER m_IPv4;
    }

    // Represents transport layer (OSI-7) filter level

    /*[StructLayout(LayoutKind.Explicit, Pack=8)]
    public struct TRANSPORT_LAYER_FILTER
    {
            [System.Runtime.InteropServices.FieldOffset(0)]
            public uint            m_dwUnionSelector;
            [System.Runtime.InteropServices.FieldOffset(4)]
            public TCPUDP_FILTER    m_TcpUdp;
    }*/

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TRANSPORT_LAYER_FILTER
    {
        public uint m_dwUnionSelector;
        public TCPUDP_FILTER m_TcpUdp;
    }

    //
    // Defines static filter entry
    //

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct STATIC_FILTER
    {
        public ulong m_Adapter;
        public uint m_dwDirectionFlags;
        public uint m_FilterAction;
        public uint m_ValidFields;
        public uint m_LastReset;
        public ulong m_Packets;
        public ulong m_Bytes;
        public DATA_LINK_LAYER_FILTER m_DataLinkFilter;
        public NETWORK_LAYER_FILTER m_NetworkFilter;
        public TRANSPORT_LAYER_FILTER m_TransportFilter;
    }

    //
    // Static filters table to be passed to WinpkFilter driver
    //
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct STATIC_FILTER_TABLE
    {
        public uint m_TableSize;

        /// // For convinience (easier marshalling to unmanaged memory) the size of the array is fixed to 256 entries
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)] // For convinience (easier marshalling to unmanaged memory) the size of the array is fixed to 256 entries
        public STATIC_FILTER[] m_StaticFilters;
    }
}
