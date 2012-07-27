using System;
using System.Collections.Generic;
using fireBwall.Packets;
using fireBwall.Modules;

namespace fireBwall.Filters.NDIS
{
    public interface INDISFilter
    {
        AdapterInformation GetAdapterInformation();
        void ProcessPacket(Packet pkt);
        void SendPacket(Packet pkt);
        void StartProcessing();
        void StopProcessing();
        ModuleList Modules { get; set; }
        bool Filtering { get; set; }
        bool DropAll { get; set; }
        void ProcessLoop();
    }
}
