using System;

namespace Sudoku_Solver_Form
{
    class SudokuSolver
    {
        private const int size = 9;
        private int[,] bord;
        private int[,] inputBord;
        private NietMogelijk[,] nietMogelijk;
        private int maxBackTracks = 20000000;

        private int backTracks;

        public int BackTracks { get { return backTracks; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InputBord">2 dimentionale array met ingevulde getallen door de gebruiker.</param>
        /// <returns>Returned het opgeloste en ingevulde bord.</returns>
        public int[,] SolveBoard(int[,] InputBord)
        {
            bord = new int[size, size];
            inputBord = InputBord;
            backTracks = 0;
            nietMogelijk = new NietMogelijk[size, size];

            //bord vullen met input getallen
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    nietMogelijk[x, y] = new NietMogelijk();
                    bord[x, y] = InputBord[x, y];
                }
            }

            while (ScanSudoku())
            {
                //zolang er gescanned kan worden, doe het
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

        private bool ScanSudoku()
        {
            bool succes = false;
            //Niet mogelijke getallen toevoegen
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    if (bord[x,y] != 0)
                    {
                        nietMogelijk[x, y].NietsMogelijk();
                        continue;
                    }

                    for(int value = 1; value <= 9; value++)
                    {
                        if (!IsAllowed(x,y,value) && !nietMogelijk[x, y].Numbers.Contains(value))
                        {
                            nietMogelijk[x, y].Numbers.Add(value);
                            succes = true;
                        }
                    }
                }
            }

            //kijken welke getallen we nu al weten
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
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

                    bool place;

                    for (int value = 1; value <= 9; value++)
                    {
                        //niet mogelijk op zichzelf, continue
                        if (nietMogelijk[x, y].Numbers.Contains(value)) continue;
                        //anders kijken we of ergens anders in het vak het nog mogelijk is

                        place = true;
                        for (int i = vakXStart; i < vakXStart + 3; i++)
                        {
                            for (int t = vakYStart; t < vakYStart + 3; t++)
                            {
                                //niet zichzelf meerekenen met mogelijkheid
                                if (i == x && t == y) continue;
                                if (!nietMogelijk[i, t].Numbers.Contains(value))
                                {
                                    place = false;
                                }
                            }
                        }

                        //als hij nergens anders in x of y as mag, plaats hem alsnog
                        if (!place)
                        {
                            bool allowedInXofY = false;
                            for (int i = 0; i <= 8; i++)
                            {
                                if (!nietMogelijk[x, i].Numbers.Contains(value) || !nietMogelijk[i, y].Numbers.Contains(value))
                                {
                                    allowedInXofY = true;
                                }
                            }
                            if (!allowedInXofY) place = true;
                        }


                        //we weten zeker dat het value moet zijn
                        if (place)
                        {
                            succes = true;
                            bord[x, y] = value;
                            inputBord[x, y] = value;
                            nietMogelijk[x, y].NietsMogelijk();
                        }

                    }

                }
            }

            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    if (nietMogelijk[x,y].Numbers.Count == 8)
                    {
                        for (int value = 1; value <=9; value++)
                        {
                            if (!nietMogelijk[x, y].Numbers.Contains(value))
                            {
                                succes = true;
                                bord[x, y] = value;
                                inputBord[x, y] = value;
                                nietMogelijk[x, y].NietsMogelijk();
                            }
                        }
                    }
                }
            }

            //rule 4
            for (int x = 0; x < 9; x++)
            {
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

                for (int y = 0; y < 9; y++)
                {
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

                    for (int value = 1; value <= 9; value++)
                    {
                        bool valueAlleenInRow = true;

                        //als getal mogelijk is
                        if (!nietMogelijk[x, y].Numbers.Contains(value))
                        {
                            //en niet in de andere rows zit
                            for (int i = vakXStart; i < vakXStart + 3; i++)
                            {
                                //zelfde rij, continue
                                if (i == x) continue;

                                for (int t = vakYStart; t < vakYStart + 3; t++)
                                {
                                    // als hij wel mogelijk is, false
                                    if (!nietMogelijk[i, t].Numbers.Contains(value))
                                    {
                                        valueAlleenInRow = false;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            valueAlleenInRow = false;
                        }

                        if (valueAlleenInRow)
                        {
                            //value kan alleen in deze rij
                            for (int rowY = 0; rowY <= 8; rowY++)
                            {
                                //eigen row skippen
                                if (rowY == vakYStart) rowY += 3;
                                if (rowY > 8) break;
                                if (!nietMogelijk[x, rowY].Numbers.Contains(value))
                                {
                                    nietMogelijk[x, rowY].Numbers.Add(value);
                                    succes = true;
                                }

                            }
                        }
                    }
                }
            }


            //true als er een nieuw getal gevonden is
            return succes;

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
                    if (backTracks > maxBackTracks || (nextY == 9) || Solve(nextX, nextY)) return true;
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
