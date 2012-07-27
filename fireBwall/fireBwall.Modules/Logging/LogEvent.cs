using System;
using System.Text;
using fireBwall.UI;
using fireBwall.Modules;

namespace fireBwall.Logging
{
    public class LogEvent
    {
        public PacketMainReturnType PMR;
        public NDISModule Module;
        public string Message;
        public DateTime time;

        public LogEvent(string message, NDISModule module)
        {
            Module = module;
            Message = message;
            time = DateTime.Now;
        }

        public override string ToString()
        {
            return time.TimeOfDay.Hours.ToString("D2") + ":" + time.TimeOfDay.Minutes.ToString("D2") + ":" + time.TimeOfDay.Seconds.ToString("D2") + " " + Module.MetaData.GetMeta().Name + " " + Message;
        }
    }
}
