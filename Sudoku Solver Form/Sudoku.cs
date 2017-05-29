﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
        private int backTracks;

        public int BackTracks { get { return backTracks; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InputBord">2 dimentionale array met ingevulde getallen door de gebruiker.</param>
        /// <returns>Returned het opgeloste en ingevulde bord.</returns>
        public int[,] SolveBoard(int[,] InputBord)
        {
            inputBord = InputBord;
            backTracks = 0;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    bord[x, y] = InputBord[x, y];
                }
            }

            if (Solve(0, 0))
            {
                return bord;
            }
            else
            {
                throw new Exception("No solve boi");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool Solve(int x, int y)
        {
            for (int value = 1; value <= 9; value++)
            {
                if (IsAllowed(x, y, value))
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
                    if ((nextY == 9) || Solve(nextX, nextY)) return true;
                    bord[x, y] = inputBord[x, y];
                    backTracks++;
                }
            }
            return false;
        }

        /// <summary>
        /// Returned true als de meegegeven value geplaatst mag worden op het punt van de x en y coördinaten, gekeken naar de regels van sudoku.
        /// Anders returned de functie false.
        /// </summary>
        /// <param name="x">De x-positie.</param>
        /// <param name="y">De y-positie.</param>
        /// <param name="value">Getal dat gecontroleerd moet worden op de coördinaten.</param>
        /// <returns>
        /// Returned true of false.
        /// </returns>
        private bool IsAllowed(int x, int y, int value)
        {
            //als hij al ingevuld is return true
            if (bord[x, y] != 0) return true;

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

            
            for (int i = vakXStart; i < vakXStart + 3; i++)
            {
                for (int t = vakYStart; t < vakYStart + 3; t++)
                {
                    if(bord[i,t] == value) return false;
                }
            }

            //x en y as
            for (int i = 0; i <= 8; i++)
            {
                if (bord[x, i] == value || bord[i, y] == value) return false;
            }

            //als het getal aan alle regels voldoet, return true
            return true;
        }


    }
}
