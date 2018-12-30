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
        private Screen currentScreen;
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

    public class SettingsScreen : Screen, INotifyPropertyChanged
        {
            public SettingsScreen(Navigator navigator) : base(navigator)
            {
                StartGame = new StartCommand(this);
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public WindowViewModel Wvm { get => this.navigator.Wvm; }

            public ICommand StartGame { get; }

            private int height = 8;
            public int Height
            {
                get { return height; }
                set
                {
                if (ReversiBoard.IsValidHeight(value))
                {
                    height = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(height)));
                }
                }
            }

            private int width = 8;
            public int Width
            {
                get { return width; }
                set
                {
                    if (ReversiBoard.IsValidWidth(value))
                    {
                        width = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(width)));
                    }
                }
            }

            private String playerBlackName = "Black" ;
            public String PlayerBlackName
        {
                get { return playerBlackName; }
                set
                {
                    playerBlackName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(playerBlackName)));
                }
            }

            private String playerWhiteName = "White";
            public String PlayerWhiteName
        {
                get { return playerWhiteName; }
                set
                {
                    playerWhiteName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(playerWhiteName)));
                }
            }

            public class StartCommand : ICommand
            {
            private readonly SettingsScreen  Ssc;

                public StartCommand(SettingsScreen Ssc)
                {
                    this.Ssc = Ssc;
                Execute(CanExecute(true));
                }
            
                public event EventHandler CanExecuteChanged { add { } remove { } }

                public bool CanExecute(object parameter)
                {
                    return true;
                }

                public void Execute(object parameter)
                {
                this.Ssc.navigator.Wvm = new WindowViewModel(new ReversiGame(Ssc.Height, Ssc.Width));
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
