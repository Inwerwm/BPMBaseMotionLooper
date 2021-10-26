using MikuMikuMethods.VMD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MotionLooper
{
    public class FrameReprinter
    {
        /// <summary>
        /// ターゲットフレームの補間曲線をソースの最近傍フレームから転写する
        /// <para>破壊的</para>
        /// </summary>
        /// <typeparam name="T">フレームの型</typeparam>
        /// <param name="sourceFrames">転写元のフレーム</param>
        /// <param name="targetFrames">転写先のフレーム</param>
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

        /// <summary>
        /// ソースVMDのフレーム位置の場所にターゲットフレームを設置したVMDを返す
        /// <para>ターゲットのフレーム数がソースより少なければループする</para>
        /// <para>補間曲線のあるフレームの場合はそれも転写する</para>
        /// </summary>
        /// <param name="source">位置と補間曲線の基VMD</param>
        /// <param name="sourceItemName">ソース側の位置と補間曲線を取得する項目名</param>
        /// <param name="target">複製するフレームの入ったVMD</param>
        /// <returns></returns>
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
                    // アイテムごとにフレームをクローンし現在のソースフレームの状態を反映して結果に追加する
                    IEnumerator<IVmdFrame>? targetIter = targetIters[i];

                    // ターゲットのイテレータを動かし、末尾まで来ていれば初期位置に戻る
                    if (!targetIter.MoveNext())
                    {
                        targetIters[i] = targetItems[i].GetEnumerator();
                        targetIter = targetIters[i];
                        targetIter.MoveNext();
                    }

                    var frame = (IVmdFrame)targetIter.Current.Clone();

                    // フレーム位置を転写
                    frame.Frame = sourceIter.Current.Frame;

                    // 補間曲線を持つフレームであればそれも転写する
                    bool sourceIsInterpolatable = sourceIter.Current.GetType().GetInterfaces()?.Contains(typeof(IVmdInterpolatable)) ?? false;
                    bool targetIsInterpolatable = frame.GetType().GetInterfaces()?.Contains(typeof(IVmdInterpolatable)) ?? false;
                    if (sourceIsInterpolatable && targetIsInterpolatable)
                    {
                        foreach (var curve in ((IVmdInterpolatable)frame).InterpolationCurves.Keys)
                        {
                            ((IVmdInterpolatable)frame).InterpolationCurves[curve] = ((IVmdInterpolatable)sourceIter.Current).InterpolationCurves[curve];
                        }
                    }

                    result.AddFrame(frame);
                }
            }

            return result;
        }
    }
}
