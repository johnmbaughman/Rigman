using System.Diagnostics;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Splat;
using flrig.net.lib;
using flrig.net.ViewModels;
using flrig.net.Views;
using System.Reflection;
using System.Runtime.Loader;
using System;

namespace flrig.net
{
    /// <summary>
    /// Class App.
    /// Implements the <see cref="Avalonia.Application" />
    /// </summary>
    /// <seealso cref="Avalonia.Application" />
    public class App : Application
    {
        /// <summary>
        /// Initializes the application by loading XAML etc.
        /// </summary>
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        /// Called when [framework initialization completed].
        /// </summary>
        public override void OnFrameworkInitializationCompleted()
        {
            RegisterDependencies();
            RegisterPlugins();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        /// <summary>
        /// Registers the dependencies.
        /// </summary>
        private void RegisterDependencies()
        {
            Locator.CurrentMutable.Register(() => new PlaceHolderClass(), typeof(IPlaceHolderClass));
            // Register dependencies here.
        }

        /// <summary>Registers the plugins.</summary>
        private void RegisterPlugins()
        {
            var pluginPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Plugins");
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
                        var instance = (IRigs) assembly.CreateInstance(info.FullName); 
                        Locator.CurrentMutable.RegisterConstant(instance, typeof(IRigs));
                    }
                }
            }
        }
    }
}