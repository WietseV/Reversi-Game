using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Reversi;

namespace ViewModel
{
    public class BoardRowViewModel
    {
        public ReversiBoard ReversiBoard { get { return ReversiGame.Board; } }
        public ReversiGame ReversiGame { get { return Parent.ReversiGame; } }
        public int Index { get; private set; }//row index
        public BoardViewModel Parent { get; set; }

        public BoardRowViewModel(BoardViewModel parent, int index)
        {
            this.Parent = parent;
            this.Index = index;
            this.Squares = new List<BoardSquareViewModel>();
            for (int i = 0; i < ReversiBoard.Width; i++) { Squares.Add(new BoardSquareViewModel(this, i)); }//initialise columns
        }

        public List<BoardSquareViewModel> Squares { get; }

        public void Refresh()
        {
            foreach (BoardSquareViewModel square in Squares) square.Refresh();
        }

        public void SendRefresh(ReversiGame newGame)
        {
            Parent.SendRefresh(newGame);
        }

    }
}
