using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.VMD;
using MotionLooper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MotionLooperTest
{
    [TestClass]
    public class DuplicateMotionTest
    {
        [TestMethod]
        public void TestDuplicateFrames()
        {
            IEnumerable<IVocaloidFrame> frames = new List<IVocaloidFrame>
            {
                new VocaloidMotionFrame("test", 0)
            };
            var calculator = new IntervalCalculator(30) { Interval = 10m };
            FrameDuplicator d = new();

            IEnumerable<IVocaloidFrame> duplicatedFrames = d.Duplicate(frames, (uint)calculator.Interval);

            Assert.AreEqual((uint)0, frames.First().Frame);
            Assert.AreEqual((uint)10, duplicatedFrames.First().Frame);
        }

        [TestMethod]
        public void TestMultipleDuplicate()
        {
            IEnumerable<IVocaloidFrame> frames = new List<IVocaloidFrame>
            {
                new VocaloidMotionFrame("test", 0)
            };
            var calculator = new IntervalCalculator(30) { Interval = 3.3m };
            FrameDuplicator d = new();

            IEnumerable<IVocaloidFrame> duplicatedFrames = d.Duplicate(frames, calculator.Interval ?? 3.3m, 6);
            uint r(decimal value) => (uint)Math.Round(value);

            Assert.AreEqual((uint)0, frames.First().Frame);
            Assert.AreEqual(r(3.3m * 0), duplicatedFrames.ElementAt(0).Frame);
            Assert.AreEqual(r(3.3m * 1), duplicatedFrames.ElementAt(1).Frame);
            Assert.AreEqual(r(3.3m * 2), duplicatedFrames.ElementAt(2).Frame);
            Assert.AreEqual(r(3.3m * 3), duplicatedFrames.ElementAt(3).Frame);
            Assert.AreEqual(r(3.3m * 4), duplicatedFrames.ElementAt(4).Frame);
            Assert.AreEqual(r(3.3m * 5), duplicatedFrames.ElementAt(5).Frame);
        }

        [TestMethod]
        public void TestCreateLoopMotion()
        {
            VocaloidMotionData vmd = new() { ModelName = "Test" };
            vmd.MotionFrames.Add(new("test", 0));
            vmd.MorphFrames.Add(new("mp", 1));

            IntervalCalculator calculator = new(30) { Interval = 10m };
            DuplicationCounter counter = new() { Beat = 4, LoopCount = 4, Frequency = 2 };
            FrameDuplicator duplicator = new();

            VocaloidMotionData loopMotion = duplicator.CreateLoopMotion(vmd, calculator, counter);

            Assert.AreEqual(2, vmd.Frames.Count());
            Assert.AreEqual(counter.ElementCount * 2, loopMotion.Frames.Count());

            Assert.AreEqual((uint)0, loopMotion.MotionFrames.ElementAt(0).Frame);
            Assert.AreEqual((uint)20, loopMotion.MotionFrames.ElementAt(1).Frame);
            Assert.AreEqual((uint)1, loopMotion.MorphFrames.ElementAt(0).Frame);
            Assert.AreEqual((uint)21, loopMotion.MorphFrames.ElementAt(1).Frame);
        }
    }
}
