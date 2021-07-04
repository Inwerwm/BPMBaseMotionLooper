using Microsoft.VisualStudio.TestTools.UnitTesting;
using MotionLooper;

namespace MotionLooperTest
{
    [TestClass]
    public class LoopTest
    {
        [TestMethod]
        public void TestCalcIntervalFromBPM()
        {
            LoopParameter loop = new(30m) { BPM = 120m };
            Assert.AreEqual(15m, loop.Interval);
        }

        [TestMethod]
        public void TestCalcBPMFromInterval()
        {
            LoopParameter loop = new(30m) { Interval = 15m };
            Assert.AreEqual(120m, loop.BPM);
        }
    }
}
