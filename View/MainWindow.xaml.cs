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
            this.DataContext = new Navigator(this);
            //this.DataContext = new BoardViewModel(Game.Board, Game);

        }

        public ReversiGame Game { get; set; }
        public ReversiBoard Board { get; set; }
    }

    public class Navigator : INotifyPropertyChanged
    {
        public Screen currentScreen;
        public WindowViewModel Wvm { get; internal set; }
        internal MainWindow Window { get; private set; }

        public Navigator(MainWindow window)
        {
            Window = window;
            this.currentScreen = new SettingsScreen(this);
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

            public WindowViewModel Wvm { get => this.navigator.Wvm; }

            public ICommand GoToSettings { get; }
        }

    public class SettingsScreen : Screen
        {
            public SettingsScreen(Navigator navigator) : base(navigator)
            {
                StartGame = new StartCommand(this);
            }
        
            public ICommand StartGame { get; }

            public class StartCommand : ICommand
            {
            private readonly SettingsScreen  Ssc;

                public StartCommand(SettingsScreen Ssc)
                {
                    this.Ssc = Ssc;
                Execute(CanExecute(true));
                }

                // The add { } remove { } gets rid of annoying warning
                public event EventHandler CanExecuteChanged { add { } remove { } }

                public bool CanExecute(object parameter)
                {
                    return true;
                }

                public void Execute(object parameter)
                {
                this.Ssc.navigator.Wvm = new WindowViewModel(new ReversiGame(8, 8));
                this.Ssc.SwitchTo(new MainScreen(this.Ssc.navigator));
                }
            }
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
