using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using flrig.net.ViewModels;
using ReactiveUI;

namespace flrig.net.Views
{
    public class ToolBarView : ReactiveUserControl<ToolBarViewModel>
    {
        public ToolBarView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.WhenActivated(disposables =>
            {
                ViewModel = new ToolBarViewModel
                {
                    Context = ((MainWindow) Parent.Parent).ViewModel.AppData
                };

                this.BindCommand(ViewModel, x => x.SettingsViewCommand, x => x.Settings)
                    .DisposeWith(disposables);
            });
        }

        public MenuItem Settings => this.FindControl<MenuItem>("Settings");

    }
}
