using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoGambling.Slack.Helpers
{
    public static class ExtensionHelper
    {
        public static string ToSlackMention(this string userId)
        {
            return $"<@{userId}>";
        }
    }
}
