using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoGambling.Data.Model;

namespace InoGambling.YouTrack.Helpers
{
    public static class TicketStateHelper
    {
        public static TicketState ToTicketState(this string state)
        {
            switch (state.ToLower())
            {
                case "submitted":
                case "open":
                    return TicketState.ToDo;
                case "in progress":
                    return TicketState.InProgress;
                case "verified":
                    return TicketState.Verified;
                case "can't reproduce":
                case "duplicate":
                case "won't fix":
                case "obsolete":
                    return TicketState.Canceled;
                default:
                    return TicketState.OnHold;
            }
        }
    }
}
