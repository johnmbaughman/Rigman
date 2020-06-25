using System;
#if DEBUG
using System.Diagnostics;
#endif
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using flrig.net.ViewModels;
using ReactiveUI;

namespace flrig.net.Views
{
    public class SerialSettings : ReactiveWindow<ISerialSettingsViewModel>
    {
        public SerialSettings()
        {
            this.WhenActivated(disposables =>
            {
#if DEBUG
                Debug.WriteLine($"[v  {Thread.CurrentThread.ManagedThreadId}]: Serial View activated\n");


                Disposable
                    .Create(() => Debug.WriteLine($"[v  {Thread.CurrentThread.ManagedThreadId}]: Serial View deactivated"))
                    .DisposeWith(disposables);

                Observable
                    .FromEventPattern(wndSerial, nameof(wndSerial.Closing))
                    .Subscribe(_ => Debug.WriteLine($"[v  {Thread.CurrentThread.ManagedThreadId}]: Serial window closing..."))
                    .DisposeWith(disposables);
#endif
                this
                    .OneWayBind(ViewModel, vm => vm.Greeting, v => v.Greeting.Text);

                this
                    .BindCommand(ViewModel, vm => vm.JustAClick, v => v.BtnClick)
                    .DisposeWith(disposables);
            });
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }

        private TextBlock Greeting => this.FindControl<TextBlock>("Greeting");
        private Button BtnClick => this.FindControl<Button>("BtnClick");
        private Window wndSerial => this.FindControl<Window>("wndSerial");
    }
}
