using System;
using System.Collections.Generic;
using System.Text;

namespace fireBwall.Packets
{
    /// <summary>
    /// Encrypted ethernet packet
    /// (Not yet implemented)
    /// </summary>
    public unsafe class EETHPacket : EthPacket
    {
        public EETHPacket(EthPacket eth)
            : base(eth.data)
        {
        }

        public EETHPacket(INTERMEDIATE_BUFFER* in_packet)
            : base(in_packet)
        {
        }

        public override bool ContainsLayer(Protocol layer)
        {
            if (layer == Protocol.EEth)
                return true;
            else
                return base.ContainsLayer(layer);
        }

        public override Protocol GetHighestLayer()
        {
            return Protocol.EEth;
        }

        public override Packet MakeNextLayerPacket()
        {
            return this;
        }
    }
}
