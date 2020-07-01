using System;
using System.Reactive;
using Rigman.Common.Models;
using ReactiveUI;

namespace Rigman.Common
{
    public static class Interactions
    {
        public static readonly Interaction<SerialSettings, SerialSettings> SerialSettings = new Interaction<SerialSettings, SerialSettings>();

        public static readonly Interaction<Unit, Unit> Exit = new Interaction<Unit, Unit>();

        public static readonly Interaction<Exception, Unit> Exceptions = new Interaction<Exception, Unit>();
    }
}