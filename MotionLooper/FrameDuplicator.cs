using MikuMikuMethods.VMD;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
