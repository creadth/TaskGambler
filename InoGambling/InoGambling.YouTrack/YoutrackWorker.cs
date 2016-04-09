using System;
using System.Linq;
using InoGambling.CommonMessages.Commands;
using InoGambling.CommonMessages.Commands.Integrations.YouTrack;
using InoGambling.Data.Model;
using InoGambling.Framework.Intergations.Trackers;
using NServiceBus;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Issues;
using C = InoGambling.Framework.BeautifulConstants;
using Ticket = InoGambling.YouTrack.Models.Ticket;

namespace InoGambling.YouTrack
{
    public class YoutrackWorker
        : IYouTrackIntegration
    {

        private const string YouTrackUrl = "luxoria";
        private const int YouTrackPort = 8080;
        private const string BotLogin = "JustABot";
        private const string BotPwd = "botpassword";

        private IBus _bus;
        private Address _coreAddress;

        public YoutrackWorker(IBus bus)
        {
            _bus = bus;
            _coreAddress = new Address(C.CoreEndpoint, C.MachineName);
        }

        /// <summary>
        /// Sync data from youtrack. Returns next min time required to build a good filter
        /// </summary>
        /// <returns></returns>
        public void Work(ref DateTime syncTime)
        {
#if DEBUG
            Console.WriteLine("Youtrack sync tick...");
#endif
            try
            {
                //Auth
                var con = new Connection(YouTrackUrl, YouTrackPort);
                con.Authenticate(BotLogin, BotPwd);
                var issueMan = new IssueManagement(con);
                var youtrackIssues = issueMan
                    .GetIssuesBySearch(BuildFilter(syncTime, syncTime.AddDays(1)))
                    .ToList();
                var tickets = issueMan
                    .GetIssuesBySearch(BuildFilter(syncTime, syncTime.AddDays(1)))
                    .Select(x =>
                    {
                        dynamic t = x; 
                        return new Ticket
                        {
                            State = t.State,
                            AssigneeName = t.AssigneeName,
                            ShortId = t.Id,
                            ProjectShortId = t.ProjectShortName,
                            UpdatedTime = ParseYouTrackTime(t.Updated)
                        };
                    }).ToList();
                _bus.Send(_coreAddress, new TaskFilterBatchCommand
                {
                    Integration = IntegrationType.Youtrack,
                    Tickets = tickets
                });
                syncTime = tickets.Max(x => x.UpdatedTime);
            }
            catch (Exception e)
            {
                //TODO: log like boss
#if DEBUG
                Console.WriteLine("Ooops. Youtrack worker is done:  " + e.Message);
#endif
            }
        }

        public void Dispose()
        {
            //todo: dispose something. Like.. really. Dispose something.
        }


        protected string BuildFilter(DateTime from, DateTime to)
        {
            var format = "yyyy-MM-dd\\Thh:mm:ss";
            var filter = $"updated: {from.ToString(format)} .. {to.ToString(format)}";
#if DEBUG
            Console.WriteLine($"Requesting filter {filter}");
#endif
            return filter;
        }

        public DateTime ParseYouTrackTime(long ticks)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(ticks).ToLocalTime();
        }


    }
}
