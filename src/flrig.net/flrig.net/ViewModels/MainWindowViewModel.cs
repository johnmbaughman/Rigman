using System;
#if DEBUG
using System.Diagnostics;
#endif
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading;
using flrig.net.lib;
using flrig.net.Views;
using ReactiveUI;
using Splat;

namespace flrig.net.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IActivatableViewModel
    {

        public MainWindowViewModel()
        {
            Activator = new ViewModelActivator();

            OnClickCommand = ReactiveCommand.Create<Unit, Unit>((args) =>
            {
#if DEBUG
                Debug.WriteLine($"[vm {Thread.CurrentThread.ManagedThreadId}]: Main View Model -> In OnClickCommand");
#endif
                Interactions.SerialSettings.Handle(null).Subscribe();
                return Unit.Default;
            });

            this.WhenActivated(disposables =>
            {
                HandleActivation();
                Disposable.Create(HandleDeactivation).DisposeWith(disposables);
            });

        }

        private void HandleDeactivation()
        {
#if DEBUG
            Debug.WriteLine($"[vm {Thread.CurrentThread.ManagedThreadId}]: Main View Model deactivated");
#endif
        }

        private void HandleActivation()
        {
#if DEBUG
            Debug.WriteLine($"[vm  {Thread.CurrentThread.ManagedThreadId}]: Main View Model activated\n");

            var rig = Locator.Current.GetService<IRigs>();
            if (rig != null)
            {
                Debug.WriteLine(rig.Name);
            }

            var other = Locator.Current.GetService<IPlaceHolderClass>();
            other.Nothing = Greeting;
            Debug.WriteLine(other.Nothing);
#endif
        }

        public string Greeting => "Welcome to Avalonia!";

        public ReactiveCommand<Unit, Unit> OnClickCommand { get; }

        public MainWindow Window { get; set; }

        public ViewModelActivator Activator { get; }
    }
}
