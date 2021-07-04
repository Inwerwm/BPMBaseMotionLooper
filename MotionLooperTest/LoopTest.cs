using Microsoft.VisualStudio.TestTools.UnitTesting;
using MotionLooper;

namespace MotionLooperTest
{
    [TestClass]
    public class LoopTest
    {
        [TestMethod]
        public void TestCalcBPM()
        {
            LoopParameter loop = new() { BPM = 120, BaseFrameRate = 30 };
            Assert.AreEqual(15d, loop.Interval);
        }
    }
}
