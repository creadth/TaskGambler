using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Policy;
using InoGambling.CommonMessages.Commands;
using InoGambling.CommonMessages.Commands.Integrations.YouTrack;
using InoGambling.Data.Model;
using InoGambling.Framework.Intergations.Trackers;
using InoGambling.YouTrack.Helpers;
using NServiceBus;
using YouTrackSharp.Admin;
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
        private Connection _con;


        public YoutrackWorker(IBus bus)
        {
            _bus = bus;
            _coreAddress = new Address(C.CoreEndpoint, C.MachineName);
            AuthYoutrack();

        }


        /// <summary>
        /// Search trhough youtrack to find specified user logijn
        /// </summary>
        /// <param name="userLogin">User login to find</param>
        /// <returns>User display name</returns>
        public string ValidateUserLogin(string userLogin)
        {
#if DEBUG
            Console.WriteLine("Validating user login....");
#endif
            var userService = new UserManagement(_con);
            var user = userService.GetUserByUserName(userLogin);
            return user?.FullName;
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
                var issueMan = new IssueManagement(_con);
                var milis = ToYouTrackTime(syncTime);
                var tickets = issueMan
                    .GetIssuesBySearch(BuildFilter(syncTime, syncTime.AddDays(1)))
                    .Where(x =>
                    {
                        dynamic t = x;
                        return t.Updated > milis;
                    })
                    .Select(x =>
                    {
                        dynamic t = x;
                        int estimation = 0;
                        var arr = (object[])t.Estimation;
                        if (arr.Length > 0)
                        {
                            int.TryParse((string)arr[0], out estimation);
                        }

                        return new Ticket
                        {
                            State = ((string)t.State).ToTicketState(),
                            AssigneeName = t.AssigneeName,
                            UpdaterName = t.UpdaterName,
                            //TODO: should we ignore ticket with estimation set not by assignee?
                            ShortId = t.Id,
                            Estimation = estimation,
                            CreateTime = ParseYouTrackTime(t.Created),
                            ProjectShortId = t.ProjectShortName,
                            UpdatedTime = ParseYouTrackTime(t.Updated),
                            Summary = t.Summary,
                            Link = BuildTicketUrl(t.Id)
                        };
                    }).ToList();
                if (tickets.Count == 0) return;
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


        /// <summary>
        /// Authorize yourself to use youtrack.
        /// </summary>
        protected void AuthYoutrack()
        {
            while (true)
            {
                try
                {
                    _con = new Connection(YouTrackUrl, YouTrackPort);
                    _con.Authenticate(BotLogin, BotPwd);
                    return;
                }
                catch (Exception)
                {
                }
                
            }
            
        }

        protected string BuildTicketUrl(string ticketShortId)
        {
            var port = YouTrackPort == 80 ? string.Empty : $":{YouTrackPort}";
            return $"http://{YouTrackUrl}{port}/issue/{ticketShortId}";
        }

        public void Dispose()
        {
            //todo: dispose something. Like.. really. Dispose something.
        }


        protected string BuildFilter(DateTime from, DateTime to)
        {
            var format = "yyyy-MM-dd\\THH:mm:ss";
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

        public long ToYouTrackTime(DateTime dt)
        {
            return (long)Math.Ceiling(dt.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalMilliseconds);
        }


    }
}
