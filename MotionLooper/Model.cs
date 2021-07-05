using MikuMikuMethods.VMD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionLooper
{
    public class Model
    {
        public BeatParameter BeatParams { get; }
        public LoopParameter LoopParams { get; }
        public FrameDuplicator FrameDuplicator { get; }

        public Model()
        {
            BeatParams = new() { Frequency = 1 };
            LoopParams = new(30);
            FrameDuplicator = new();
        }

        private VocaloidMotionData ReadFile(string filePath) =>
            !File.Exists(filePath)                          ? throw new FileNotFoundException() :
            Path.GetExtension(filePath).ToLower() != ".vmd" ? throw new InvalidDataException() :
                                                                    new VocaloidMotionData(filePath);

        private VocaloidMotionData CreateLoopMotion(VocaloidMotionData vmd) =>
            FrameDuplicator.CreateLoopMotion(vmd, LoopParams, BeatParams);

        public void GenerateLoopMotion(string filePath)
        {
            var sourceVMD = ReadFile(filePath);
            var savePath = Path.Combine(Path.GetDirectoryName(filePath) ?? "", Path.GetFileNameWithoutExtension(filePath) + "_loop.vmd");
            CreateLoopMotion(sourceVMD).Write(savePath);
        }
    }
}
