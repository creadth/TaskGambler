using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace InoGambling.Framework.Intergations.Messengers
{
    public interface IBaseMessengerIntegration
        : IBaseIntegration
    {
        /// <summary>
        /// Sends specified message to specified recepient
        /// </summary>
        /// <param name="message"></param>
        /// <param name="recepient"></param>
        bool SendMessage(string message, string recepient);

        bool SendBroadcast(string message);
    }
}
