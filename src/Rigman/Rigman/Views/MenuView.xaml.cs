using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Rigman.ViewModels;
using ReactiveUI;

namespace Rigman.Views
{
    public class MenuView : ReactiveUserControl<MenuViewModel>
    {
        public MenuView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.WhenActivated(disposables =>
            {
                ViewModel = new MenuViewModel
                {
                    Context = ((MainWindow) Parent.Parent).ViewModel.AppData
                };

                this.BindCommand(ViewModel, x => x.SettingsViewCommand, x => x.Settings)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.ExitCommand, x => x.Exit)
                    .DisposeWith(disposables);

            });
        }

        public MenuItem Settings => this.FindControl<MenuItem>("Settings");
        public MenuItem Exit => this.FindControl<MenuItem>("Exit");
    }
}
