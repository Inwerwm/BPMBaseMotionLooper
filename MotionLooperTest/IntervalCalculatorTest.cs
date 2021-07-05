using Microsoft.VisualStudio.TestTools.UnitTesting;
using MotionLooper;

namespace MotionLooperTest
{
    [TestClass]
    public class IntervalCalculatorTest
    {
        [TestMethod]
        public void TestCalcIntervalFromBPM()
        {
            IntervalCalculator calculator = new(30m) { BPM = 120m };
            Assert.AreEqual(15m, calculator.Interval);
        }

        [TestMethod]
        public void TestCalcBPMFromInterval()
        {
            IntervalCalculator calculator = new(30m) { Interval = 15m };
            Assert.AreEqual(120m, calculator.BPM);
        }

        [TestMethod]
        public void TestBPMSetToZero()
        {
            IntervalCalculator calculator = new(30m) { BPM = 0m };
            Assert.IsNull(calculator.BPM);
            Assert.IsNull(calculator.Interval);
        }

        [TestMethod]
        public void TestIntervalSetToZero()
        {
            IntervalCalculator calculator = new(30m) { Interval = 0m };
            Assert.IsNull(calculator.BPM);
            Assert.IsNull(calculator.Interval);
        }

        [TestMethod]
        public void TestBPMSetToNegative()
        {
            IntervalCalculator calculator = new(30m) { BPM = -120m };
            Assert.IsNull(calculator.BPM);
            Assert.IsNull(calculator.Interval);
        }

        [TestMethod]
        public void TestIntervalSetToNegative()
        {
            IntervalCalculator calculator = new(30m) { Interval = -15m };
            Assert.IsNull(calculator.BPM);
            Assert.IsNull(calculator.Interval);
        }
    }
}
