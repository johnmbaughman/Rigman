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
    }
}