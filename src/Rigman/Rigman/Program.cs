using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reflection;
using System.Runtime.Loader;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging.Serilog;
using Avalonia.ReactiveUI;
using Rigman.Common;
using Rigman.ViewModels;
using Rigman.Views;
using ReactiveUI;
using Splat;

namespace Rigman
{
    internal static class Program
    {
        private static MainWindow _mainWindow;

        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args) => BuildAvaloniaApp().Start(AppMain, args);

        private static void AppMain(Application app, string[] args)
        {
            RxApp.DefaultExceptionHandler = new ExceptionHandler();

            _mainWindow = new MainWindow();
            _mainWindow.DataContext = new MainWindowViewModel
            {
                Window = _mainWindow
            };

            RegisterInteractions();
            RegisterDependencies();
            RegisterPlugins();

            app.Run(_mainWindow);
        }

        private static void RegisterInteractions()
        {
            Interactions.SerialSettings.RegisterHandler(
                async interaction =>
                {
                    var dialog = new SerialSettings { ViewModel = new SerialSettingsViewModel() };
                    await dialog.ShowDialog(_mainWindow);
                    _mainWindow.Focus();
                });
        }

        /// <summary>
        /// Registers the dependencies.
        /// </summary>
        private static void RegisterDependencies()
        {
            Locator.CurrentMutable.Register(() => new PlaceHolderClass(), typeof(IPlaceHolderClass));
            // Register dependencies here.
        }

        /// <summary>Registers the plugins.</summary>
        private static void RegisterPlugins()
        {
            var pluginPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Plugins");
            if (!Directory.Exists(pluginPath))
            {
                Directory.CreateDirectory(pluginPath);
            }

            var plugins = new DirectoryInfo(pluginPath).GetFiles().Select(f => f.FullName).ToList();
            if (!plugins.Any()) return;

            var pluginAssemblies = new AssemblyLoadContext("Plugins", true);

            foreach (var plugin in plugins)
            {
                pluginAssemblies.LoadFromAssemblyPath(plugin);

                foreach (var assembly in pluginAssemblies.Assemblies)
                {
#if DEBUG
                    foreach (var definedType in assembly.DefinedTypes)
                    {
                        Debug.WriteLine(definedType.Name);
                        foreach (var implementedInterface in definedType.ImplementedInterfaces)
                        {
                            Debug.WriteLine(implementedInterface.Name);
                        }
                    }
#endif

                    var classes = assembly.DefinedTypes.Where(dt => dt.ImplementedInterfaces.Any(ii => ii.Name == "IRigs"));
                    foreach (var info in classes)
                    {
                        var instance = (IRigs)assembly.CreateInstance(info.FullName ?? "UNKNOWN");
                        Locator.CurrentMutable.RegisterConstant(instance, typeof(IRigs));
                    }
                }
            }
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
