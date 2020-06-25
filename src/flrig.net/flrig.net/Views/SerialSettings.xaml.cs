using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace flrig.net.Views
{
    public class SerialSettings : Window
    {
        public SerialSettings()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
