using Microsoft.Win32;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;

namespace MotionLooper
{
    public class ViewModel : INotifyPropertyChanged, IDisposable
    {
        private bool disposedValue;

        public event PropertyChangedEventHandler? PropertyChanged;
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        bool ignoreChange = false;

        public ReactiveProperty<string> FilePath { get; }
        public ReactiveProperty<decimal?> Interval { get; }
        public ReactiveProperty<decimal?> BPM { get; }
        public ReactiveProperty<int> Frequency { get; }
        public ReactiveProperty<int> Beat { get; }
        public ReactiveProperty<int> LoopNum { get; }
        public ReactiveProperty<bool> EnableDecrement { get; }
        public ReactiveProperty<int> ElementNum { get; }

        public ReactiveProperty<bool> IsFileSpecified { get; }
        public ReactiveProperty<bool> IsDuplicationCountVaild { get; }
        public ReactiveProperty<string> Log { get; }

        public ReactiveCommand OpenFile { get; }
        public ReactiveCommand ExecuteGeneration { get; }
        private Action<string> AppendLog { get; }

        private Model Model { get; }

        public ViewModel(Action<string> logAppender)
        {
            FilePath = new ReactiveProperty<string>(Properties.Settings.Default.FilePath).AddTo(Disposable);
            Interval = new ReactiveProperty<decimal?>(Properties.Settings.Default.Interval).AddTo(Disposable);
            BPM = new ReactiveProperty<decimal?>(Properties.Settings.Default.BPM).AddTo(Disposable);
            Frequency = new ReactiveProperty<int>(Properties.Settings.Default.Frequency).AddTo(Disposable);
            Beat = new ReactiveProperty<int>(Properties.Settings.Default.Beat).AddTo(Disposable);
            LoopNum = new ReactiveProperty<int>(Properties.Settings.Default.LoopNum).AddTo(Disposable);
            EnableDecrement = new ReactiveProperty<bool>(Properties.Settings.Default.Decrement).AddTo(Disposable);
            ElementNum = new ReactiveProperty<int>().AddTo(Disposable);

            IsFileSpecified = new ReactiveProperty<bool>().AddTo(Disposable);
            IsDuplicationCountVaild = new ReactiveProperty<bool>().AddTo(Disposable);
            Log = new ReactiveProperty<string>().AddTo(Disposable);

            OpenFile = new ReactiveCommand();
            ExecuteGeneration = new[] { IsDuplicationCountVaild, IsFileSpecified }.CombineLatestValuesAreAllTrue().ToReactiveCommand();

            AppendLog = logAppender;

            Model = new();
            SetSubscribes();
        }

        private void SetSubscribes()
        {
            FilePath.Subscribe(path => IsFileSpecified.Value = !string.IsNullOrEmpty(path));

            Interval.Subscribe(interval =>
            {
                if (ignoreChange) return;

                Model.IntervalCalculator.Interval = interval;

                ignoreChange = true;
                BPM.Value = Model.IntervalCalculator.BPM;
                ignoreChange = false;
            });
            BPM.Subscribe(bpm =>
            {
                if (ignoreChange) return;

                Model.IntervalCalculator.BPM = bpm;

                ignoreChange = true;
                Interval.Value = Model.IntervalCalculator.Interval;
                ignoreChange = false;
            });

            Action<bool> UpdateLoopParam = isInterval =>
            {
                if (isInterval) BPM.Value = Model.IntervalCalculator.BPM;
                else Interval.Value = Model.IntervalCalculator.Interval;
            };

            Frequency.Subscribe(freq => Model.DuplicationCounter.Frequency = freq);
            Beat.Subscribe(beat => Model.DuplicationCounter.Beat = beat);
            LoopNum.Subscribe(lnum => Model.DuplicationCounter.LoopCount = lnum);
            EnableDecrement.Subscribe(dec => Model.DuplicationCounter.Decrement = dec);

            Action<int> UpdateElemNum = _ =>
            {
                ElementNum.Value = Model.DuplicationCounter.ElementCount;
                IsDuplicationCountVaild.Value = ElementNum.Value > 0;
            };
            Frequency.Subscribe(UpdateElemNum);
            Beat.Subscribe(UpdateElemNum);
            LoopNum.Subscribe(UpdateElemNum);
            EnableDecrement.Subscribe(_ => UpdateElemNum(0));

            OpenFile.Subscribe(_ =>
            {
                var ofd = new OpenFileDialog()
                {
                    Filter = "VMDファイル(*.vmd)|*.vmd",
                    AddExtension = true,
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Multiselect = false,
                };

                if (ofd.ShowDialog() ?? false)
                {
                    string fileName = ofd.FileName;
                    ReadFile(fileName);
                }
            });

            ExecuteGeneration.Subscribe(_ =>
            {
                try
                {
                    var filePath = FilePath.Value ?? "";
                    var sourceVMD = Model.ReadFile(filePath);
                    var savePath = Path.Combine(Path.GetDirectoryName(filePath) ?? "", Path.GetFileNameWithoutExtension(filePath) + "_loop.vmd");
                    var loopMotion = Model.CreateLoopMotion(sourceVMD);
                    loopMotion.Write(savePath);
                    AppendLog($"出力が完了しました。");
                    AppendLog($"フレーム数 : {sourceVMD.Frames.Count()} → {loopMotion.Frames.Count()}");
                    AppendLog($"保存先 : {savePath}");
                    AppendLog(Environment.NewLine);
                }
                catch (FileNotFoundException)
                {
                    AppendLog("ファイルが見つかりませんでした。");
                    FilePath.Value = null;
                }
                catch (InvalidDataException)
                {
                    AppendLog("非VMDファイルが指定されました。");
                    FilePath.Value = null;
                }
                catch (Exception ex)
                {
                    AppendLog($"エラーが発生しました。{ex.Message}");
                }
            });

            FilePath.Subscribe(path => Properties.Settings.Default.FilePath = path);
            Interval.Subscribe(iv => Properties.Settings.Default.Interval = iv ?? 1);
            BPM.Subscribe(bpm => Properties.Settings.Default.BPM = bpm ?? 1);
            Frequency.Subscribe(freq => Properties.Settings.Default.Frequency = freq);
            Beat.Subscribe(beat => Properties.Settings.Default.Beat = beat);
            LoopNum.Subscribe(l => Properties.Settings.Default.LoopNum = l);
            EnableDecrement.Subscribe(d => Properties.Settings.Default.Decrement = d);
        }

        public void ReadFile(string fileName)
        {
            try
            {
                var vmd = Model.ReadFile(fileName);
                FilePath.Value = fileName;
                AppendLog($"入力フレーム数 : {vmd.Frames.Count()}");
            }
            catch (FileNotFoundException)
            {
                AppendLog("ファイルが見つかりませんでした。");
                FilePath.Value = null;
            }
            catch (InvalidDataException)
            {
                AppendLog("非VMDファイルが指定されました。");
                FilePath.Value = null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Disposable.Dispose();
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
