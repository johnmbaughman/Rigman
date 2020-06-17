using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using flrig.net.lib;
using flrig.net.ViewModels;
using flrig.net.Views;

namespace flrig.net
{
    public class App : Application
    {
        public string PluginsPath
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                path = Path.Combine(Path.GetDirectoryName(path), "Plugins\\netstandard2.0");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(path), "Plugins\\netstandard2.0"));
                }

                return Path.Combine(path, "flrig.net.lib.dll");
            }
        }


        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            string[] pluginPaths = new string[]
            {
                PluginsPath
            };

            IEnumerable<IRigs> commands = pluginPaths.SelectMany(pluginPath =>
            {
                Assembly pluginAssembly = LoadPlugin(pluginPath);
                return CreateCommands(pluginAssembly);
            }).ToList();

            var args = new List<string> { "hello" }.ToArray();
            if (args.Length == 0)
            {
                Debug.WriteLine("Commands: ");
                foreach (IRigs command in commands)
                {
                    Console.WriteLine($"{command.Name}\t - {command.Description}");
                }
            }
            else
            {
                foreach (string commandName in args)
                {
                    Debug.WriteLine($"-- {commandName} --");

                    IRigs command = commands.FirstOrDefault(c => c.Name == commandName);
                    if (command == null)
                    {
                        Console.WriteLine("No such command is known.");
                        return;
                    }

                    command.Execute();

                    Debug.WriteLine(Environment.NewLine);
                }
            }

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        static Assembly LoadPlugin(string relativePath)
        {
            // Navigate up to the solution root
            string root = Path.GetFullPath(Path.Combine(
                Path.GetDirectoryName(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(
                                Path.GetDirectoryName(typeof(Program).Assembly.Location)))))));

            string pluginLocation = Path.GetFullPath(Path.Combine(root, relativePath.Replace('\\', Path.DirectorySeparatorChar)));
            Console.WriteLine($"Loading commands from: {pluginLocation}");
            PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }

        static IEnumerable<IRigs> CreateCommands(Assembly assembly)
        {
            int count = 0;

            foreach (Type type in assembly.GetTypes())
            {
                if (!typeof(IRigs).IsAssignableFrom(type)) continue;
                if (!(Activator.CreateInstance(type) is IRigs result)) continue;
                count++;
                yield return result;
            }

            if (count != 0) yield break;
            string availableTypes = string.Join(",", assembly.GetTypes().Select(t => t.FullName));
            throw new ApplicationException(
                $"Can't find any type which implements ICommand in {assembly} from {assembly.Location}.\n" +
                $"Available types: {availableTypes}");
        }
    }
}
