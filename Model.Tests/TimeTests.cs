using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tellurian.Trains.Models.Planning.Tests
{

    [TestClass]
    public class TimeTests {

        [TestMethod]
        public void ParsesDouble() {
            var actual = Time.Parse("0.5");
            Assert.AreEqual("12:00", actual.ToString());
        }
    }
}
