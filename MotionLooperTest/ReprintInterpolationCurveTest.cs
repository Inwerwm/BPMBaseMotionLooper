using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.VMD;
using System.Collections.Generic;

namespace MotionLooperTest
{
    [TestClass]
    public class ReprintInterpolationCurveTest
    {
        List<IVmdFrame> SourceFrames { get; } = new();

        [TestInitialize]
        public void CreateTestData()
        {
            SourceFrames.Clear();

            (uint Frame, float X, float Y) early = (0, 0.5f, 0.0f);
            (uint Frame, float X, float Y) late = (10, 0.0f, 0.5f);

            Add("センター");
            Add("上半身");
            Add("下半身");

            void Add(string boneName)
            {
                SourceFrames.Add(CreateFrame(boneName, early.Frame, early.X, early.Y));
                SourceFrames.Add(CreateFrame(boneName, late.Frame, late.X, late.Y));
            }

            IVmdFrame CreateFrame(string boneName, uint frame, float xPos, float yPos)
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

        [TestMethod]
        public void TestReprint()
        {

        }
    }
}