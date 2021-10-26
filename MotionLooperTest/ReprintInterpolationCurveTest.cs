using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods;
using MikuMikuMethods.VMD;
using MotionLooper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

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
            SameCountReprintTest(SourceFrames, 4, 6);
            SameCountReprintTest(SourceFrames, 1, 9);
            SameCountReprintTest(SourceFrames, 1, 11);
        }

        private void SameCountReprintTest(List<VmdMotionFrame> sourceFrames, params uint[] frames)
        {
            var targetFrames = frames.Select(f => new VmdMotionFrame("センター", f)).ToList();

            var reprinter = new FrameReprinter();
            reprinter.ReprintFromNearest(sourceFrames, targetFrames);

            foreach ((VmdMotionFrame Source, VmdMotionFrame Reprinted) frame in sourceFrames.Zip(targetFrames))
            {
                Assert.AreEqual(GeteEarly(frame.Source, InterpolationItem.XPosition).X, GeteEarly(frame.Reprinted, InterpolationItem.XPosition).X);
            }
        }

        [TestMethod]
        public void TestPutFromScore()
        {
            VocaloidMotionData source;
            VocaloidMotionData target;

            source = new();
            target = new();
            CreateFrames(source, 10, i => i, _ => new(0, 0, 0), i => i * 0.1f);
            CreateFrames(target, 2, i => i, i => new(i + 1), i => i * 0.5f);
            AssertIsPutFromScoreValid(source, target);

            source = new();
            target = new();
            CreateFrames(source, 12, i => i, _ => new(0, 0, 0), i => i / 12.0f);
            CreateFrames(target, 3, i => i * 2, i => new(i + 1), i => i / 3.0f);
            AssertIsPutFromScoreValid(source, target);

            static void CreateFrames(VocaloidMotionData vmd, int createCount, Func<uint, uint> frameSetter, Func<uint, Vector3> positionSetter, Func<uint, float> curveSetter)
            {
                for (uint i = 0; i < createCount; i++)
                {
                    var frame = new VmdMotionFrame("センター", frameSetter(i));
                    frame.Position = positionSetter(i);
                    foreach (var curve in frame.InterpolationCurves.Keys)
                    {
                        frame.InterpolationCurves[curve].EarlyControlePointFloat = (curveSetter(i), 0);
                        frame.InterpolationCurves[curve].LateControlePointFloat = (0, curveSetter(i));
                    }

                    vmd.AddFrame(frame);
                }
            }
        }

        private static void AssertIsPutFromScoreValid(VocaloidMotionData source, VocaloidMotionData target)
        {
            var reprinter = new FrameReprinter();
            var result = reprinter.PutFromScore(source, "センター", target);

            AreEqualFrameAndInterpolationCurvesBetweenSourceAndResult(source, result);
            AreEqualPositionBetweenTargetAndResult(target, result);

            static void AreEqualFrameAndInterpolationCurvesBetweenSourceAndResult(VocaloidMotionData source, VocaloidMotionData result)
            {
                foreach ((VmdMotionFrame Source, VmdMotionFrame Result) item in source.MotionFrames.Zip(result.MotionFrames))
                {
                    Assert.AreEqual(item.Source.Frame, item.Result.Frame);

                    foreach (var curve in item.Source.InterpolationCurves.Keys)
                    {
                        Assert.AreEqual(item.Source.InterpolationCurves[curve].EarlyControlePoint, item.Result.InterpolationCurves[curve].EarlyControlePoint);
                        Assert.AreEqual(item.Source.InterpolationCurves[curve].LateControlePoint, item.Result.InterpolationCurves[curve].LateControlePoint);
                    }
                }
            }

            static void AreEqualPositionBetweenTargetAndResult(VocaloidMotionData target, VocaloidMotionData result)
            {
                var q = new Queue<VmdMotionFrame>(result.MotionFrames);
                while (q.Any())
                {
                    foreach (var tFrame in target.MotionFrames)
                    {
                        if (!q.Any()) break;
                        var rFrame = q.Dequeue();
                        Assert.AreEqual(tFrame.Position, rFrame.Position);
                    }
                }
            }
        }
    }
}