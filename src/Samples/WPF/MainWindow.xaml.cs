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
        public CommandKeyBinding TestShortcut{ get; } = new CommandKeyBinding("Test", new KeyStroke(Key.T));

        public MainWindow()
        {
            this.InitializeComponent();
            var bindings = new ObservableCollection<CommandKeyBinding> {
                new CommandKeyBinding {
                    CommandName = "42",
                },
                this.TestShortcut,
            };
            for (int i = 0; i < 30; i++)
                bindings.Add(new CommandKeyBinding {CommandName = i.ToString()});
            this.KeySettings.DataContext = bindings;
        }
    }
}
