using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.VMD;
using MotionLooper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionLooperTest
{
    [TestClass]
    public class MotionLoopTest
    {
        [TestMethod]
        public void TestMotionLoop()
        {
            VocaloidMotionData vmd = new() { ModelName = "Test" };
            vmd.MotionFrames.Add(new("test", 0));
            vmd.MorphFrames.Add(new("mp", 1));
            
            LoopParameter loop = new(30) { Interval = 10m };
            BeatParameter beat = new() { Beat = 4, BeatCount = 4, Frequency = 2 };
            FrameDuplicator duplicator = new();

            VocaloidMotionData loopMotion = duplicator.CreateLoopMotion(vmd, loop, beat);

            Assert.AreEqual(2, vmd.Frames.Count());
            Assert.AreEqual(beat.ElementCount * 2, loopMotion.Frames.Count());

            Assert.AreEqual((uint)0, loopMotion.MotionFrames.ElementAt(0).Frame);
            Assert.AreEqual((uint)20, loopMotion.MotionFrames.ElementAt(1).Frame);
            Assert.AreEqual((uint)1, loopMotion.MorphFrames.ElementAt(0).Frame);
            Assert.AreEqual((uint)21, loopMotion.MorphFrames.ElementAt(1).Frame);
        }
    }
}
