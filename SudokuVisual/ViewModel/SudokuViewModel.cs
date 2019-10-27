using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuVisual.Model;
using ReactiveUI;
using System.Reactive;
using System.Diagnostics;
using System.Collections.ObjectModel;
using SudokuVisual.Class;


namespace SudokuVisual.ViewModel
{
    /*
     * PowerShell script to generate bindings
    $i = 0
foreach($r in (0..8))
{
    foreach($c in (0..8))
    {
        write-host "<TextBox Grid.Column=`"$c`" Grid.Row=`"$r`" Text=`"`{Binding Board[$i], UpdateSourceTrigger=PropertyChanged`}`"></TextBox>"
        $i++  
    }
}
     */
    class SudokuViewModel : ReactiveObject
    {
        private string[] _board;
        private ObservableArray _arr;
        private Sudoku _sudoku;
        public ReactiveCommand<Unit, bool> SolveCommand { get; set; }

        public ObservableArray Board
        {
            get { return _arr; }
            set { this.RaiseAndSetIfChanged(ref _arr, value); }
        }

        public SudokuViewModel()
        {
            _board = new string[81];
            _arr = new ObservableArray(81);
            _sudoku = new Sudoku();
            _sudoku.WhenAnyValue(x => x.BoardIndex).Subscribe(x =>
            {
                int r = x / 9;
                int c = x % 9;
                _arr[x] = _sudoku.Board[r,c] + "";
                this.RaisePropertyChanged("Board");
                Debug.WriteLine("BoardIndex event, raising Board");
            });

            this.WhenAnyValue(x => x.Board.Index).Subscribe(x =>
            {
                int r = x / 9;
                int c = x % 9;
                int val;
                if(Int32.TryParse(_arr[x], out val))
                {
                    if(val < 1 || val > 9)
                    {
                        return;
                    }
                    _sudoku.SetBoard(r, c, val);
                    return;
                }
                _sudoku.SetBoard(r, c, 0);
            });

            SolveCommand = ReactiveCommand.CreateFromTask(() =>
            {
                return Task.Run(() =>
                {
                    _sudoku.Solve();
                    return true;
                });
            });
        }
    }
}
