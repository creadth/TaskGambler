using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace InoGambling.Framework.Intergations.Trackers
{
    public interface IYouTrackIntegration
        : IBaseTrackerIntegration
    {
        void Work(ref DateTime syncTime);

        string ValidateUserLogin(string userLogin);
    }
}
