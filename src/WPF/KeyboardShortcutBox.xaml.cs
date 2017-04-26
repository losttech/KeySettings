namespace LostTech.App
{
    using System;
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

        public bool IsCapturingGesture {
            get => (bool) this.GetValue(IsCapturingGestureProperty);
            private set => this.SetValue(IsCapturingGestureProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsCapturingGesture.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCapturingGestureProperty =
            DependencyProperty.Register(nameof(IsCapturingGesture), typeof(bool), typeof(KeyboardShortcutBox),
                new PropertyMetadata(false, propertyChangedCallback: IsCapturingGesturePropertyChanged));

        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            if (false.Equals(e.NewValue))
                this.IsCapturingGesture = false;

            base.OnIsKeyboardFocusWithinChanged(e);
        }

        protected virtual void OnIsCapturingGestureChanged(bool newValue)
        {
            if (newValue && object.ReferenceEquals(this.EnterShortcutButton, Keyboard.FocusedElement))
                Keyboard.Focus(this.KeyText);
        }

        static void IsCapturingGesturePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            ((KeyboardShortcutBox) source).OnIsCapturingGestureChanged((bool) e.NewValue);
        }

        void TextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (!this.IsCapturingGesture) {
                if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.None) {
                    this.IsCapturingGesture = true;
                    e.Handled = true;
                }
                return;
            }

            try {
                this.Shortcut = Keyboard.Modifiers == ModifierKeys.None
                    ? new KeyGesture(e.Key)
                    : new KeyGesture(e.Key, Keyboard.Modifiers);
                e.Handled = true;
            }
            catch (NotSupportedException) {

            }
            this.IsCapturingGesture = false;
        }

        void EnterShortcutButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsCapturingGesture = true;
            if (object.ReferenceEquals(this.EnterShortcutButton, Keyboard.FocusedElement))
                Keyboard.Focus(this.KeyText);
        }
    }
}
