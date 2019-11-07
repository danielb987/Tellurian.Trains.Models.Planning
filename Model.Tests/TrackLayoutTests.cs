using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Tellurian.Trains.Models.Planning.Tests
{
    [TestClass]
    public class TrackLayoutTests
    {
         [TestMethod] public void LoadsTestLayout() {
            TestDataFactory.Init();
            ILayoutRepository repository = new TestRepository();
            var (item, _) = repository.GetLayout("test");
            var layout = item.Value;
            Assert.AreEqual(3, layout.Stations.Count);
            Assert.AreEqual(layout.Station("G").Value.Stretches.First().End.Station, layout.Station("Yb"));
        }

        [ExpectedException(typeof (TrackLayoutException))]
        [TestMethod] public void AddingSameStationTwiceThrows() {
            var target = new TrackLayout("Test");
            var station = TestDataFactory.CreateStation1();
            target.Add(station);
            target.Add(station);
        }
    }
}
