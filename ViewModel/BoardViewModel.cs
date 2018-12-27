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
        public ReversiBoard ReversiBoard { get; set; }
        public ReversiGame ReversiGame { get; set; }

        public BoardViewModel(ReversiBoard board, ReversiGame game)
        {
            this.ReversiBoard = board;
            this.ReversiGame = game;
            this.Rows = new List<BoardRowViewModel>();
            for (int i = 0; i < ReversiBoard.Height; i++) { Rows.Add(new BoardRowViewModel(this, i)); }//initalize rows
        }

        public List<BoardRowViewModel> Rows { get; }

        public void Refresh()
        {
            foreach (BoardRowViewModel row in Rows) { row.Refresh(); }
        }

        public void SendRefresh(ReversiGame newGame)
        {
            this.ReversiGame = newGame;
            this.ReversiBoard = newGame.Board;
            this.Refresh();
        }
    }
}
