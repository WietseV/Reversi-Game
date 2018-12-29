using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Model.Reversi;
using ViewModel;
using System.ComponentModel;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Game = new ReversiGame(8, 8);
            WindowViewModel wvm = new WindowViewModel(Game);
            this.DataContext = new Navigator(wvm);
            //this.DataContext = new BoardViewModel(Game.Board, Game);

        }

        public ReversiGame Game { get; set; }
        public ReversiBoard Board { get; set; }
    }

    public class Navigator : INotifyPropertyChanged
    {
        public Screen currentScreen;
        public WindowViewModel wvm;

        public Navigator(WindowViewModel wvm)
        {
            this.currentScreen = new MainScreen(this);
            this.wvm = wvm;
        }

        public Screen CurrentScreen
        {
            get
            {
                return currentScreen;
            }
            set
            {
                this.currentScreen = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentScreen)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
       }

    public abstract class Screen
        {
            public readonly Navigator navigator;

            public Screen(Navigator navigator)
            {
                this.navigator = navigator;
            }

            public void SwitchTo(Screen screen)
            {
                this.navigator.CurrentScreen = screen;
            }
        }

    public class MainScreen : Screen
        {
            public MainScreen(Navigator navigator) : base(navigator)
            {
                GoToSettings = new EasyCommand(() => SwitchTo(new SettingsScreen(navigator)));
            }

            public ICommand GoToSettings { get; }
        }

    public class SettingsScreen : Screen
        {
            public SettingsScreen(Navigator navigator) : base(navigator)
            {
                GoToMain = new EasyCommand(() => SwitchTo(new MainScreen(navigator)));
            }

            public ICommand GoToMain { get; }
        }

    public class EasyCommand : ICommand
        {
            public readonly Action action;

            public EasyCommand(Action action)
            {
                this.action = action;
            }

            // The add { } remove { } gets rid of annoying warning
            public event EventHandler CanExecuteChanged { add { } remove { } }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                action();
            }
        }
    
}
