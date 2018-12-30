using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Reversi;

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
            get { return ReversiGame.Board.Height* ReversiGame.Board.Width; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
