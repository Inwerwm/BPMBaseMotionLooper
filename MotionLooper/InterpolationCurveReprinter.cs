using MikuMikuMethods.VMD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MotionLooper
{
    public class InterpolationCurveReprinter
    {
        public void ReprintFromNearest<T>(IEnumerable<T> sourceFrames, List<T> targetFrames) where T : class, IVmdInterpolatable, IVmdFrame =>
            targetFrames.AsParallel().ForAll(target =>
            {
                uint memo = uint.MaxValue;
                T? nearest = null;
                foreach (var source in sourceFrames.OrderBy(f => f.Frame))
                {
                    var dif = source.Frame < target.Frame ? target.Frame - source.Frame : source.Frame - target.Frame;

                    if (memo < dif) break;
                    nearest = source;
                    memo = dif;
                }

                foreach (var curveType in target.InterpolationCurves.Keys)
                {
                    target.InterpolationCurves[curveType] = nearest?.InterpolationCurves[curveType] ?? new();
                }
            });
    }
}
