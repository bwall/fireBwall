using System;
using System.Collections.Generic;

namespace fireBwall.Utils
{
    // ip address object and it's relevant stuff
    public class IPObj
    {
        public IPAddr Address = new IPAddr();

        public IPObj() { }

        public SerializableList<int> Touched_Ports = new SerializableList<int>();

        public long last_access = long.MinValue;
        public long last_packet = long.MinValue;
        public float average_time = 0;

        public bool Reported = false;

        public IPObj(IPAddr addr)
        {
            this.Address = addr;
            time(DateTime.Now);
        }

        /// <summary>
        /// adds a port and access time
        /// </summary>
        /// <param name="p"></param>
        public void addPort(int p)
        {
            if (!Touched_Ports.Contains(p))
                Touched_Ports.Add(p);
            time(DateTime.Now);
        }

        /// <summary>
        /// Give me the datetime from one of my packets so I can use it to calculate my average!
        /// </summary>
        /// <param name="t"></param>
        private void time(DateTime t)
        {
            if (last_packet != long.MinValue)
            {
                long span = (t.Ticks) - last_packet;
                average_time = (average_time + span) / 2;
            }

            last_packet = t.Ticks;
            last_access = t.Ticks;
        }

        /// <summary>
        /// Returns a list of all the ports touched by this IP
        /// </summary>
        /// <returns></returns>
        public List<int> getTouchedPorts()
        {
            return Touched_Ports;
        }

        /// <summary>
        /// Returns the average packet time for this IPObj
        /// </summary>
        /// <returns></returns>
        public float getAverage()
        {
            return average_time / 10000000;
        }
    }
}
