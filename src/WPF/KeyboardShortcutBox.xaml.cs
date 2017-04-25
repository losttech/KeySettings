namespace LostTech.App
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for KeyboardShortcutBox.xaml
    /// </summary>
    public partial class KeyboardShortcutBox : UserControl
    {
        public KeyboardShortcutBox()
        {
            this.InitializeComponent();
        }

        public KeyGesture Shortcut {
            get => (KeyGesture) this.GetValue(ShortcutProperty);
            set => this.SetValue(ShortcutProperty, value);
        }

        // Using a DependencyProperty as the backing store for Shortcut.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShortcutProperty =
            DependencyProperty.Register(nameof(Shortcut), typeof(KeyGesture), typeof(KeyboardShortcutBox),
                new PropertyMetadata(null));

    }
}
