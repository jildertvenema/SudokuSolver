using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku_Solver_Form
{
    class SudokuSolver
    {
        private const int size = 9;
        private int[,] bord = new int[size, size];
        private int[,] inputBord = new int[size, size];

        public int[,] SolveBoard(int[,] InputBord)
        {
            inputBord = InputBord;


            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    bord[x, y] = InputBord[x, y];
                }
            }

            if (solve(0, 0))
            {
                return bord;
            }
            else
            {
                throw new Exception("No solve boi");
            }
        }


        private bool solve(int x, int y)
        {
            for (int value = 1; value <= 9; value++)
            {
                if (isAllowed(x, y, value) || bord[x,y] != 0)
                {
                    //invullen
                    if (bord[x, y] == 0)
                    {
                        bord[x, y] = value;
                    }


                    int nextX;
                    int nextY;

                    if (x == 8)
                    {
                        nextX = 0;
                        nextY = y + 1;
                    }
                    else
                    {
                        nextX = x + 1;
                        nextY = y;
                    }
                    if ((nextY == 9) || solve(nextX, nextY)) return true;
                    bord[x, y] = inputBord[x, y];
                }
            }
            return false;
        }


        private bool isAllowed(int x, int y, int value)
        {
            //als hij al ingevuld is mag er geen getal bij
            if (bord[x, y] != 0) return false;

            //kijken of het getal al in het 3x3 vak zit

            int vakXStart = 0;
            int vakYStart = 0;

            //start x krijgen
            if (x <= 2)
            {
                vakXStart = 0;
            }
            else
            {
                if (x <= 5) vakXStart = 3;
                else if (x <= 8) vakXStart = 6;
            }

            //start y krijgen
            if (y <= 2)
            {
                vakYStart = 0;
            }
            else
            {
                if (y <= 5) vakYStart = 3;
                else if (y <= 8) vakYStart = 6;
            }

            //vak vullen met zijn getallen
            List<int> vak = new List<int>();
            for (int i = vakXStart; i < vakXStart + 3; i++)
            {
                for (int t = vakYStart; t < vakYStart + 3; t++)
                {
                    vak.Add(bord[i,t]);
                }
            }

            //als hij er al in zit return false
            foreach(int i in vak)
            {
                if (i == value) return false;
            }

            //kijken of het getal al in de rij zit
            List<int> xGetallen = new List<int>();
            List<int> yGetallen = new List<int>();

            //getallen toevoegen in 2 lijsten
            for (int i = 0; i <= 8; i++)
            {
                xGetallen.Add(bord[x, i]);
                yGetallen.Add(bord[i, y]);
            }

            //return false al hij al in de rij staat
            if (xGetallen.IndexOf(value) != -1 || yGetallen.IndexOf(value) != -1) return false;

            //als het getal aan alle regels voldoet, return true
            return true;
        }


    }
}
