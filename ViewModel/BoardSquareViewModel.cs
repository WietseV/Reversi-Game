using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DataStructures;
using Model.Reversi;

namespace ViewModel
{
    public class BoardSquareViewModel : INotifyPropertyChanged
    {
        public Player owner;// get the owner of the tile with location
        public int Index1 { get; private set; }//row index
        public int Index2 { get; private set; }//column index
        public ReversiBoard ReversiBoard { get { return ReversiGame.Board; } }
        public ReversiGame ReversiGame { get { return Parent.ReversiGame; } }
        public List<ReversiGame> OldGames { get; set; }
        public Vector2D Position { get { return new Vector2D(this.Index1, this.Index2); }}
        public ICommand PutStone { get; private set; }//to handle the button click of the stones
        public BoardRowViewModel Parent { get; set; }

        public BoardSquareViewModel(BoardRowViewModel parent, int index)
        {
            this.Parent = parent;
            this.Index1 = parent.Index;
            this.Index2 = index;
            this.Owner = ReversiBoard[Position];
            this.PutStone = new PutStoneCommand(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Player Owner
        {
            get { return owner; }
            set
            {
                owner = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Owner)));
            }
        }

        public void Refresh()
        {
            Owner = ReversiBoard[Position];
        }

        public void SendRefresh(ReversiGame newGame)
        {
            Parent.SendRefresh(newGame);
        }
    }

    public class PutStoneCommand : ICommand
    {
        private BoardSquareViewModel BoardSquareViewModel;

        public PutStoneCommand(BoardSquareViewModel BoardSquareViewModel)
        {
            this.BoardSquareViewModel = BoardSquareViewModel;
            this.BoardSquareViewModel.PropertyChanged += (sender, e) =>
            {
                CanExecuteChanged?.Invoke(this, new EventArgs());
            };
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return BoardSquareViewModel.ReversiGame.IsValidMove(BoardSquareViewModel.Position);
        }

        public void Execute(object parameter)
        {
            BoardSquareViewModel.Parent.Parent.Undoing = false;
                BoardSquareViewModel.SendRefresh(BoardSquareViewModel.ReversiGame.PutStone(BoardSquareViewModel.Position));
        }
    }
}
