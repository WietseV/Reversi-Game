using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Reversi;

namespace ViewModel
{
    public class WindowViewModel
    {
        public ReversiBoard ReversiBoard { get; set; }
        public ReversiGame ReversiGame { get; set; }

        public WindowViewModel(ReversiGame game)
        {
            this.ReversiGame = game;
            this.ReversiBoard = this.ReversiGame.Board;
            new BoardViewModel(this); //initalize rows
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
