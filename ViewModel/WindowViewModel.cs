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
        public ReversiGame ReversiGame { get; set; }
        public BoardViewModel Board { get; }
        public GameSettingsViewModel GameSettings { get; }

        public WindowViewModel(ReversiGame game)
        {
            this.ReversiGame = game;
            this.Board = new BoardViewModel(this);
            this.GameSettings = new GameSettingsViewModel(this); 
        }

        public void SendRefresh(ReversiGame newGame)
        {
            this.ReversiGame = newGame;
            Board.Refresh();
            GameSettings.Refresh();
        }
    }
}
