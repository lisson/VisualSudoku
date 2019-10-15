using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ReactiveUI;

namespace SudokuVisual.Model
{
    class Sudoku : ReactiveObject
    {
        private int[,] _board;
        private int _boardIndex;

        public int[,] Board
        {
            get { return _board; }
            set { _board = value; }
        }

        public int BoardIndex
        {
            get { return _boardIndex; }
            set { this.RaiseAndSetIfChanged(ref _boardIndex, value);  }
        }

        public Sudoku()
        {
            Board = new int[9,9];
        }

        public void SetBoard(int r, int c, int value)
        {
            _board[r,c] = value;
            BoardIndex = r * 9 + c;
            Debug.Write(String.Format("Setting {0}, {1} to {2}", r, c, value));
        }

        public void Solve()
        {
            Stack<Cell> st = new Stack<Cell>();
            int row = 0;
            int col = 0;
            Cell cell;
            int val;
            while (true)
            {
                FindNextEmptyCell(out row, out col);
                if (row == -1 && col == -1)
                {
                    //No more empty cells
                    return;
                }
                if (_board[row,col] != 0)
                {
                    continue;
                }
                var candidates = ValidValues(row, col);
                if (candidates.Count == 0)
                {
                    if (st.Count == 0)
                    {
                        // this board has no solution
                        return;
                    }
                    // this cell has no solution, backtrack
                    while (st.Count > 0)
                    {
                        cell = st.Pop();
                        if (cell.Candidates.Count == 0)
                        {
                            SetBoard(cell.Row, cell.Col, 0);
                            continue;
                        }
                        val = cell.Candidates.FirstOrDefault();
                        SetBoard(cell.Row, cell.Col, val);
                        cell.Candidates.RemoveAt(0);
                        st.Push(cell);
                        break;
                    }
                    continue;
                }
                cell = new Cell(row, col);
                cell.Candidates = candidates;
                val = cell.Candidates.FirstOrDefault();
                SetBoard(cell.Row, cell.Col, val);
                cell.Candidates.RemoveAt(0);
                st.Push(cell);
            }
        }

        public void FindNextEmptyCell(out int row, out int col)
        {
            int r = 0;
            int c = 0;
            for (int i = 0; i < 81; i++)
            {
                r = i / 9;
                c = i % 9;
                if (_board[r,c] == 0)
                {
                    row = r;
                    col = c;
                    return;
                }
            }
            row = -1;
            col = -1;
        }

        public List<int> ValidValues(int row, int column)
        {
            bool[] vals = new bool[9];

            for (int i = 0; i < 9; i++)
            {
                if (_board[row,i] == 0)
                {
                    continue;
                }
                int val = _board[row,i] - 1;
                vals[val] = true;
            }

            for (int i = 0; i < 9; i++)
            {
                if (_board[i,column] == 0)
                {
                    continue;
                }
                int val = _board[i,column]- 1;
                vals[val] = true;
            }

            int startRow = (int)(row / 3) * 3;
            int startCol = (int)(column / 3) * 3;
            for (int i = 0; i < 9; i++)
            {
                int r = startRow + (int)(i / 3);
                int c = startCol + i % 3;
                if (_board[r,c] == 0)
                {
                    continue;
                }
                int val = _board[r,c] - 1;
                vals[val] = true;
            }
            List<int> set = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                if (vals[i] == false)
                {
                    set.Add(i + 1);
                }
            }
            return set;
        }

        public bool IsValidSudoku()
        {
            bool[] row = new bool[9];
            bool[,] columns = new bool[9, 9];
            bool[,] blocks = new bool[9, 9];
            for (int i = 0; i < _board.Length; i++)
            {
                for (int j = 0; j < _board.GetLength(0); j++)
                {
                    if (_board[i,j] == 0)
                    {
                        continue;
                    }
                    int val = _board[i,j] - 1;
                    if (columns[j, val] == true)
                    {
                        return false;
                    }
                    columns[j, val] = true;
                    if (row[val] == true)
                    {
                        return false;
                    }
                    row[val] = true;
                    int block = (int)(i / 3) * 3 + (int)(j / 3);
                    if (blocks[block, val] == true)
                    {
                        return false;
                    }
                    blocks[block, val] = true;
                }
                row = new bool[9];
            }
            return true;
        }
    }
}
