using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionLooper
{
    public record LoopParameter
    {
        public decimal BPM { get; init; }
        public decimal BaseFrameRate { get; init; }
        public decimal Interval => BaseFrameRate / (BPM / 60);
    }
}
