using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoGambling.YouTrack;
using NServiceBus;

namespace InoGambling.Slack.Zapuskator
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new BusConfiguration();
            var configurator = new ConfigureEndpoint();
            configurator.Customize(config);
            var bus = Bus.Create(config).Start();
            Console.ReadLine();
        }
    }
}
