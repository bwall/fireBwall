using System;
using System.Collections.Generic;
using fireBwall.Utils;
using System.Text;

namespace fireBwall.Utils
{
    public class Quad : IEquatable<Quad>
    {
        public IPAddr dstIP = null;
        public int dstPort = -1;
        public IPAddr srcIP = null;
        public int srcPort = -1;

        public override int GetHashCode()
        {
            return srcIP.GetHashCode() ^ dstIP.GetHashCode() ^ dstPort ^ srcPort;
        }

        public class EqualityComparer : IEqualityComparer<Quad>
        {

            public bool Equals(Quad x, Quad y)
            {
                return (x.srcIP == y.srcIP && x.srcPort == y.srcPort &&
                        x.dstIP == y.dstIP && x.dstPort == y.dstPort) ||
                        (x.srcIP == y.dstIP && x.srcPort == y.dstPort &&
                        x.dstIP == y.srcIP && x.dstPort == y.srcPort);
            }

            public int GetHashCode(Quad obj)
            {
                return obj.srcIP.GetHashCode() ^ obj.dstIP.GetHashCode() ^ obj.dstPort ^ obj.srcPort;
            }
        }

        public bool Equals(Quad other)
        {
            return (srcIP.Equals(other.srcIP) && srcPort == other.srcPort && dstIP.Equals(other.dstIP) && dstPort == other.dstPort);
        }
    }
}
