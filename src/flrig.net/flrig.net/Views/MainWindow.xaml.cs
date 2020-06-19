using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Splat;
using flrig.net.lib;

namespace flrig.net.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();

            var rig = Locator.Current.GetService<IRigs>();
            if (rig != null)
            {
                Debug.WriteLine(rig.Name);
            }
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
