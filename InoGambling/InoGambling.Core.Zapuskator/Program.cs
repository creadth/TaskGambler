using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace InoGambling.Core.Zapuskator
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                var config = new BusConfiguration();
                var configurator = new ConfigureEndpoint();
                configurator.Customize(config);
                var bus = Bus.Create(config).Start();
                Console.ReadLine();
            }
        }
    }
}
