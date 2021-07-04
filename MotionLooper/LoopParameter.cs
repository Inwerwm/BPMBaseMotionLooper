using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionLooper
{
    public class LoopParameter
    {
        public decimal BaseFrameRate { get; init; }
        
        public decimal BPM { get; set; }
        public decimal Interval { get; set; }

        public LoopParameter(decimal baseFrameRate)
        {
            BaseFrameRate = baseFrameRate;
        }
    }
}
