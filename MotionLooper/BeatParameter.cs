using System;

namespace MotionLooper
{
    public class BeatParameter
    {
        public int Frequency { get; set; }
        public int Beat { get; set; }
        public int BeatCount { get; set; }
        public int ElementCount => (int)Math.Ceiling(Beat * BeatCount / (decimal)Frequency);
    }
}
