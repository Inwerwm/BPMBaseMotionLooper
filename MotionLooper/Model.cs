using MikuMikuMethods.VMD;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MotionLooper
{
    public class Model
    {
        public DuplicationCounter DuplicationCounter { get; }
        public IntervalCalculator IntervalCalculator { get; }
        public FrameDuplicator FrameDuplicator { get; }
        public FrameReprinter FrameReprinter { get; }

        public Model()
        {
            DuplicationCounter = new();
            IntervalCalculator = new(30);
            FrameDuplicator = new();
            FrameReprinter = new();
        }

        public VocaloidMotionData ReadFile(string filePath) =>
            !File.Exists(filePath) ? throw new FileNotFoundException() :
            Path.GetExtension(filePath).ToLower() != ".vmd" ? throw new InvalidDataException() :
                                                                    new VocaloidMotionData(filePath);

        public VocaloidMotionData CreateLoopMotion(VocaloidMotionData vmd) =>
            FrameDuplicator.CreateLoopMotion(vmd, IntervalCalculator, DuplicationCounter);

        public void ReprintMorph(VocaloidMotionData source, VocaloidMotionData target)
        {
            if (source.CameraFrames.Any() && target.CameraFrames.Any())
                FrameReprinter.ReprintFromNearest(source.CameraFrames, target.CameraFrames);
            if (source.MotionFrames.Any() && target.MotionFrames.Any())
                FrameReprinter.ReprintFromNearest(source.MotionFrames, target.MotionFrames);
        }

        public VocaloidMotionData FollowPut(VocaloidMotionData source, string sourceItemName, VocaloidMotionData target) =>
            FrameReprinter.PutFromScore(source, sourceItemName, target);
    }
}
