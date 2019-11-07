namespace Tellurian.Trains.Models.Planning.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;

    [TestClass]
    public class StationCallTests
    {
        private Train Train;

        [TestInitialize]
        public void TestInitialize()
        {
            Train = new Train("4321");
        }

        [TestMethod]
        public void TrackIsNullThrows()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new StationCall(null, new Time(12, 00), new Time(12, 00)));
        }

        [TestMethod]
        public void ArrivalIsNullThrows()
        {
            var track = TestDataFactory.CreateStationTrack();
            Assert.ThrowsException<ArgumentNullException>(() => new StationCall(track, null, new Time(12, 00)));
        }

        [TestMethod]
        public void DepartureIsNullThrows()
        {
            var track = TestDataFactory.CreateStationTrack();
            Assert.ThrowsException<ArgumentNullException>(() => new StationCall(track, new Time(12, 00), null));
        }

        [TestMethod]
        public void ArrivalAfterDepartureThrows()
        {
            var track = TestDataFactory.CreateStationTrack();
            var target = new StationCall(track, new Time(12, 00), new Time(11, 59));
            Train.Add(target);
            var validationErrors = target.GetValidationErrors();
            Assert.AreEqual(1, validationErrors.Count());
            Assert.IsFalse(validationErrors.Any(ve => string.IsNullOrWhiteSpace(ve.Text)));
        }

        [TestMethod]
        public void EqualsWorks()
        {
            TestDataFactory.Init();
            var station = TestDataFactory.CreateStation1();
            var train = TestDataFactory.CreateTrain1();
            var one = new StationCall(station.Tracks.First(), new Time(12, 00), new Time(12, 00)) { Train = train };
            var another = new StationCall(station.Tracks.First(), new Time(12, 00), new Time(12, 00)) { Train = train };
            Assert.AreEqual(one, another);
        }
    }
}
