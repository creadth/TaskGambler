using System.Collections.Generic;
using InoGambling.Data.Model;
using NServiceBus;
using Ticket = InoGambling.YouTrack.Models.Ticket;

namespace InoGambling.CommonMessages.Commands.Integrations.YouTrack
{
    /// <summary>
    /// Used by integration. Contains tasks received by filter for specific integration
    /// </summary>
    public class TaskFilterBatchCommand
        : ICommand
    {
        public IntegrationType Integration { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
