using System; 

namespace fireBwall.Logging
{
    public class DebugLogMessage
    {
        DateTime when;
        string message;

        public DebugLogMessage(string message)
        {
            when = DateTime.Now;
            this.message = message;
        }

        public override string ToString()
        {
            return "[" + when.TimeOfDay.ToString() + "] " + message;
        }
    }
}
