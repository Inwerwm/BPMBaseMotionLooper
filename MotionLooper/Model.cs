using MikuMikuMethods;
using MikuMikuMethods.VMD;
using System;
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

        private IEnumerable<string>? ExtractFrameNames(IEnumerable<IVmdFrame> frames) => frames.Any() ? frames.GroupBy(f => f.Name).Select(g => g.Key) : null;

        public IEnumerable<string> ExtractIncludedItemNames(VocaloidMotionData vmd) => vmd.Type switch
        {
            VMDType.Camera => ExtractFrameNames(vmd.Frames) ?? Array.Empty<string>(),
            VMDType.Model =>
                (
                    ExtractFrameNames(vmd.MorphFrames) ?? Array.Empty<string>()
                ).Concat(
                    ExtractFrameNames(vmd.MotionFrames)?.OrderBy(name => name, new BoneNameComparer()) as IEnumerable<string> ?? Array.Empty<string>()
                ),
            _ => throw new NotImplementedException(),
        };
    }
}
