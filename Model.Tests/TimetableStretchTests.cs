using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tellurian.Trains.Models.Planning.Tests {

    [TestClass]
    public class TimetableStretchTests {

        private TimetableStretch Target { get; set; }

        [TestInitialize]
        public void TestInitialize() {
            TestDataFactory.Init();
            Target = new TimetableStretch("10", "Ten");
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void NullNumberThrows() {
            Target = new TimetableStretch(null);
        }
 
        [TestMethod]
        public void PropertiesAreSet() {
            Assert.AreEqual("10", Target.Number);
            Assert.AreEqual("Ten", Target.Description);
        }

        [TestMethod]
        public void EqualsWithSameNumber() {
            var other = new TimetableStretch("10");
            Assert.AreEqual(Target, other);
        }

        [TestMethod]
        public void DistanceToFirstStationIsZero()
        {
            Target.Add(CreateTrackStretch());
            Assert.AreEqual(0.0, Target.DistanceToStation(Target.Stations.First()));
        }
        [TestMethod]
        public void DistanceToSecondStationIsOverZero()
        {
            Target.Add(CreateTrackStretch());
            Assert.AreEqual(5.0, Target.DistanceToStation(Target.Stations.Last()));
        }

        private static TrackStretch CreateTrackStretch() {
            var exit1 = TestDataFactory.Stations.First().Exits.First();
            var exit2 = TestDataFactory.Stations.Last().Exits.Last();
            return new TrackStretch(exit1, exit2, 5.0, 1);
        }
    }
}
