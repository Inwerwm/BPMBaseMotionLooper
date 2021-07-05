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
            !File.Exists(filePath)                          ? throw new FileNotFoundException() :
            Path.GetExtension(filePath).ToLower() != ".vmd" ? throw new InvalidDataException() :
                                                                    new VocaloidMotionData(filePath);

        private VocaloidMotionData CreateLoopMotion(VocaloidMotionData vmd) =>
            FrameDuplicator.CreateLoopMotion(vmd, IntervalCalculator, DuplicationCounter);

        public void GenerateLoopMotion(string filePath)
        {
            try
            {
                var sourceVMD = ReadFile(filePath);
                var savePath = Path.Combine(Path.GetDirectoryName(filePath) ?? "", Path.GetFileNameWithoutExtension(filePath) + "_loop.vmd");
                CreateLoopMotion(sourceVMD).Write(savePath);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
