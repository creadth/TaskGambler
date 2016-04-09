using SlackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoGambling.Slack
{
    class Bot
    {
        static void Main()
        {            
            SlackSocketClient client = new SlackSocketClient("xoxb-33255991907-GIsd17eRb8bMgLuVMrm4Uv8H");
            client.Connect((connected) => {
                //This is called once the client has emitted the RTM start command               

            }, () => {
                //This is called once the RTM client has connected to the end point
                client.PostMessage(null, "#general", "Party Hard", "Bot", null, false, null, false,
                   null, null, true);
            });
        }
    }
}
