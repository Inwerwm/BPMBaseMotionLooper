using MikuMikuMethods.VMD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MotionLooper
{
    public class InterpolationCurveReprinter
    {
        public T[] Reprint<T>(IEnumerable<T> sourceFrames, IEnumerable<T> targetFrames) where T : IVmdInterpolatable, IVmdFrame =>
            targetFrames.Select(target =>
            {
                target = (T)target.Clone();
                var nearestSource = FindNearest(sourceFrames.OrderBy(f => f.Frame), target, (s, t) => (sbyte)(s.Frame > t.Frame ? -1 : s.Frame < t.Frame ? 1 : 0));
                foreach (var curveType in target.InterpolationCurves.Keys.ToArray())
                {
                    target.InterpolationCurves[curveType] = nearestSource.InterpolationCurves[curveType];
                }

                return target;
            }).ToArray();

        private T FindNearest<T>(IOrderedEnumerable<T> source, T query, Func<T, T, sbyte> comparer)
        {
            T[] array = source.ToArray();
            int mid = array.Length / 2;
            sbyte r;

            return RecursiveFind(array, query, comparer) ?? array[mid];

            T? RecursiveFind(T[] array, T q, Func<T, T, sbyte> cmp)
            {
                if (array.Length < 1)
                    return default;

                mid = array.Length / 2;
                r = cmp(array[mid], q);

                return r < 0 ? RecursiveFind(array.Take(mid).ToArray(), q, cmp) :
                       r > 0 ? RecursiveFind(array.Skip(mid + 1).ToArray(), q, cmp) :
                       array[mid];
            }
        }
    }
}
