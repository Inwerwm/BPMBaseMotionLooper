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

        public ReactiveProperty<string?> FilePath { get; }
        public ReactiveProperty<string?> ReprintSourceFilePath { get; }
        public ReactiveProperty<decimal?> Interval { get; }
        public ReactiveProperty<decimal?> BPM { get; }
        public ReactiveProperty<int> Frequency { get; }
        public ReactiveProperty<int> Beat { get; }
        public ReactiveProperty<int> LoopNum { get; }
        public ReactiveProperty<bool> EnableDecrement { get; }
        public ReactiveProperty<int> ElementNum { get; }
        public ReactiveProperty<object> SelectedPuttingBaseItem { get; }

        public ReactiveProperty<bool> IsFileLoaded { get; }
        public ReactiveProperty<bool> IsReprintSourceFileLoaded { get; }
        public ReactiveProperty<bool> IsDuplicationCountVaild { get; }
        public ReactiveProperty<bool> IsPuttingBaseItemSelected { get; }
        public ReactiveProperty<string> Log { get; }

        public ReactiveCollection<string> ReprintSourceItemNames { get; }

        public ReactiveCommand OpenFile { get; }
        public ReactiveCommand OpenReprintSourceFile { get; }
        public ReactiveCommand LoadReprintSourceItemNames { get; }
        public ReactiveCommand ExecuteGeneration { get; }
        public ReactiveCommand ExecuteMorphReprinting { get; }
        public ReactiveCommand ExecutePutFromScore { get; }
        private Action<string> AppendLog { get; }

        private Model Model { get; }

        public ViewModel(Action<string> logAppender)
        {
            FilePath = new ReactiveProperty<string?>(Properties.Settings.Default.FilePath).AddTo(Disposable);
            ReprintSourceFilePath = new ReactiveProperty<string?>(Properties.Settings.Default.ReprintSourceFilePath).AddTo(Disposable);
            Interval = new ReactiveProperty<decimal?>(Properties.Settings.Default.Interval).AddTo(Disposable);
            BPM = new ReactiveProperty<decimal?>(Properties.Settings.Default.BPM).AddTo(Disposable);
            Frequency = new ReactiveProperty<int>(Properties.Settings.Default.Frequency).AddTo(Disposable);
            Beat = new ReactiveProperty<int>(Properties.Settings.Default.Beat).AddTo(Disposable);
            LoopNum = new ReactiveProperty<int>(Properties.Settings.Default.LoopNum).AddTo(Disposable);
            EnableDecrement = new ReactiveProperty<bool>(Properties.Settings.Default.Decrement).AddTo(Disposable);
            ElementNum = new ReactiveProperty<int>().AddTo(Disposable);
            SelectedPuttingBaseItem = new ReactiveProperty<object>().AddTo(Disposable);

            ReprintSourceItemNames = new ReactiveCollection<string>().AddTo(Disposable);

            IsFileLoaded = new ReactiveProperty<bool>().AddTo(Disposable);
            IsReprintSourceFileLoaded = new ReactiveProperty<bool>().AddTo(Disposable);
            IsDuplicationCountVaild = new ReactiveProperty<bool>().AddTo(Disposable);
            IsPuttingBaseItemSelected = new ReactiveProperty<bool>().AddTo(Disposable);

            Log = new ReactiveProperty<string>().AddTo(Disposable);

            OpenFile = new ReactiveCommand();
            OpenReprintSourceFile = new ReactiveCommand();
            LoadReprintSourceItemNames = new ReactiveCommand();
            ExecuteGeneration = new[] { IsDuplicationCountVaild, IsFileLoaded }.CombineLatestValuesAreAllTrue().ToReactiveCommand();
            ExecuteMorphReprinting = new[] { IsFileLoaded, IsReprintSourceFileLoaded }.CombineLatestValuesAreAllTrue().ToReactiveCommand();
            ExecutePutFromScore = new[] { IsFileLoaded, IsReprintSourceFileLoaded, IsPuttingBaseItemSelected }.CombineLatestValuesAreAllTrue().ToReactiveCommand();

            AppendLog = logAppender;

            Model = new();
            SetSubscribes();
        }

        private void SetSubscribes()
        {
            FilePath.Subscribe(path => IsFileLoaded.Value = !string.IsNullOrEmpty(path));
            ReprintSourceFilePath.Subscribe(path => IsReprintSourceFileLoaded.Value = !string.IsNullOrEmpty(path));

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

            SelectedPuttingBaseItem.Subscribe(item => IsPuttingBaseItemSelected.Value = !(item is null));

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

            Action<ReactiveProperty<string?>> showOpenFileDialog = filePathProperty =>
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
                    ReadFile(ofd.FileName, filePathProperty);
                }
            };

            OpenFile.Subscribe(_ => showOpenFileDialog(FilePath));
            OpenReprintSourceFile.Subscribe(_ => { showOpenFileDialog(ReprintSourceFilePath); LoadReprintSourceItemNames.Execute(); });
            LoadReprintSourceItemNames.Subscribe(_ =>
            {
                try
                {
                    var reprintSourcePath = ReprintSourceFilePath.Value ?? "";
                    var reprintSourceVmd = Model.ReadFile(reprintSourcePath);

                    ReprintSourceItemNames.Clear();
                    ReprintSourceItemNames.AddRangeOnScheduler(Model.ExtractIncludedItemNames(reprintSourceVmd));
                }
                catch (Exception)
                {
                    
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
            ExecuteMorphReprinting.Subscribe(_ =>
            {
                try
                {
                    var reprintTargetPath = FilePath.Value ?? "";
                    var reprintTargetVMD = Model.ReadFile(reprintTargetPath);

                    var reprintSourcePath = ReprintSourceFilePath.Value ?? "";
                    var reprintSourceVmd = Model.ReadFile(reprintSourcePath);

                    var savePath = Path.Combine(Path.GetDirectoryName(reprintTargetPath) ?? "", Path.GetFileNameWithoutExtension(reprintTargetPath) + "_morphReprenmted.vmd");

                    Model.ReprintMorph(reprintSourceVmd, reprintTargetVMD);
                    reprintTargetVMD.Write(savePath);

                    AppendLog("出力が完了しました。");
                    AppendLog($"{Path.GetFileName(reprintTargetPath)} の各フレームに {Path.GetFileName(reprintSourcePath)} 内の近傍フレームのモーフを転写しました");
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
            ExecutePutFromScore.Subscribe(_ =>
            {
                try
                {
                    var reprintTargetPath = FilePath.Value ?? "";
                    var reprintTargetVMD = Model.ReadFile(reprintTargetPath);

                    var reprintSourcePath = ReprintSourceFilePath.Value ?? "";
                    var reprintSourceVmd = Model.ReadFile(reprintSourcePath);

                    var reprintSourceItemName = SelectedPuttingBaseItem.Value as string ?? string.Empty;

                    var savePath = Path.Combine(Path.GetDirectoryName(reprintTargetPath) ?? "", Path.GetFileNameWithoutExtension(reprintTargetPath) + "_followingPut.vmd");

                    Model.FollowPut(reprintSourceVmd, reprintSourceItemName, reprintTargetVMD).Write(savePath);

                    AppendLog("出力が完了しました。");
                    AppendLog($"{Path.GetFileName(reprintTargetPath)} の各フレームを {Path.GetFileName(reprintSourcePath)} 内の {reprintSourceItemName} と同じフレーム位置に設置しました");
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
            ReprintSourceFilePath.Subscribe(path => Properties.Settings.Default.ReprintSourceFilePath = path);
            Interval.Subscribe(iv => Properties.Settings.Default.Interval = iv ?? 1);
            BPM.Subscribe(bpm => Properties.Settings.Default.BPM = bpm ?? 1);
            Frequency.Subscribe(freq => Properties.Settings.Default.Frequency = freq);
            Beat.Subscribe(beat => Properties.Settings.Default.Beat = beat);
            LoopNum.Subscribe(l => Properties.Settings.Default.LoopNum = l);
            EnableDecrement.Subscribe(d => Properties.Settings.Default.Decrement = d);
        }

        public void ReadFile(string fileName, ReactiveProperty<string?> filePathProperty)
        {
            try
            {
                var vmd = Model.ReadFile(fileName);
                filePathProperty.Value = fileName;
                AppendLog($"入力フレーム数 : {vmd.Frames.Count()}");
            }
            catch (FileNotFoundException)
            {
                AppendLog("ファイルが見つかりませんでした。");
                filePathProperty.Value = null;
            }
            catch (InvalidDataException)
            {
                AppendLog("非VMDファイルが指定されました。");
                filePathProperty.Value = null;
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
