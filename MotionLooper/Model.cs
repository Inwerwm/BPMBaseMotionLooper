﻿using MikuMikuMethods.VMD;
using System.IO;

namespace MotionLooper
{
    public class Model
    {
        public DuplicationCounter DuplicationCounter { get; }
        public IntervalCalculator IntervalCalculator { get; }
        public FrameDuplicator FrameDuplicator { get; }

        public Model()
        {
            DuplicationCounter = new();
            IntervalCalculator = new(30);
            FrameDuplicator = new();
        }

        public VocaloidMotionData ReadFile(string filePath) =>
            !File.Exists(filePath) ? throw new FileNotFoundException() :
            Path.GetExtension(filePath).ToLower() != ".vmd" ? throw new InvalidDataException() :
                                                                    new VocaloidMotionData(filePath);

        public VocaloidMotionData CreateLoopMotion(VocaloidMotionData vmd) =>
            FrameDuplicator.CreateLoopMotion(vmd, IntervalCalculator, DuplicationCounter);
    }
}
