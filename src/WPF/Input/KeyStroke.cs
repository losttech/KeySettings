namespace LostTech.App.Input
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows.Input;

    public sealed class KeyStroke: INotifyPropertyChanged
    {
        public KeyStroke() { }

        public KeyStroke(Key key, ModifierKeys modifiers = ModifierKeys.None)
        {
            this.Keys.Add(key);
            this.Modifiers = modifiers;
        }

        public KeyStroke(params Key[] keys)
        {
            foreach (Key key in keys)
                this.Keys.Add(key);
        }

        public KeyStroke(ModifierKeys modifiers, IEnumerable<Key> keys = null)
        {
            this.Modifiers = modifiers;

            if (keys == null)
                return;

            foreach (Key key in keys)
                this.Keys.Add(key);
        }

        static readonly KeyConverter KeyConverter = new KeyConverter();
        static readonly ModifierKeysConverter ModifierConverter = new ModifierKeysConverter();

        ModifierKeys modifiers;
        public ObservableCollection<Key> Keys { get; } = new ObservableCollection<Key>();

        public ModifierKeys Modifiers
        {
            get => this.modifiers;
            set {
                this.modifiers = value;
                this.OnPropertyChanged();
            }
        }

        public string ToString(CultureInfo cultureInfo)
        {
            var result = new StringBuilder();
            if (this.Modifiers != ModifierKeys.None) {
                result.Append(ModifierConverter.ConvertToString(null, cultureInfo, this.Modifiers));
                result.Append('+');
            }
            for (int i = 0; i < this.Keys.Count; i++) {
                result.Append(KeyConverter.ConvertToString(null, cultureInfo, this.Keys[i]));
                if (i + 1 != this.Keys.Count)
                    result.Append('+');
            }
            return result.ToString();
        }

        public override string ToString() => this.ToString(CultureInfo.InvariantCulture);

        public override bool Equals(object obj)
        {
            if (obj is KeyStroke other)
                return this.Equals(other);

            return false;
        }

        public bool Equals(KeyStroke other)
        {
            return other?.Modifiers == this.Modifiers
                && new SortedSet<Key>(this.Keys).SetEquals(new SortedSet<Key>(other.Keys));
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
