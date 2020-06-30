using System;
using System.Reactive;
using Dock.Model.Controls;
using flrig.net.lib;
using ReactiveUI;

namespace flrig.net.ViewModels
{
    public class ToolBarViewModel : Tool
    {
        public ToolBarViewModel()
        {
            SettingsViewCommand = ReactiveCommand.Create<Unit, Unit>(SerialSettings);
        }

        private Unit SerialSettings(Unit arg)
        {
            Interactions.SerialSettings.Handle(null).Subscribe();
            return Unit.Default;
        }

        public AppData AppData => ((AppData)Context);

        public ReactiveCommand<Unit, Unit> SettingsViewCommand { get; set; }
    }
}