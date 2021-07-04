using Microsoft.VisualStudio.TestTools.UnitTesting;
using MotionLooper;

namespace MotionLooperTest
{
    [TestClass]
    public class BeatParameterTest
    {
        [TestMethod]
        public void TestCalcElementCount()
        {
            BeatParameter beat = new()
            {
                Frequency = 2,
                Beat = 4,
                BeatCount = 9
            };
            Assert.AreEqual(18, beat.ElementCount);

            beat = new()
            {
                Frequency = 6,
                Beat = 4,
                BeatCount = 5
            };
            Assert.AreEqual(4, beat.ElementCount);
        }
    }
}
