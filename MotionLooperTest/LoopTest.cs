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
            LoopParameter loop = new(120, 30);
            Assert.AreEqual(15m, loop.Interval);
        }

        public void TestCalcBPMFromInterval()
        {
            LoopParameter loop = new(30) { Interval = 15m };
            Assert.AreEqual(120m, loop.BPM);
        }
    }
}
