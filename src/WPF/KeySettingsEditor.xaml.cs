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

        void CommandNameContainer_Click(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var item = (ContentPresenter)this.ShortcutList.ItemContainerGenerator.ContainerFromItem(element.Tag);
            ((KeyboardShortcutBox)item.Tag)?.BeginCapture();
        }

        void ShortcutBox_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var control = (Control)sender;
            var presenter = (ContentPresenter)control.TemplatedParent;
            presenter.Tag = control;
        }
    }
}
