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

        public VocaloidMotionData PutFromScore(VocaloidMotionData source, string sourceItemName, VocaloidMotionData target)
        {
            var sourceFrames = source.Frames.Where(f => f.Name == sourceItemName).OrderBy(f => f.Frame);
            var targetItems = target.Frames.GroupBy(f => f.Name).ToArray();

            var sourceIter = sourceFrames.GetEnumerator();
            var targetIters = targetItems.Select(g => g.GetEnumerator()).ToList();

            var result = new VocaloidMotionData() { Header = target.Header, ModelName = target.ModelName };
            while (sourceIter.MoveNext())
            {
                for (int i = 0; i < targetIters.Count; i++)
                {
                    IEnumerator<IVmdFrame>? targetIter = targetIters[i];
                    // アイテムごとにフレームをクローンし現在のソースフレームの状態を反映して結果に追加する
                    if (!targetIter.MoveNext())
                    {
                        targetIters[i] = targetItems[i].GetEnumerator();
                        targetIter = targetIters[i];
                        targetIter.MoveNext();
                    }

                    var frame = (IVmdFrame)targetIter.Current.Clone();
                    frame.Frame = sourceIter.Current.Frame;

                    bool sourceIsInterpolatable = sourceIter.Current.GetType().GetInterfaces()?.Contains(typeof(IVmdInterpolatable)) ?? false;
                    bool targetIsInterpolatable = frame.GetType().GetInterfaces()?.Contains(typeof(IVmdInterpolatable)) ?? false;
                    if (sourceIsInterpolatable && targetIsInterpolatable)
                    {
                        foreach (var curve in ((IVmdInterpolatable)frame).InterpolationCurves.Keys)
                        {
                            ((IVmdInterpolatable)frame).InterpolationCurves[curve] = ((IVmdInterpolatable)sourceIter.Current).InterpolationCurves[curve];
                        }
                    }

                    switch (frame.FrameType)
                    {
                        case VmdFrameType.Camera:
                            result.CameraFrames.Add((VmdCameraFrame)frame);
                            break;
                        case VmdFrameType.Light:
                            result.LightFrames.Add((VmdLightFrame)frame);
                            break;
                        case VmdFrameType.Shadow:
                            result.ShadowFrames.Add((VmdShadowFrame)frame);
                            break;
                        case VmdFrameType.Property:
                            result.PropertyFrames.Add((VmdPropertyFrame)frame);
                            break;
                        case VmdFrameType.Morph:
                            result.MorphFrames.Add((VmdMorphFrame)frame);
                            break;
                        case VmdFrameType.Motion:
                            result.MotionFrames.Add((VmdMotionFrame)frame);
                            break;
                        default:
                            break;
                    }

                }
            }

            return result;
        }
    }
}
