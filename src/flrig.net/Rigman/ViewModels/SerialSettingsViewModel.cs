#if DEBUG
using System.Diagnostics;
#endif
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using flrig.net.lib;
using ReactiveUI;
using Splat;

namespace flrig.net.ViewModels
{
    public class SerialSettingsViewModel : ViewModelBase, ISerialSettingsViewModel
    {
        private string _greeting;

        public SerialSettingsViewModel()
        {
            Activator = new ViewModelActivator();

            JustAClick = ReactiveCommand.Create<Unit, Unit>(args =>
            {
#if DEBUG
                Debug.WriteLine($"[vm {Thread.CurrentThread.ManagedThreadId}]: Serial View Model -> In JustAClick");
#endif
                return Unit.Default;
            });

            this.WhenActivated(disposables =>
            {
                HandleActivation();
                Disposable.Create(HandleDeactivation).DisposeWith(disposables);

                Observable
                    .Timer(
                        TimeSpan.FromMilliseconds(100), // give the view time to activate
                        TimeSpan.FromMilliseconds(1000),
                        RxApp.MainThreadScheduler)
                    .Take(Traits.Length)
                    .Do(
                        t =>
                        {
                            var newGreeting = $"Hello, {Traits[t % Traits.Length]} world !";
#if DEBUG
                            Debug.WriteLine($"[vm {Thread.CurrentThread.ManagedThreadId}]: Serial View Model Timer Observable -> Setting greeting to: \"{newGreeting}\"");
#endif
                            Greeting = newGreeting;
                        },
                        () =>
                        {
#if DEBUG
                            Debug.WriteLine("Those are all the greetings, folks! Feel free to close the window now...\n");
#endif
                        })
                    .Subscribe()
                    .DisposeWith(disposables);

            });

            this
                .WhenAnyValue(vm => vm.Greeting)
                .Skip(1)
                .Do(
                    greeting =>
                    {
#if DEBUG
                        Debug.WriteLine($"[vm {Thread.CurrentThread.ManagedThreadId}]: Serial View Model WhenAnyValue()   ->  Greeting value changed to: \"{greeting}\"\n");
#endif
                    })
                .Subscribe();
        }

        private void HandleDeactivation()
        {
#if DEBUG
            Debug.WriteLine($"[vm  {Thread.CurrentThread.ManagedThreadId}]: Serial View Model deactivated");
#endif
        }

        private void HandleActivation()
        {
#if DEBUG
            Debug.WriteLine($"[vm  {Thread.CurrentThread.ManagedThreadId}]: Serial View Model activated\n");

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

        public ReactiveCommand<Unit, Unit> JustAClick { get; }

        public string Greeting
        {
            get => _greeting;
            set => this.RaiseAndSetIfChanged(ref _greeting, value);
        }

        private static readonly string[] Traits = {
            "expressive",
            "clear",
            "responsive",
            "concurrent",
            "reactive"
        };

        public ViewModelActivator Activator { get; }
    }

    public interface ISerialSettingsViewModel : IActivatableViewModel
    {
        ReactiveCommand<Unit, Unit> JustAClick { get; }
        string Greeting { get; set; }
    }
}