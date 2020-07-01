#if DEBUG
using System.Diagnostics;
#endif
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Rigman.ViewModels;
using ReactiveUI;

namespace Rigman.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            ViewModel = new MainWindowViewModel();

            this.WhenActivated(disposables =>
            {
#if DEBUG
                Debug.WriteLine($"[v  {Thread.CurrentThread.ManagedThreadId}]: Main View activated\n");


                Disposable
                    .Create(() => Debug.WriteLine($"[v  {Thread.CurrentThread.ManagedThreadId}]: Main View deactivated"))
                    .DisposeWith(disposables);

                Observable
                    .FromEventPattern(wndMain, nameof(wndMain.Closing))
                    .Subscribe(_ => { Debug.WriteLine($"[v  {Thread.CurrentThread.ManagedThreadId}]: Main window closing..."); })
                    .DisposeWith(disposables);
#endif

                this
                    .BindCommand(ViewModel, vm => vm.OnClickCommand, v => v.BtnSerialSettings)
                    .DisposeWith(disposables);
            });

            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private Button BtnSerialSettings => this.FindControl<Button>("btnSerialSettings");

        private Window wndMain => this.FindControl<Window>("wndMain");
    }
}
