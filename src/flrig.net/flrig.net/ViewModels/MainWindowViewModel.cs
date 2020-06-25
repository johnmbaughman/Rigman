using System;
using System.Diagnostics;
using System.Reactive;
using Avalonia;
using flrig.net.lib;
using flrig.net.Views;
using ReactiveUI;

namespace flrig.net.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Interactions.SerialSettings.RegisterHandler(
                async interaction =>
                {
                    var dialog = new SerialSettings();
                    await dialog.ShowDialog(Window);
                    Window.Focus();
                });

            OnClickCommand = ReactiveCommand.Create<Unit, Unit>((args) =>
            {
#if DEBUG
                Debug.WriteLine("In click");
#endif
                Interactions.SerialSettings.Handle(null).Subscribe();
                return Unit.Default;
            });
        }

        public string Greeting => "Welcome to Avalonia!";

        public ReactiveCommand<Unit, Unit> OnClickCommand { get; }

        public MainWindow Window { get;  set; }
    }
}
