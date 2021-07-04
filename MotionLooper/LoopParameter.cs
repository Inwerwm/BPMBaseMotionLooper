using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionLooper
{
    public class LoopParameter
    {
        private decimal bpm;
        private decimal interval;

        public decimal BaseFrameRate { get; init; }

        public decimal BPM
        {
            get => bpm;
            set
            {
                bpm = value;
                Interval = 30 / (value / 60);
            }
        }
        public decimal Interval
        {
            get => interval;
            set
            {
                interval = value;
            }
        }

        public LoopParameter(decimal baseFrameRate)
        {
            BaseFrameRate = baseFrameRate;
        }
    }
}
