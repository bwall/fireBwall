using System;
using System.Text;
using System.Diagnostics;

namespace fireBwall.Utils
{
    /// <summary>
    /// Class to manage an adapters total transfered data
    /// </summary>
    public class BandwidthCounter
    {
        /// <summary>
        /// Class to manage an adapters current transfer rate
        /// </summary>
        class MiniCounter
        {
            public uint bits = 0;
            public uint kbits = 0;
            public uint mbits = 0;
            public uint gbits = 0;
            public uint tbits = 0;
            public uint pbits = 0;
            DateTime lastRead = DateTime.UtcNow;
            readonly Stopwatch watch = Stopwatch.StartNew();

            /// <summary>
            /// Adds bits
            /// </summary>
            /// <param name="count">The number of bits to add</param>
            public void AddBits(uint count)
            {
                bits += count;

                kbits += bits / 1024;
                bits = bits % 1024;

                mbits += kbits / 1024;
                kbits = kbits % 1024;

                gbits += mbits / 1024;
                mbits = mbits % 1024;

                tbits += gbits / 1024;
                gbits = gbits % 1024;

                pbits += tbits / 1024;
                tbits = tbits % 1024;
            }

            /// <summary>
            /// Returns the bits per second since the last time this function was called
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                double millis = watch.ElapsedMilliseconds;
                watch.Restart();
                if (pbits > 0)
                {
                    double ret = (double)pbits + ((double)((double)tbits / 1024f));
                    ret = 1000f * ret / (millis);
                    lastRead = DateTime.UtcNow;
                    string s = ret.ToString();
                    if (s.Length > 6)
                        s = s.Substring(0, 6);
                    return s + " Pb";
                }
                else if (tbits > 0)
                {
                    double ret = (double)tbits + ((double)((double)gbits / 1024f));
                    ret = 1000f * ret / (millis);
                    lastRead = DateTime.UtcNow;
                    string s = ret.ToString();
                    if (s.Length > 6)
                        s = s.Substring(0, 6);
                    return s + " Tb";
                }
                else if (gbits > 0)
                {
                    double ret = (double)gbits + ((double)((double)mbits / 1024f));
                    ret = 1000f * ret / (millis);
                    lastRead = DateTime.UtcNow;
                    string s = ret.ToString();
                    if (s.Length > 6)
                        s = s.Substring(0, 6);
                    return s + " Gb";
                }
                else if (mbits > 0)
                {
                    double ret = (double)mbits + ((double)((double)kbits / 1024f));
                    ret = 1000f * ret / (millis);
                    lastRead = DateTime.UtcNow;
                    string s = ret.ToString();
                    if (s.Length > 6)
                        s = s.Substring(0, 6);
                    return s + " Mb";
                }
                else if (kbits > 0)
                {
                    double ret = (double)kbits + ((double)((double)bits / 1024f));
                    ret = 1000f * ret / (millis);
                    lastRead = DateTime.UtcNow;
                    string s = ret.ToString();
                    if (s.Length > 6)
                        s = s.Substring(0, 6);
                    return s + " Kb";
                }
                else
                {
                    double ret = bits;
                    ret = 1000f * ret / (millis);
                    lastRead = DateTime.UtcNow;
                    string s = ret.ToString();
                    if (s.Length > 6)
                        s = s.Substring(0, 6);
                    return s + " b";
                }
            }
        }

        private uint bits = 0;
        private uint kbits = 0;
        private uint mbits = 0;
        private uint gbits = 0;
        private uint tbits = 0;
        private uint pbits = 0;
        MiniCounter perSecond = new MiniCounter();

        /// <summary>
        /// Empty constructor, because thats constructive
        /// </summary>
        public BandwidthCounter()
        {

        }

        /// <summary>
        /// Accesses the current transfer rate, returning the text
        /// </summary>
        /// <returns></returns>
        public string GetPerSecond()
        {
            lock (this)
            {
                string s = perSecond.ToString() + "/s";
                perSecond = new MiniCounter();
                return s;
            }
        }

        public void AddBytes(uint count)
        {
            AddBits(count * 8);
        }

        /// <summary>
        /// Adds bits to the total transfered
        /// </summary>
        /// <param name="count">Byte count</param>
        public void AddBits(uint count)
        {
            lock (this)
            {
                perSecond.AddBits(count);
            }
            bits += count;
            kbits += bits / 1024;
            bits = bits % 1024;

            mbits += kbits / 1024;
            kbits = kbits % 1024;

            gbits += mbits / 1024;
            mbits = mbits % 1024;

            tbits += gbits / 1024;
            gbits = gbits % 1024;

            pbits += tbits / 1024;
            tbits = tbits % 1024;
        }

        /// <summary>
        /// Prints out a relevant string for the bits transfered
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (pbits > 0)
            {
                double ret = (double)pbits + ((double)((double)tbits / 1024));
                string s = ret.ToString();
                if (s.Length > 6)
                    s = s.Substring(0, 6);
                return s + " Pb";
            }
            else if (tbits > 0)
            {
                double ret = (double)tbits + ((double)((double)gbits / 1024));
                string s = ret.ToString();
                if (s.Length > 6)
                    s = s.Substring(0, 6);
                return s + " Tb";
            }
            else if (gbits > 0)
            {
                double ret = (double)gbits + ((double)((double)mbits / 1024));
                string s = ret.ToString();
                if (s.Length > 6)
                    s = s.Substring(0, 6);
                return s + " Gb";
            }
            else if (mbits > 0)
            {
                double ret = (double)mbits + ((double)((double)kbits / 1024));
                string s = ret.ToString();
                if (s.Length > 6)
                    s = s.Substring(0, 6);
                return s + " Mb";
            }
            else if (kbits > 0)
            {
                double ret = (double)kbits + ((double)((double)bits / 1024));
                string s = ret.ToString();
                if (s.Length > 6)
                    s = s.Substring(0, 6);
                return s + " Kb";
            }
            else
            {
                string s = bits.ToString();
                if (s.Length > 6)
                    s = s.Substring(0, 6);
                return s + " b";
            }
        }
    }
}
