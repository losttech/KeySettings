namespace WPF
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using LostTech.App;
    using LostTech.App.Input;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.KeySettings.DataContext = new ObservableCollection<CommandKeyBinding> {
                new CommandKeyBinding {
                    CommandName = "42",
                },
                new CommandKeyBinding {
                    CommandName = "Test",
                    Shortcut = new KeyStroke(Key.T, ModifierKeys.Control),
                },
            };
        }
    }
}
