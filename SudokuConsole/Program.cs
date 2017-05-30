using Sudoku_Solver_Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuConsole
{
    class Program
    {
        static SudokuSolver s = new SudokuSolver();
        static int[,] sudoku = new int[9,9];
        static DateTime dt;

        static void Main(string[] args)
        {
            sudoku[0, 0] = 1;
            sudoku[0, 2] = 5;
            sudoku[2, 2] = 3;

            dt = DateTime.Now;
            sudoku = s.SolveBoard(sudoku);
            Console.WriteLine(DateTime.Now - dt);
            Console.ReadKey();

            foreach (int i in sudoku)
                Console.WriteLine(i);
            Console.WriteLine(s.backtracks);
            Console.ReadKey();
        }
    }
}
