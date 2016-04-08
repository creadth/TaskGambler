
using System.Linq;
using InoGambling.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InoGambling.Data.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var context = new InoGamblingDbContext();

            var project = context.Set<Project>().FirstOrDefault();
        }
    }
}
