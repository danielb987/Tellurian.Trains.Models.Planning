using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Tellurian.Trains.Models.Planning.Tests
{
    [TestClass]
    public class TrainPartTests
    {

        public Train Train { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            TestDataFactory.Init();
            Train = TestDataFactory.CreateTrain1();
        }

        [TestMethod]
        public void NullTrainThrows()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new TrainPart(null, 0, 1));
        }

        [TestMethod]
        public void NegativeStartIndexThrows()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new TrainPart(Train, -1, 1));
        }

        [TestMethod]
        public void FromIndexIsLastIndexThrows()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new TrainPart(Train, Train.Calls.Count() - 1, 1));
        }

        [TestMethod]
        public void ToIndexEqualToStartIndexThrows()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new TrainPart(Train, 1, 1));
        }

        [TestMethod]
        public void ToIndexIsGreaterThanLastThrows()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new TrainPart(Train, 1, 3));
        }

        [TestMethod]
        public void FromAndToStationsAreSet()
        {
            var target = new TrainPart(Train, 1, 2);
            Assert.AreEqual(target.From.Station.Name, "Ytterby");
            Assert.AreEqual(target.To.Station.Name, "Stenungsund");
        }

        [TestMethod]
        public void EqualWorks()
        {
            var one = new TrainPart(Train, 1, 2);
            var another = new TrainPart(Train, 1, 2);
            Assert.AreEqual(one, another);
        }
    }
}
