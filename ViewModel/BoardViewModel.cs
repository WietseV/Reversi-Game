using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Reversi;

namespace ViewModel
{
    public class BoardViewModel
    {
        public ReversiBoard ReversiBoard { get { return ReversiGame.Board; } }
        public ReversiGame ReversiGame { get { return Parent.ReversiGame; } }
        public WindowViewModel Parent { get; set; }
        public List<BoardRowViewModel> Rows { get; }

        public BoardViewModel(WindowViewModel parent)
        {
            this.Parent = parent;
            this.Rows = new List<BoardRowViewModel>();
            for (int i = 0; i < ReversiBoard.Height; i++) { Rows.Add(new BoardRowViewModel(this, i)); }//initalize rows
        }

        public void Refresh()
        {
            foreach (BoardRowViewModel row in Rows) { row.Refresh(); }
        }

        public void SendRefresh(ReversiGame newGame)
        {
            Parent.SendRefresh(newGame);
        }
    }
}
