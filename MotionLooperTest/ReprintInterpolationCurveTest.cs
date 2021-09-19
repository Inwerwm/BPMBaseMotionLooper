using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods;
using MikuMikuMethods.VMD;
using MotionLooper;
using System.Collections.Generic;
using System.Linq;

namespace MotionLooperTest
{
    [TestClass]
    public class ReprintInterpolationCurveTest
    {
        List<VmdMotionFrame> SourceFrames { get; } = new();
        (byte X, byte Y) GeteEarly(IVmdInterpolatable frame, InterpolationItem ipItem) => frame.InterpolationCurves[ipItem].EarlyControlePoint;
        (byte X, byte Y) GeteLate(IVmdInterpolatable frame, InterpolationItem ipItem) => frame.InterpolationCurves[ipItem].LateControlePoint;


        [TestInitialize]
        public void CreateTestData()
        {
            SourceFrames.Clear();

            (uint Frame, float X, float Y) early = (0, 0.5f, 0.0f);
            (uint Frame, float X, float Y) late = (10, 0.0f, 0.5f);

            Add("センター");

            // 指定ボーン名のフレームを生成して補間曲線を設定し SourceFrames に追加する
            void Add(string boneName)
            {
                SourceFrames.Add(CreateFrame(boneName, early.Frame, early.X, early.Y));
                SourceFrames.Add(CreateFrame(boneName, late.Frame, late.X, late.Y));

                VmdMotionFrame CreateFrame(string boneName, uint frame, float xPos, float yPos)
                {
                    var key = new VmdMotionFrame(boneName, frame);
                    foreach (var curve in key.InterpolationCurves)
                    {
                        curve.Value.EarlyControlePointFloat = (xPos, yPos);
                        curve.Value.LateControlePointFloat = (1.0f - xPos, 1.0f - yPos);
                    }
                    return key;
                }
            }
        }

        [TestMethod]
        public void TestSameFrameReprint()
        {
            SameCountReprintTest(SourceFrames, 0, 10);
        }

        [TestMethod]
        public void TestNotSameFrameReprint()
        {
            SameCountReprintTest(SourceFrames, 1, 9);
        }

        private void SameCountReprintTest(List<VmdMotionFrame> sourceFrames, params uint[] frames)
        {
            var targetFrames = frames.Select(f => new VmdMotionFrame("センター", f));

            var reprinter = new InterpolationCurveReprinter();
            var reprinted = reprinter.Reprint(sourceFrames, targetFrames);

            Assert.AreEqual(targetFrames.Count(), reprinted.Count());
            foreach ((VmdMotionFrame Source, VmdMotionFrame Reprinted) frame in sourceFrames.Zip(reprinted))
            {
                Assert.AreEqual(GeteEarly(frame.Source, InterpolationItem.XPosition).X, GeteEarly(frame.Reprinted, InterpolationItem.XPosition).X);
            }
        }
    }
}