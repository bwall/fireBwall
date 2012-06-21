using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using fireBwall.Modules;
using fireBwall.Packets;
using fireBwall.Utils;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace fireBwall.Filters.NDIS
{
    public class WinpkFilter : INDISFilter
    {
        #region Constructor

        public WinpkFilter(IntPtr hNdisapi, IntPtr adapterHandle, string name, bool filter = true, bool dropall = false) 
        {
            this.Filtering = filter;
            this.DropAll = dropall;
            this.hNdisapi = hNdisapi;
			this.adapterHandle = adapterHandle;
            name = name.Substring(0, name.IndexOf((char)0x00));
			foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
			{
                if (name.StartsWith("\\DEVICE\\" + ni.Id))
                {
                    inter = new AdapterInformation();
                    inter.InterfaceInformation = ni;
                    inter.DataIn = new BandwidthCounter();
                    inter.DataOut = new BandwidthCounter();
                }
			}
		}

        #endregion

        #region Variables

        ManualResetEvent hEvent = null;
        AdapterInformation inter = new AdapterInformation();
        ADAPTER_MODE mode = new ADAPTER_MODE();
        bool processing = false;
        Thread processingThread = null;
        PcapFileWriter pcaplog;
        IntPtr hNdisapi = IntPtr.Zero;
        Queue<Packet> processQueue = new Queue<Packet>();
        public IntPtr adapterHandle = IntPtr.Zero;

        #endregion

        #region Functions

        public void UpdateNetworkInterface(string name)
        {
            name = name.Substring(0, name.IndexOf((char)0x00));
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (name.EndsWith(ni.Id))
                {
                    inter.InterfaceInformation = ni;
                }
            }
        }

        void SetAdapterMode()
        {
            mode.dwFlags = Ndisapi.MSTCP_FLAG_SENT_TUNNEL | Ndisapi.MSTCP_FLAG_RECV_TUNNEL;
            mode.hAdapterHandle = adapterHandle;
            Ndisapi.SetAdapterMode(hNdisapi, ref mode);
        }

        void SetPacketEvent()
        {
            hEvent = new ManualResetEvent(false);
            Ndisapi.SetPacketEvent(hNdisapi, adapterHandle, hEvent.SafeWaitHandle);
        }

        public void StartProcessing()
        {
            SetAdapterMode();
            SetPacketEvent();
            processingThread = new Thread(ProcessLoop);
            processingThread.Name = "ProcessLoop for " + this.inter.Name;
            processing = true;
            processingThread.Start();
        }

        public void StopProcessing()
        {
            if (processingThread != null)
            {
                processingThread.Abort();
                processing = false;
                hEvent.Close();
                Modules.ShutdownAllModules();
                pcaplog.Close();
            }
        }

        public AdapterInformation GetAdapterInformation()
        {
            inter.Adapter = this;
            return inter;
        }

        public void ProcessPacket(Packets.Packet pkt)
        {
            processQueue.Enqueue(pkt);
        }

        public unsafe void SendPacket(Packets.Packet pkt)
        {
            bool worked = false;
            if (pkt.Outbound)
            {
                ETH_REQUEST Request = new ETH_REQUEST();
                INTERMEDIATE_BUFFER* ib = pkt.IB;
                Request.EthPacket.Buffer = (IntPtr)ib;
                Request.hAdapterHandle = adapterHandle;
                if (!Ndisapi.SendPacketToAdapter(hNdisapi, ref Request))
                {
                    worked = false;
                }
                else
                {
                    worked = true;
                }
            }
            else
            {
                ETH_REQUEST Request = new ETH_REQUEST();
                INTERMEDIATE_BUFFER* ib = pkt.IB;
                Request.EthPacket.Buffer = (IntPtr)ib;
                Request.hAdapterHandle = adapterHandle;
                worked = Ndisapi.SendPacketToMstcp(hNdisapi, ref Request);
            }
            if (pkt.CodeGenerated)
                pkt.ClearGeneratedPacket();
        }

        public Modules.ModuleList Modules { get; set; }

        public bool Filtering { get; set; }

        public bool DropAll { get; set; }

        public unsafe void ProcessLoop()
        {
            // Allocate and initialize packet structures
            ETH_REQUEST Request = new ETH_REQUEST();
            INTERMEDIATE_BUFFER PacketBuffer = new INTERMEDIATE_BUFFER();

            IntPtr PacketBufferIntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(PacketBuffer));
            try
            {
                win32api.ZeroMemory(PacketBufferIntPtr, Marshal.SizeOf(PacketBuffer));

                Request.hAdapterHandle = adapterHandle;
                Request.EthPacket.Buffer = PacketBufferIntPtr;

                Modules = new ModuleList(this);

                Modules.LoadExternalModules();

                Modules.UpdateModuleOrder();

                string folder = Configuration.ConfigurationManagement.Instance.ConfigurationPath;
                folder = folder + System.IO.Path.DirectorySeparatorChar + "pcapLogs";
                if (!System.IO.Directory.Exists(folder))
                    System.IO.Directory.CreateDirectory(folder);
                string f = folder + System.IO.Path.DirectorySeparatorChar + "blocked-" + inter.Name + "-" + DateTime.Now.ToBinary() + ".pcap";
                pcaplog = new PcapFileWriter(f);

                INTERMEDIATE_BUFFER* PacketPointer;

                while (true)
                {
                    hEvent.WaitOne();
                    while (Ndisapi.ReadPacket(hNdisapi, ref Request))
                    {
                        PacketPointer = (INTERMEDIATE_BUFFER*)PacketBufferIntPtr;
                        Packet pkt = new EthPacket(PacketPointer).MakeNextLayerPacket();

                        if (pkt.Outbound)
                        {
                            inter.DataOut.AddBits(pkt.Length());
                        }
                        else
                        {
                            inter.DataIn.AddBits(pkt.Length());
                        }

                        bool drop = false;
                        bool log = false;

                        if (this.Filtering)
                        {
                            for (int x = 0; x < Modules.Count; x++)
                            {
                                NDISModule fm = Modules.GetModule(x);
                                if (fm == null)
                                    continue;
                                int pmr = fm.PacketMain(ref pkt);
                                if (pmr == 0)
                                    continue;
                                if ((pmr & (int)PacketMainReturnType.LogPacket) == (int)PacketMainReturnType.LogPacket)
                                {
                                    log = true;
                                }
                                if ((pmr & (int)PacketMainReturnType.Drop) == (int)PacketMainReturnType.Drop)
                                {
                                    drop = true;
                                    break;
                                }
                            }
                        }

                        if (!drop && !DropAll)
                        {
                            if (pkt.Outbound)
                                Ndisapi.SendPacketToAdapter(hNdisapi, ref Request);
                            else
                                Ndisapi.SendPacketToMstcp(hNdisapi, ref Request);
                        }
                        if(log)
                            pcaplog.AddPacket(pkt.Data(), (int)pkt.Length());
                    }

                    //OM NOM NOM PASTA!
                    while (processQueue.Count != 0)
                    {
                        Packet pkt = processQueue.Dequeue().MakeNextLayerPacket();

                        if (pkt.Outbound)
                        {
                            inter.DataOut.AddBits(pkt.Length());
                        }
                        else
                        {
                            inter.DataIn.AddBits(pkt.Length());
                        }

                        bool drop = false;
                        bool log = false;

                        if (this.Filtering)
                        {
                            for (int x = 0; x < Modules.Count; x++)
                            {
                                NDISModule fm = Modules.GetModule(x);
                                if (fm == null)
                                    continue;
                                int pmr = fm.PacketMain(ref pkt);
                                if (pmr == 0)
                                    continue;
                                if ((pmr & (int)PacketMainReturnType.LogPacket) == (int)PacketMainReturnType.LogPacket)
                                {
                                    log = true;
                                }
                                if ((pmr & (int)PacketMainReturnType.Drop) == (int)PacketMainReturnType.Drop)
                                {
                                    drop = true;
                                    break;
                                }
                            }
                        }

                        if (!drop && !DropAll)
                        {
                            if (pkt.Outbound)
                                Ndisapi.SendPacketToAdapter(hNdisapi, ref Request);
                            else
                                Ndisapi.SendPacketToMstcp(hNdisapi, ref Request);
                        }
                        if (log)
                            pcaplog.AddPacket(pkt.Data(), (int)pkt.Length());
                    }
                    hEvent.Reset();
                }
            }
            catch (Exception tae)
            {                
                Marshal.FreeHGlobal(PacketBufferIntPtr);
                Logging.LogCenter.Instance.LogException(tae);
            }
        }

        #endregion
    }
}
