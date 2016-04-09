using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoGambling.Data.Model;
using NServiceBus;

namespace InoGambling.CommonMessages.Commands.Integrations
{
    /// <summary>
    /// Used by integration to request min time to filter tasks
    /// </summary>
    public class SyncTimeCommand
        : ICommand
    {
        public IntegrationType Integration { get; set; }
        public DateTime SyncTime { get; set; }
    }
}
