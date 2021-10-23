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
            var targetFrames = frames.Select(f => new VmdMotionFrame("センター", f)).ToList();

            var reprinter = new InterpolationCurveReprinter();
            reprinter.ReprintFromNearest(sourceFrames, targetFrames);

            foreach ((VmdMotionFrame Source, VmdMotionFrame Reprinted) frame in sourceFrames.Zip(targetFrames))
            {
                Assert.AreEqual(GeteEarly(frame.Source, InterpolationItem.XPosition).X, GeteEarly(frame.Reprinted, InterpolationItem.XPosition).X);
            }
        }

        [TestMethod]
        public void TestPutFromScore()
        {
            var source = new VocaloidMotionData();
            var target = new VocaloidMotionData();

            for (uint i = 0; i < 10; i++)
            {
                var frame = new VmdMotionFrame("センター", i);
                foreach (var curve in frame.InterpolationCurves.Keys)
                {
                    frame.InterpolationCurves[curve].EarlyControlePointFloat = (i * 0.1f, 0);
                    frame.InterpolationCurves[curve].LateControlePointFloat = (0, i * 0.1f);
                }

                source.AddFrame(frame);
            }

            target.AddFrame(new VmdMotionFrame("ボーン", 0) { Position = new(1, 1, 1) });
            target.AddFrame(new VmdMotionFrame("ボーン", 1) { Position = new(2, 2, 2) });

            var reprinter = new InterpolationCurveReprinter();
            var result = reprinter.PutFromScore(source, "センター", target);
            var r = result.MotionFrames;

            foreach ((VmdMotionFrame Source, VmdMotionFrame Result) item in source.MotionFrames.Zip(r))
            {
                Assert.AreEqual(item.Source.Frame, item.Result.Frame);
                foreach (var curve in item.Source.InterpolationCurves.Keys)
                {
                    Assert.AreEqual(item.Source.InterpolationCurves[curve].EarlyControlePoint, item.Result.InterpolationCurves[curve].EarlyControlePoint);
                    Assert.AreEqual(item.Source.InterpolationCurves[curve].LateControlePoint, item.Result.InterpolationCurves[curve].LateControlePoint);
                }
            }
        }
    }
}