using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Reversi;
using System.Windows;

namespace ViewModel
{
    public class GameSettingsViewModel : INotifyPropertyChanged
    {
        public ReversiGame ReversiGame { get { return Parent.ReversiGame; } }
        public WindowViewModel Parent { get; set; }

        public GameSettingsViewModel(WindowViewModel parent)
        {
            this.Parent = parent;
            this.CurrentPlayer = ReversiGame.CurrentPlayer;
            this.WhitePoints = ReversiGame.Board.CountStones(Player.WHITE);
            this.BlackPoints = ReversiGame.Board.CountStones(Player.BLACK);
            this.IsGameOver = "";
            this.WindowHeight = 500;
            this.WindowWidth = 750;
        }

        public void SendRefresh(ReversiGame newGame)
        {
            Parent.SendRefresh(newGame);
        }

        public void Refresh()
        {
            CurrentPlayer = ReversiGame.CurrentPlayer;
            WhitePoints = ReversiGame.Board.CountStones(Player.WHITE);
            BlackPoints = ReversiGame.Board.CountStones(Player.BLACK);
            IsGameOver = "";
            Winner = null;
        }

        private Player winner;
        public Player Winner
        {
            get { return winner; }
            private set
            {
                if (ReversiGame.IsGameOver)
                {
                    if(ReversiGame.Board.CountStones(Player.WHITE) > ReversiGame.Board.CountStones(Player.BLACK)) { winner = Player.WHITE; }
                    else if(ReversiGame.Board.CountStones(Player.WHITE) < ReversiGame.Board.CountStones(Player.BLACK)) { winner = Player.BLACK; }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Winner)));
                }
            }
        }

        private String isGameOver;
        public String IsGameOver
        {
            get { return isGameOver; }
            set
            {
                if (ReversiGame.IsGameOver)
                {
                    isGameOver = "Game Over! The winner was: ";
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsGameOver)));
                }
            }
        }

        private Player currentPlayer;
        public Player CurrentPlayer
        {
            get { return currentPlayer;  }
            private set
            {
                currentPlayer = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(currentPlayer)));
            }
        }

        private int windowHeight;
        public int WindowHeight
        {
            get { return windowHeight; }
            set
            {
                windowHeight = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowHeight)));
            }
        }

        private int windowWidth;
        public int WindowWidth
        {
            get { return windowWidth; }
            set
            {
                windowWidth = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowWidth)));
            }
        }

        private int whitePoints;
        public int WhitePoints
        {
            get { return  ReversiGame.Board.CountStones(Player.WHITE); }
            set
            {
                whitePoints = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(whitePoints)));
            }
        }

        private int blackPoints;
        public int BlackPoints
        {
            get { return ReversiGame.Board.CountStones(Player.BLACK); }
            set
            {
                blackPoints = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(blackPoints)));
            }
        }

        public int MaxPoints
        {
            get { return ReversiGame.Board.Height * ReversiGame.Board.Width; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
