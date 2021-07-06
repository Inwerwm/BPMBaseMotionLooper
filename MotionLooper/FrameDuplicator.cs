using MikuMikuMethods.VMD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MotionLooper
{
    public class FrameDuplicator
    {
        public IEnumerable<IVocaloidFrame> Duplicate(IEnumerable<IVocaloidFrame> frames, uint offset) =>
            frames.Select(frame =>
            {
                var cloneFrame = frame.Clone() as IVocaloidFrame;
                if (cloneFrame != null)
                    cloneFrame.Frame += offset;
                return cloneFrame;
            }).Where(frame => frame != null).Cast<IVocaloidFrame>();

        public IEnumerable<IVocaloidFrame> Duplicate(IEnumerable<IVocaloidFrame> frames, decimal interval, int count) =>
            Enumerable.Range(0, count).SelectMany(i => Duplicate(frames, (uint)Math.Round(interval * i)));

        public VocaloidMotionData CreateLoopMotion(VocaloidMotionData vmd, IntervalCalculator loop, DuplicationCounter counter)
        {
            if (loop.Interval is null)
                throw new ArgumentNullException("設置間隔が未設定です。");

            var result = new VocaloidMotionData()
            {
                Header = vmd.Header,
                ModelName = vmd.ModelName,
            };

            // nullならここまで到達しない
            var interval = loop.Interval.Value * counter.Frequency;
            var count = counter.ElementCount;

            void CreateAndAddDuplicate<T>(List<T> source, List<T> result) where T : IVocaloidFrame
            {
                if (source.Any())
                    result.AddRange(Duplicate(source.Select(f => (IVocaloidFrame)f), interval, count).Select(f => (T)f));
            }

            CreateAndAddDuplicate(vmd.CameraFrames, result.CameraFrames);
            CreateAndAddDuplicate(vmd.LightFrames, result.LightFrames);
            CreateAndAddDuplicate(vmd.MorphFrames, result.MorphFrames);
            CreateAndAddDuplicate(vmd.MotionFrames, result.MotionFrames);
            CreateAndAddDuplicate(vmd.PropertyFrames, result.PropertyFrames);
            CreateAndAddDuplicate(vmd.ShadowFrames, result.ShadowFrames);

            return result;
        }
    }
}
