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
        private VocaloidMotionData ReadFile(string filePath) =>
            !File.Exists(filePath)                          ? throw new FileNotFoundException() :
            Path.GetExtension(filePath).ToLower() != ".vmd" ? throw new InvalidDataException() :
                                                                    new VocaloidMotionData(filePath);


    }
}
