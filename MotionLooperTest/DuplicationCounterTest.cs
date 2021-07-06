using Microsoft.VisualStudio.TestTools.UnitTesting;
using MotionLooper;

namespace MotionLooperTest
{
    [TestClass]
    public class DuplicationCounterTest
    {
        [TestMethod]
        public void TestCalcElementCount()
        {
            DuplicationCounter counter = new()
            {
                Frequency = 2,
                Beat = 4,
                LoopCount = 9
            };
            Assert.AreEqual(18, counter.ElementCount);

            counter = new()
            {
                Frequency = 6,
                Beat = 4,
                LoopCount = 5
            };
            Assert.AreEqual(4, counter.ElementCount);
        }

        [TestMethod]
        public void TestDecrement()
        {
            DuplicationCounter counter = new()
            {
                Frequency = 2,
                Beat = 4,
                LoopCount = 9,
                Decrement = true
            };
            Assert.AreEqual(17, counter.ElementCount);
        }
    }
}
