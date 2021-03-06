﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoGambling.Framework
{
    public static class BeautifulConstants
    {
        public const string MachineName = "localhost";

        public const string CoreEndpoint = "InoGambling.Core";
        public const string YouTrackEndpoint = "InoGambling.YouTrack";
        public const string SlackEndpoint = "InoGambling.Slack";

        public const int TrackerSleepDelay = 3000;

        public const double StartingPoints = 50;
        public const double TimeWindowEstimationPercentage = .1d;

    }
}
