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

        private Model Model { get; }

        public ViewModel()
        {
            FilePath = new ReactiveProperty<string>().AddTo(Disposable);
            Interval = new ReactiveProperty<decimal?>(15).AddTo(Disposable);
            BPM = new ReactiveProperty<decimal?>(0).AddTo(Disposable);
            Frequency = new ReactiveProperty<int>(1).AddTo(Disposable);
            Beat = new ReactiveProperty<int>(1).AddTo(Disposable);
            LoopNum = new ReactiveProperty<int>(1).AddTo(Disposable);
            EnableDecrement = new ReactiveProperty<bool>().AddTo(Disposable);
            ElementNum = new ReactiveProperty<int>().AddTo(Disposable);

            Model = new();
            SetSubscribes();
        }

        private void SetSubscribes()
        {
            Interval.Subscribe(interval =>
            {
                if (ignoreChange) return;

                Model.LoopParams.Interval = interval;
                
                ignoreChange = true;
                BPM.Value = Model.LoopParams.BPM;
                ignoreChange = false;
            });
            BPM.Subscribe(bpm =>
            {
                if (ignoreChange) return;

                Model.LoopParams.BPM = bpm;

                ignoreChange = true;
                Interval.Value = Model.LoopParams.Interval;
                ignoreChange = false;
            });

            Action<bool> UpdateLoopParam = isInterval =>
            {
                if (isInterval) BPM.Value = Model.LoopParams.BPM;
                else Interval.Value = Model.LoopParams.Interval;
            };
            
            Frequency.Subscribe(freq => Model.BeatParams.Frequency = freq);
            Beat.Subscribe(beat => Model.BeatParams.Beat = beat);
            LoopNum.Subscribe(lnum => Model.BeatParams.LoopCount = lnum);
            EnableDecrement.Subscribe(dec => Model.BeatParams.Decrement = dec);

            Action<int> UpdateElemNum = _ => ElementNum.Value = Model.BeatParams.ElementCount;
            Frequency.Subscribe(UpdateElemNum);
            Beat.Subscribe(UpdateElemNum);
            LoopNum.Subscribe(UpdateElemNum);
            EnableDecrement.Subscribe(_ => UpdateElemNum(0));
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
