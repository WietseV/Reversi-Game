using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Model.Reversi;

namespace ViewModel
{
    public class BoardViewModel : INotifyPropertyChanged

    {
        public ReversiBoard ReversiBoard { get { return ReversiGame.Board; } }
        public ReversiGame ReversiGame { get { return Parent.ReversiGame; } }
        public WindowViewModel Parent { get; set; }
        public List<BoardRowViewModel> Rows { get; }
        public Boolean Undoing { get; set; }

        public BoardViewModel(WindowViewModel parent)
        {
            this.Undoing = false;
            this.Parent = parent;
            this.OldGames = new List<ReversiGame>();
            this.Rows = new List<BoardRowViewModel>();
            for (int i = 0; i < ReversiBoard.Height; i++) { Rows.Add(new BoardRowViewModel(this, i)); }//initalize rows
        }

        public void Refresh()
        {
            foreach (BoardRowViewModel row in Rows) { row.Refresh(); }
        }

        public void SendRefresh(ReversiGame newGame)
        {
            if (!Undoing) { OldGames.Add(this.ReversiGame); }
            Parent.SendRefresh(newGame);
        }

        private List<ReversiGame> oldGames;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<ReversiGame> OldGames
        {
            get { return oldGames; }
            set
            {
                oldGames = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OldGames)));
            }
        }

    }
}
