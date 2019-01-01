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
        public List<ReversiGame> OldGames { get; set; }
        public WindowViewModel Parent { get; set; }
        public List<BoardRowViewModel> Rows { get; }
        public Boolean Undoing { get; set; }
        public ICommand Undo { get; private set; }//to handle the button click of undo

        public BoardViewModel(WindowViewModel parent)
        {
            this.Undoing = false;
            this.Parent = parent;
            this.OldGames = new List<ReversiGame>();
            this.Rows = new List<BoardRowViewModel>();
            this.Undo = new UndoCommand(this);
            for (int i = 0; i < ReversiBoard.Height; i++) { Rows.Add(new BoardRowViewModel(this, i)); }//initalize rows
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Refresh()
        {
            foreach (BoardRowViewModel row in Rows) { row.Refresh(); }
        }

        public void SendRefresh(ReversiGame newGame)
        {
            if (!Undoing) { OldGames.Add(this.ReversiGame); }
            Parent.SendRefresh(newGame);
        }

        public class UndoCommand : ICommand
        {
            private BoardViewModel BoardViewModel;

            public UndoCommand(BoardViewModel BoardViewModel)
            {
                this.BoardViewModel = BoardViewModel;
                this.BoardViewModel.PropertyChanged += (sender, e) =>
                {
                    CanExecuteChanged?.Invoke(this, new EventArgs());
                };
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                if (BoardViewModel.OldGames.Count != 0 && !BoardViewModel.Parent.ReversiGame.IsGameOver) { 
                BoardViewModel.Undoing = true;
                BoardViewModel.SendRefresh(BoardViewModel.OldGames[BoardViewModel.OldGames.Count - 1]);
                BoardViewModel.OldGames.RemoveAt(BoardViewModel.OldGames.Count - 1);
                }
            }
        }
    }
}
