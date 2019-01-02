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
        }

        public ReversiGame Game { get; set; }
        public ReversiBoard Board { get; set; }
    }

    public class Navigator : INotifyPropertyChanged
    {
        private Screen currentScreen;
        public WindowViewModel Wvm { get; internal set; }
        public PlayerInfo Pi { get; set; }
        internal MainWindow Window { get; private set; }

        public Navigator(MainWindow window)
        {
            Window = window;
            this.Pi = GameInfo.GetPlayerInfo(Player.BLACK);
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

    public class MainScreen : Screen, INotifyPropertyChanged
    {
        public MainScreen(Navigator navigator) : base(navigator)
        {
            GoToSettings = new EasyCommand(() => SwitchTo(new SettingsScreen(navigator)));
            Undo = new UndoCommand(this);
        }


        public WindowViewModel Wvm { get => this.navigator.Wvm; }
        public PlayerInfo Pi { get => this.navigator.Pi; }

        public ICommand GoToSettings { get; }
        public UndoCommand Undo { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public class UndoCommand : ICommand
        {
            private readonly MainScreen Ms;

            public event EventHandler CanExecuteChanged;
            public UndoCommand(MainScreen Ms)
            {
                this.Ms = Ms;
                this.Ms.PropertyChanged += (sender, e) =>
                {
                    CanExecuteChanged?.Invoke(this, new EventArgs());
                };
            }


            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                if (this.Ms.navigator.Wvm.Board.OldGames.Count != 0 && !this.Ms.navigator.Wvm.Board.Parent.ReversiGame.IsGameOver)
                {
                    this.Ms.navigator.Wvm.Board.Undoing = true;
                    this.Ms.navigator.Wvm.Board.SendRefresh(this.Ms.navigator.Wvm.Board.OldGames[this.Ms.navigator.Wvm.Board.OldGames.Count - 1]);
                    this.Ms.navigator.Wvm.Board.OldGames.RemoveAt(this.Ms.navigator.Wvm.Board.OldGames.Count - 1);
                }
            }
        }
    }

    public class SettingsScreen : Screen, INotifyPropertyChanged
    {
        public SettingsScreen(Navigator navigator) : base(navigator)
        {
            StartGame = new StartCommand(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public WindowViewModel Wvm { get => this.navigator.Wvm; }
        public PlayerInfo Pi { get => this.navigator.Pi; }

        public ICommand StartGame { get; }

        private int height = 4;
        public int Height
        {
            get { return height; }
            set
            {
                if (ReversiBoard.IsValidHeight(value))
                {
                    height = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Height)));
                }
            }
        }

        private int width = 4;
        public int Width
        {
            get { return width; }
            set
            {
                if (ReversiBoard.IsValidWidth(value))
                {
                    width = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Width)));
                }
            }
        }

        private String blackName;
        public String BlackName
        {
            get => blackName ?? Pi.Player1;
            set
            {
                blackName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BlackName)));
            }
        }

        private String whiteName;
        public String WhiteName
        {
            get => whiteName ?? Pi.Player2;
            set
            {
                whiteName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WhiteName)));
            }
        }

        private Brush whiteColor;
        public Brush WhiteColor
        {
            get => whiteColor ?? GameInfo.GetPlayerInfo(Player.WHITE).Color2; set
            {
                whiteColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WhiteColor)));
            }
        }

        private Brush blackColor;
        public Brush BlackColor
        {
            get => blackColor ?? GameInfo.GetPlayerInfo(Player.BLACK).Color1; set
            {
                blackColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BlackColor)));
            }
        }

        public class StartCommand : ICommand
        {
            private SettingsScreen Ssc;

            public StartCommand(SettingsScreen Ssc)
            {
                this.Ssc = Ssc;
                Execute(CanExecute(true));
            }

            public event EventHandler CanExecuteChanged { add { } remove { } }

            public bool CanExecute(object parameter)
            {
                return ReversiBoard.IsValidHeight(Ssc.Height) && ReversiBoard.IsValidWidth(Ssc.Width) && Ssc.Height == Ssc.Width && Ssc.Height != 2;
            }

            public void Execute(object parameter)
            {
                GameInfo.GetPlayerInfo(Player.BLACK).Player1 = Ssc.BlackName;
                GameInfo.GetPlayerInfo(Player.WHITE).Player2 = Ssc.WhiteName;
                GameInfo.GetPlayerInfo(Player.BLACK).Color1 = Ssc.BlackColor;
                GameInfo.GetPlayerInfo(Player.WHITE).Color2 = Ssc.WhiteColor;
                this.Ssc.navigator.Wvm = new WindowViewModel(new ReversiGame(Ssc.Height, Ssc.Width));
                this.Ssc.navigator.Window.Height = this.Ssc.Height *30 + 180;
                this.Ssc.SwitchTo(new MainScreen(this.Ssc.navigator));
            }
        }
    }

    public class GameInfo
    {
        private static GameInfo inst;
        public static GameInfo Inst
        {
            get
            {
                if (inst == null)
                {
                    inst = new GameInfo
                    {
                        Pi = new PlayerInfo
                        {
                            Player1 = "Black",
                            Color1 = Brushes.Black,
                            Player2 = "Red",
                            Color2 = Brushes.DarkRed
                        }
                    };
                }
                return inst;
            }
        }

        private PlayerInfo Pi;

        public static PlayerInfo GetPlayerInfo(Player player)
        {
            if (player == Player.BLACK || player == Player.WHITE) return Inst.Pi;
            else return null;
        }


    }

    public class PlayerInfo : INotifyPropertyChanged
    {
        private String player1;
        public String Player1
        {
            get => player1; set
            {
                player1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Player1)));
            }
        }

        private Brush color1;
        public Brush Color1
        {
            get => color1; set
            {
                color1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Color1)));
            }
        }

        private String player2;
        public String Player2
        {
            get => player2; set
            {
                player2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Player2)));
            }
        }

        private Brush color2;
        public Brush Color2
        {
            get => color2; set
            {
                color2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Color2)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
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
