using Dock.Model.Controls;
using Rigman.Common;
using ReactiveUI;
using System;
using System.Reactive;

namespace Rigman.ViewModels
{
    public class MenuViewModel : Tool
    {
        public MenuViewModel()
        {
            SettingsViewCommand = ReactiveCommand.Create<Unit, Unit>(SerialSettings);
            ExitCommand = ReactiveCommand.Create<Unit, Unit>(Exit);
        }

        private Unit SerialSettings(Unit arg)
        {
            Interactions.SerialSettings.Handle(null).Subscribe();
            return Unit.Default;
        }

        private Unit Exit(Unit arg)
        {
            Interactions.Exit.Handle(Unit.Default).Subscribe();
            return Unit.Default;
        }

        public AppData AppData => (AppData)Context;
        public ReactiveCommand<Unit, Unit> SettingsViewCommand { get; set; }
        public ReactiveCommand<Unit, Unit> ExitCommand { get; set; }
    }
}