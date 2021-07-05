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
    internal class ViewModel : INotifyPropertyChanged, IDisposable
    {
        private bool disposedValue;

        public event PropertyChangedEventHandler? PropertyChanged;
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        public ReactiveProperty<string> FilePath { get; }
        public ReactiveProperty<decimal?> Interval { get; }
        public ReactiveProperty<decimal?> BPM { get; }
        public ReactiveProperty<int> Frequency { get; }
        public ReactiveProperty<int> Beat { get; }
        public ReactiveProperty<int> LoopNum { get; }
        public ReactiveProperty<bool> EnableDecrement { get; }
        public ReactiveProperty<int> ElementNum { get; }

        public ViewModel()
        {
            FilePath = new ReactiveProperty<string>().AddTo(Disposable);
            Interval = new ReactiveProperty<decimal?>().AddTo(Disposable);
            BPM = new ReactiveProperty<decimal?>().AddTo(Disposable);
            Frequency = new ReactiveProperty<int>().AddTo(Disposable);
            Beat = new ReactiveProperty<int>().AddTo(Disposable);
            LoopNum = new ReactiveProperty<int>().AddTo(Disposable);
            EnableDecrement = new ReactiveProperty<bool>().AddTo(Disposable);
            ElementNum = new ReactiveProperty<int>().AddTo(Disposable);
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
