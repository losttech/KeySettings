namespace LostTech.App
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using LostTech.App.Annotations;
    using LostTech.App.Input;

    public sealed class CommandKeyBinding: INotifyPropertyChanged
    {
        string commandName;
        KeyStroke shortcut;

        public CommandKeyBinding() { }

        public CommandKeyBinding(string commandName, [NotNull] KeyStroke shortcut)
        {
            this.commandName = commandName ?? throw new ArgumentNullException(nameof(commandName));
            this.shortcut = shortcut ?? throw new ArgumentNullException(nameof(shortcut));
        }

        public string CommandName
        {
            get => this.commandName;
            set {
                if (value == this.commandName)
                    return;
                this.commandName = value;
                this.OnPropertyChanged();
            }
        }

        public KeyStroke Shortcut
        {
            get => this.shortcut;
            set {
                if (Equals(value, this.shortcut))
                    return;
                this.shortcut = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
