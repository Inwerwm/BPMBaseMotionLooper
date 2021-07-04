using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionLooper
{
    public record LoopParameter
    {
        public int BPM { get; init; }
        public int BaseFrameRate { get; init; }
        public double Interval { get; init; }
    }
}
