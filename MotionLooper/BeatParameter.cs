using System;

namespace MotionLooper
{
    public class BeatParameter
    {
        /// <summary>
        /// 周期
        /// </summary>
        public int Frequency { get; set; }
        /// <summary>
        /// 拍子
        /// </summary>
        public int Beat { get; set; }
        /// <summary>
        /// 設置拍数
        /// </summary>
        public int BeatCount { get; set; }
        /// <summary>
        /// 複製回数
        /// </summary>
        public int ElementCount => (int)Math.Ceiling(Beat * BeatCount / (decimal)Frequency);
    }
}
