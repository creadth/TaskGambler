using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace InoGambling.CommonMessages.Commands
{
    /// <summary>
    /// I am a simple command.
    /// </summary>
    public class SimpleCommand : ICommand
    {
        public object SomeShit { get; set; }
    }
}
