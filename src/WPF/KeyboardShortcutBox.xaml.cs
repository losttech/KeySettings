namespace LostTech.App
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Gma.System.MouseKeyHook;
    using LostTech.App.Input;

    /// <summary>
    /// Interaction logic for KeyboardShortcutBox.xaml
    /// </summary>
    public partial class KeyboardShortcutBox : UserControl
    {
        IKeyboardMouseEvents globalHook;

        public KeyboardShortcutBox()
        {
            this.InitializeComponent();
        }

        public KeyStroke Shortcut {
            get => (KeyStroke) this.GetValue(ShortcutProperty);
            set => this.SetValue(ShortcutProperty, value);
        }

        public bool IsCapturingGesture {
            get => (bool) this.GetValue(IsCapturingGestureProperty);
            private set => this.SetValue(IsCapturingGestureProperty, value);
        }

        public bool ExtendedCapture {
            get => (bool) this.GetValue(ExtendedCaptureProperty);
            set => this.SetValue(ExtendedCaptureProperty, value);
        }

        public bool ShowEditButton {
            get => (bool)this.GetValue(ShowEditButtonProperty);
            set => this.SetValue(ShowEditButtonProperty, value);
        }

        public void BeginCapture()
        {
            Keyboard.Focus(this.KeyText);
            this.IsCapturingGesture = true;
        }


        void StopGlobalHook()
        {
            this.globalHook?.Dispose();
            this.globalHook = null;
        }

        void StartGlobalHook()
        {
            this.globalHook = Hook.GlobalEvents();
            this.downKeys.Clear();
            this.globalHook.KeyDown += this.GlobalKeyDown;
            this.globalHook.KeyUp += this.GlobalKeyUp;
        }

        readonly HashSet<Key> downKeys = new HashSet<Key>();

        void GlobalKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            e.Handled = true;
            Key key = AsWpf(e.KeyData);
            this.downKeys.Add(key);
            ModifierKeys modifiers = this.downKeys.Select(GetModifier).Aggregate((a, b) => a | b);
            IEnumerable<Key> keys = this.downKeys.Where(k => GetModifier(k) == ModifierKeys.None);
            this.Shortcut = new KeyStroke(modifiers, keys);
        }

        static Key AsWpf(System.Windows.Forms.Keys key) => KeyInterop.KeyFromVirtualKey((int) key);

        void GlobalKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            e.Handled = true;
            this.IsCapturingGesture = false;
        }

        static ModifierKeys GetModifier(Key key)
        {
            switch (key) {
            case Key.LeftAlt: case Key.RightAlt:
                return ModifierKeys.Alt;
            case Key.LeftCtrl: case Key.RightCtrl:
                return ModifierKeys.Control;
            case Key.LeftShift: case Key.RightShift:
                return ModifierKeys.Shift;
            case Key.LWin: case Key.RWin:
                return ModifierKeys.Windows;
            default:
                return ModifierKeys.None;
            }
        }
        static ModifierKeys GetKeyboardModifiers()
            => Keyboard.Modifiers | (IsWinDown() ? ModifierKeys.Windows : ModifierKeys.None);

        static bool IsWinDown() => Keyboard.IsKeyDown(Key.LWin) || Keyboard.IsKeyDown(Key.RWin);

        // Using a DependencyProperty as the backing store for ShowEditButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowEditButtonProperty =
            DependencyProperty.Register(nameof(ShowEditButton), typeof(bool), typeof(KeyboardShortcutBox),
                new PropertyMetadata(true));
        // Using a DependencyProperty as the backing store for IsCapturingGesture.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCapturingGestureProperty =
            DependencyProperty.Register(nameof(IsCapturingGesture), typeof(bool), typeof(KeyboardShortcutBox),
                new PropertyMetadata(false, propertyChangedCallback: IsCapturingGesturePropertyChanged));
        // Using a DependencyProperty as the backing store for Shortcut.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShortcutProperty =
            DependencyProperty.Register(nameof(Shortcut), typeof(KeyStroke), typeof(KeyboardShortcutBox),
                new PropertyMetadata(null));

        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            if (false.Equals(e.NewValue))
                this.IsCapturingGesture = false;

            base.OnIsKeyboardFocusWithinChanged(e);
        }

        protected virtual void OnIsCapturingGestureChanged(bool newValue)
        {
            if (newValue && ReferenceEquals(this.EnterShortcutButton, Keyboard.FocusedElement))
                Keyboard.Focus(this.KeyText);

            this.StopGlobalHook();
            if (this.ExtendedCapture && newValue)
                this.StartGlobalHook();
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

            this.Shortcut = new KeyStroke(e.Key, Keyboard.Modifiers);
            e.Handled = true;
            this.IsCapturingGesture = false;
        }

        void EnterShortcutButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsCapturingGesture = true;
            if (ReferenceEquals(this.EnterShortcutButton, Keyboard.FocusedElement))
                Keyboard.Focus(this.KeyText);
        }

        // Using a DependencyProperty as the backing store for ExtendedCapture.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExtendedCaptureProperty =
            DependencyProperty.Register(nameof(ExtendedCapture), typeof(bool), typeof(KeyboardShortcutBox),
                new PropertyMetadata(false, propertyChangedCallback: ExtendedCapturePropertyChanged));

        static void ExtendedCapturePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            ((KeyboardShortcutBox)source).OnExtendedCaptureChanged((bool)e.NewValue);
        }

        protected virtual void OnExtendedCaptureChanged(bool newValue)
        {
            if (!this.IsCapturingGesture)
                return;

            this.StopGlobalHook();
            if (newValue)
                this.StartGlobalHook();
        }

    }
}
