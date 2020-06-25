using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging.Serilog;
using Avalonia.ReactiveUI;
using flrig.net.lib;
using flrig.net.ViewModels;
using flrig.net.Views;
using ReactiveUI;

namespace flrig.net
{
    internal static class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args) => BuildAvaloniaApp().Start(AppMain, args);

        private static void AppMain(Application app, string[] args)
        {
            RxApp.DefaultExceptionHandler = new ExceptionHandler();

            var mainWindow = new MainWindow();
            mainWindow.DataContext = new MainWindowViewModel
            {
                Window = mainWindow
            };
            app.Run(mainWindow);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug()
                .UseReactiveUI();
    }

    public class ExceptionHandler : IObserver<Exception>
    {
        public void OnNext(Exception value)
        {
            if (Debugger.IsAttached) Debugger.Break();
            Interactions.Exceptions.Handle(value).Subscribe();
        }

        public void OnError(Exception value)
        {
            if (Debugger.IsAttached) Debugger.Break();
            Interactions.Exceptions.Handle(value).Subscribe();
        }

        public void OnCompleted()
        {
            if (Debugger.IsAttached) Debugger.Break();
            RxApp.MainThreadScheduler.Schedule(() => throw new NotImplementedException());
        }
    }
}
