using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuVisual.Model;
using ReactiveUI;
using System.Reactive;


namespace SudokuVisual.ViewModel
{
    class SudokuViewModel : ReactiveObject
    {
        private int[] _board;
        private Sudoku _sudoku;
        public ReactiveCommand<Unit,Unit> SolveCommand { get; set; }

        public int[] Board
        {
            get { return _board; }
            set { _board = value; }
        }

        public SudokuViewModel()
        {
            _board = new int[81];
            _sudoku = new Sudoku();
            _sudoku.WhenAnyValue(x => x.BoardIndex).Subscribe(x =>
            {
                int r = x / 9;
                int c = x % 9;
                _board[x] = _sudoku.Board[r,c];
                this.RaisePropertyChanged("Board");
            });
            SolveCommand = ReactiveCommand.Create(() =>
            {
                _sudoku.Solve();
            });
        }
    }
}
