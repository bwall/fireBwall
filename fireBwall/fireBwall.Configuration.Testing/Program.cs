using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace fireBwall.Configuration.Testing
{
    class Program
    {
        public static void Setup()
        {
            ConfigurationManagement.Instance.ConfigurationPath = "temp";
        }

        [STAThread]
        static void Main(string[] args)
        {
            string[] my_args = { Assembly.GetExecutingAssembly().Location };

            int returnCode = NUnit.ConsoleRunner.Runner.Main(my_args);

            if (returnCode != 0)
                Console.Beep();
        }
    }
}
