using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Tellurian.Trains.Models.Planning.Tests
{
    [TestClass]
    public class StationTrackTests
    {
        private StationTrack Target;
        private Train Train1;
        private Train Train2;

        [TestInitialize]
        public void TestInitialize()
        {
            Target = TestDataFactory.CreateStationTrack();
            Train1 = new Train("1234") { Category = "Godståg" };
            Train2 = new Train("22") { Category = "Persontåg" };
        }

        [TestMethod]
        public void WhenNoCallsThenTimeslotIsFree()
        {
            Train1.Add(new StationCall(Target, new Time(12, 00), new Time(12, 30)));
            Assert.AreEqual(1, Target.Calls.Count);
            Assert.AreEqual(Train1.Calls[0], Target.Calls.First());
        }

        [TestMethod]
        public void WhenArrival1IsSameTimeAsDeparture2ThenNotConflict()
        {
            Train1.Add(new StationCall(Target, new Time(12, 00), new Time(12, 30)));
            Train2.Add(new StationCall(Target, new Time(12, 30), new Time(12, 45)));
            var validationErrors = Target.GetValidationErrors(null);
            Assert.AreEqual(0, validationErrors.Count());
            Assert.IsFalse(validationErrors.Any(ve => string.IsNullOrWhiteSpace(ve.Text)));
        }

        [TestMethod]
        public void WhenCallsNotOverlapsThenTimeslotIsFree()
        {
            Train1.Add(new StationCall(Target, new Time(12, 00), new Time(12, 30)));
            Train2.Add(new StationCall(Target, new Time(12, 31), new Time(12, 45)));
            Assert.AreEqual(2, Target.Calls.Count);
            var validationErrors = Target.GetValidationErrors(null);
            Assert.AreEqual(0, validationErrors.Count());
            Assert.IsFalse(validationErrors.Any(ve => string.IsNullOrWhiteSpace(ve.Text)));
        }
    }
}
