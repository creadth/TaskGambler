
using System;
using System.Linq;
using InoGambling.Data.Model;
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

            try
            {
                var project = context.Set<Project>().FirstOrDefault();

            }
            catch (Exception e)
            {

                var tmp = e;
            }
            
        }
    }
}
