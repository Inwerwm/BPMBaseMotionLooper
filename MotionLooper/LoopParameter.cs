using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionLooper
{
    public class LoopParameter
    {
        private decimal? bpm;
        private decimal? interval;

        public decimal BaseFrameRate { get; init; }

        public decimal? BPM
        {
            get => bpm;
            set
            {
                decimal? val = value.HasValue && value != 0m ? value : null;

                bpm = val;
                var beetPerSecond = val / 60;
                interval = 30 / beetPerSecond;
            }
        }
        public decimal? Interval
        {
            get => interval;
            set
            {
                interval = value;
                var bps = 30 / value;
                bpm = bps * 60;
            }
        }

        public LoopParameter(decimal baseFrameRate)
        {
            BaseFrameRate = baseFrameRate;
        }
    }
}
