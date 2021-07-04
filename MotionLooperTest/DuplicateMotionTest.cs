using Microsoft.VisualStudio.TestTools.UnitTesting;
using MotionLooper;
using MikuMikuMethods.VMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var loopParam = new LoopParameter(30) { Interval = 10m };
            Duplicator d = new();

            IEnumerable<IVocaloidFrame> duplicatedFrames = d.Duplicate(frames, loopParam.Interval);

            Assert.AreEqual((uint)10, duplicatedFrames.First().Frame);
        }
    }
}
