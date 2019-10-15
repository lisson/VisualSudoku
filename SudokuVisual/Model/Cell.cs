using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuVisual.Model
{
    class Cell
    {
        public int Row { get; }

        public int Col { get; }

        public List<int> Candidates { get; set; }

        public Cell(int r, int c)
        {
            Row = r;
            Col = c;
        }
    }
}
