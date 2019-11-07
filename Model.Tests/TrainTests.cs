using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Tellurian.Trains.Models.Planning.Tests
{
    [TestClass]
    public class TrainTests
    {
        private Train Target;

        [TestInitialize]
        public void TestInitialize()
        {
            Target = new Train("G1234") { Category = "Godståg" };
        }

        [TestMethod]
        public void PropertiesAreSet()
        {
            Assert.AreEqual("Godståg", Target.Category);
            Assert.AreEqual("G1234", Target.Number);
            Assert.AreEqual("", Target.ExtenalId);
        }

        [TestMethod]
        public void AddsFirstTimetableCall()
        {
            var station = TestDataFactory.CreateStation1();
            var call = new StationCall(station.Tracks.First(), new Time(12, 30), new Time(12, 45));
            Target.Add(call);
            Assert.IsFalse(Target.CheckTrainTimeSequence().Any());
        }

        [TestMethod]
        public void WhenSecondTimetableCallIsBeforeLastThenValidationErrors()
        {
            var station = TestDataFactory.CreateStation1();
            Target.Add(new StationCall(station.Tracks.First(), new Time(12, 30), new Time(12, 45)));
            Target.Add(new StationCall(station.Tracks.First(), new Time(12, 30), new Time(12, 45)));
            var validationErrors = Target.GetValidationErrors(new ValidationOptions());
            Assert.AreEqual(1, validationErrors.Count());
            Assert.IsFalse(validationErrors.Any(ve => string.IsNullOrWhiteSpace(ve.Text)));
        }

        [TestMethod]
        public void WhenSecondTimetableCallIsAfterLastThenThrows()
        {
            var station = TestDataFactory.CreateStation1();
            var call1 = new StationCall(station.Tracks.First(), new Time(12, 30), new Time(12, 45));
            var call2 = new StationCall(station.Tracks.First(), new Time(12, 50), new Time(12, 55));
            Target.Add(call1);
            Target.Add(call2);
        }
    }
}
