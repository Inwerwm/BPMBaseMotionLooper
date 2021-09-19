using MikuMikuMethods.VMD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MotionLooper
{
    public class InterpolationCurveReprinter
    {
        public T[] Reprint<T>(IEnumerable<T> sourceFrames, IEnumerable<T> targetFrames) where T : IVmdInterpolatable, IVmdFrame =>
            targetFrames.Select(t =>
            {
                var target = (T)t.Clone();

                uint memo = uint.MaxValue;
                T? nearest = default;
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

                return target;
            }).OrderBy(f => f.Frame).ToArray();
    }
}
