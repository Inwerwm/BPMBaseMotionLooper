using Microsoft.Win32;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

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

        public ReactiveProperty<bool> IsDuplicationCountVaild { get; }
        public ReactiveProperty<string> Log { get; }

        public ReactiveCommand OpenFile { get; }
        public ReactiveCommand ExecuteGeneration { get; }
        private Action<string> AppendLog { get; }

        private Model Model { get; }

        public ViewModel(Action<string> logAppender)
        {
            FilePath = new ReactiveProperty<string>().AddTo(Disposable);
            Interval = new ReactiveProperty<decimal?>(15).AddTo(Disposable);
            BPM = new ReactiveProperty<decimal?>(0).AddTo(Disposable);
            Frequency = new ReactiveProperty<int>(1).AddTo(Disposable);
            Beat = new ReactiveProperty<int>(1).AddTo(Disposable);
            LoopNum = new ReactiveProperty<int>(1).AddTo(Disposable);
            EnableDecrement = new ReactiveProperty<bool>().AddTo(Disposable);
            ElementNum = new ReactiveProperty<int>().AddTo(Disposable);

            IsDuplicationCountVaild = new ReactiveProperty<bool>().AddTo(Disposable);
            Log = new ReactiveProperty<string>().AddTo(Disposable);

            OpenFile = new ReactiveCommand();
            ExecuteGeneration = IsDuplicationCountVaild.ToReactiveCommand();

            AppendLog = logAppender;

            Model = new();
            SetSubscribes();
        }

        private void SetSubscribes()
        {
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
                    FilePath.Value = ofd.FileName;
                }
            });
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
