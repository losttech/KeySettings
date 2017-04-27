namespace LostTech.App
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for KeySettingsEditor.xaml
    /// </summary>
    public partial class KeySettingsEditor : UserControl
    {
        public KeySettingsEditor()
        {
            InitializeComponent();
        }

        public bool ExtendedCapture {
            get => (bool)this.GetValue(ExtendedCaptureProperty);
            set => this.SetValue(ExtendedCaptureProperty, value);
        }

        // Using a DependencyProperty as the backing store for ExtendedCapture.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExtendedCaptureProperty =
            DependencyProperty.Register(nameof(ExtendedCapture), typeof(bool), typeof(KeySettingsEditor), new PropertyMetadata(false));

        void BindingList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.None) {
                var item = this.BindingList.SelectedItem;
                if (item != null) {
                    var itemContainer = (ListViewItem)this.BindingList.ItemContainerGenerator.ContainerFromItem(item);
                    var keyBox = itemContainer.Tag as KeyboardShortcutBox;
                    keyBox?.BeginCapture();
                }
                e.Handled = true;
            }
        }

        void ShortcutBox_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var control = (Control)sender;
            var presenter = (ContentPresenter)control.TemplatedParent;
            var item = (ListViewItem)this.BindingList.ItemContainerGenerator.ContainerFromItem(presenter.Content);
            item.Tag = control;
        }
    }
}
