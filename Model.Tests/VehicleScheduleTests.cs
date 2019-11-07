using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Tellurian.Trains.Models.Planning.Tests
{
    [TestClass]
    public class VehicleScheduleTests
    {
        private VehicleSchedule Target { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            Target = VehicleSchedule.Loco("W1");
        }

        [TestMethod]
        public void ConstructorSetsProperties()
        {
            Assert.AreEqual("W1", Target.Identity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddsNullTrainPartThrows()
        {
            Target.Add(null);
        }

        [TestMethod]
        public void AddsTrainPart()
        {
            TestDataFactory.Init();
            var train = TestDataFactory.CreateTrains("Persontåg", new Time(12, 00)).First();
            var part = new TrainPart(train, 0, 1);
            Target.Add(part);
            Assert.AreEqual(part, Target.Parts.First());
        }
    }
}
