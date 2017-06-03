using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver_Form
{
    abstract class BoardSolver
    {
        public string BoardName;
        public int BoardSize;
        public abstract int[,] SolveBoard(int[,] board);
    }
}
