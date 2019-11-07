using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Tellurian.Trains.Models.Planning.Tests
{
    [TestClass]
    public class StationExitTests
    {
        [TestMethod]
        public void PropertiesAreSet()
        {
            TestDataFactory.Init();
            var station = TestDataFactory.Stations.First();
            var target = new StationExit("Vänster");
            station.Add(target);
            Assert.AreEqual(station, target.Station);
            Assert.AreEqual("Vänster", target.Name);
        }
    }
}
