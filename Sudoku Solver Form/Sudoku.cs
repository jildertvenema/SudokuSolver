using System;

namespace Sudoku_Solver_Form
{
    /// <summary>
    /// Een sudoku oplosser, die sudoku's kan oplossen met behulp van algoritmes, backtracking en recursie.
    /// </summary>
    class SudokuSolver : BoardSolver
    {
        private const int size = 9;
        private int[,] bord;
        private int[,] inputBord;
        private NietMogelijk[,] nietMogelijk;
        private int maxBackTracks;
        private bool enableScan;
        private int backTracks;
        private int scanCount;

        public event EventHandler ScanComplete;
        public int BackTracks { get { return backTracks; } }
        public bool EnableScan { get { return enableScan; }set { enableScan = value; } }


        /// <summary>
        /// Een sudoku oplosser, die sudoku's kan oplossen met behulp van algoritmes, backtracking en recursie.
        /// </summary>
        /// <param name="MaxBackTracks">Maximum aantal backtracks, standaard is het maximum integer nummer</param>
        /// <param name="EnableScan">Met EnableScan zal het proces veel sneller gaan, gaat getallen invullen voor het backtracken</param>
        public SudokuSolver(int MaxBackTracks = int.MaxValue, bool EnableScan = true)
        {
            BoardSize = size;
            BoardName = "Sudoku";

            maxBackTracks = MaxBackTracks;
            enableScan = EnableScan;
        }

        /// <summary>
        /// Een sudoku bord oplossen.
        /// </summary>
        /// <param name="InputBord">2 dimentionale array met ingevulde getallen door de gebruiker.</param>
        /// <returns>Returned het opgeloste en ingevulde bord.</returns>
        public override int[,] SolveBoard(int[,] InputBord)
        {
            //aantal variabelen op 0 zetten of decladeren
            bord = new int[BoardSize, BoardSize];
            inputBord = InputBord;
            backTracks = 0;
            nietMogelijk = new NietMogelijk[BoardSize, BoardSize];
            scanCount = 0;

            //bord vullen met gegeven getallen van InputBord
            for (int x = 0; x < BoardSize; x++)
            {
                for (int y = 0; y < BoardSize; y++)
                {
                    nietMogelijk[x, y] = new NietMogelijk();
                    bord[x, y] = InputBord[x, y];
                }
            }


            //de scan gaat kijken of hij al al getallen in kan vullen, en zolang dat lukt, gaat hij door
            //dit is als voorbereiding van het backtracken, zo zal het proces veel sneller gaan
            if (enableScan) ScanSudoku();

            //we solven het bord, beginnen op positie 0,0
            //als het gelukt is, return het bord, anders trow Exception
            if (Solve(0, 0))
            {
                return bord;
            }
            else
            {
                throw new Exception("There is no possible solution for the given sudoku board");
            }
        }

        /// <summary>
        /// Kijkt of er getallen ingevuld kunnen worden voor het backtracking procces begint, kijkende naar de sudoku trucjes/regels.
        /// Roept zichzelf recursief aan, totdat er geen verandering meer kan worden gedaan.
        /// </summary>
        private void ScanSudoku()
        {
            scanCount++;

            //Niet mogelijke getallen toevoegen, door voor elke positie, alle 9 values proberen of ze "safe" zijn met IsAllowed() methode
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
                        }
                    }
                }
            }


            //kijken welke getallen we nu al weten, door te kijken of er maar 1 positie is in een row/3x3, waar een bepaald getal mogelijk is
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
                            bord[x, y] = value;
                            inputBord[x, y] = value;
                            nietMogelijk[x, y].NietsMogelijk();
                            ScanSudoku();
                            return;
                        }

                    }

                }
            }

            //hier wordt er gekeken of er ergens op een vakje/x-as/y-as 8 getallen niet mogelijk zijn, wat betekend dat er maar 1 mogelijk is
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
                                bord[x, y] = value;
                                inputBord[x, y] = value;
                                nietMogelijk[x, y].NietsMogelijk();
                                ScanSudoku();
                                return;
                            }
                        }
                    }
                }
            }

            //rule 4 op de x-as
            //rule 4 is kijken of er in 1 x of y positie een bepaald getal moet staan, wat betekend dat er ergens anders op die x of y positie
            //dat zelfde getal niet mag staan
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
                                    ScanSudoku();
                                    return;
                                }

                            }
                        }
                    }
                }
            }

            //rule 4 op de y-as
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
                                for (int t = vakYStart; t < vakYStart + 3; t++)
                                {
                                    //zelfde rij, continue
                                    if (t == y) continue;

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
                            for (int rowX = 0; rowX <= 8; rowX++)
                            {
                                //eigen row skippen
                                if (rowX == vakXStart) rowX += 3;
                                if (rowX > 8) break;
                                if (!nietMogelijk[rowX, y].Numbers.Contains(value))
                                {
                                    nietMogelijk[rowX, y].Numbers.Add(value);
                                    ScanSudoku();
                                    return;
                                }

                            }
                        }
                    }
                }
            }

            //Als we hier zijn gekomen, betekend het dat er geen veranderingen zijn gedaan in deze scan.
            //De scan is dus voltooid.
            //ScanComplete event aanroepen zodat andere classes weten dat de scan klaar is, en je daar feedback voor kan geven aan de user.
            ScanComplete.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// De functie die het bord oplost, kijkt of elke value mogelijk is op een x en y positie, en plaatst deze.
        /// </summary>
        /// <param name="x">x-positie</param>
        /// <param name="y">y-positie</param>
        /// <returns>Oplossing gevonden</returns>
        private bool Solve(int x, int y)
        {
            for (int value = 1; value <= 9; value++)
            {
                if (IsAllowed(x, y, value))
                {
                    //als hij nog niet is ingevuld, vul hem in.
                    if (bord[x, y] == 0)
                    {
                        bord[x, y] = value;
                    }

                    //Volgende positie opzoeken
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

                    //Recursief de volgende positie aanroepen
                    if (backTracks > maxBackTracks || (nextY == 9) || Solve(nextX, nextY)) return true;

                    //Backtracken als we hier zijn gekomen
                    bord[x, y] = inputBord[x, y];
                    backTracks++;
                }
            }

            //geen oplossing mogelijk
            return false;
        }

        /// <summary>
        /// Controleren of een value op de x en y positie mag staan.
        /// </summary>
        /// <param name="x">De x-positie.</param>
        /// <param name="y">De y-positie.</param>
        /// <param name="value">Getal dat gecontroleerd moet worden op de coördinaten.</param>
        /// <returns>
        /// Returned true als het getal op de positie mag staan.
        /// </returns>
        private bool IsAllowed(int x, int y, int value)
        {
            //als hij al ingevuld is return true
            if (bord[x, y] != 0) return true;

            //Als we al weten van de scans of het getal er niet mag staan, return false
            //Bespaard veel tijd
            if (nietMogelijk[x, y].Numbers.IndexOf(value) != -1) return false;

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

            //Als hij er al in staat, return false
            for (int i = vakXStart; i < vakXStart + 3; i++)
            {
                for (int t = vakYStart; t < vakYStart + 3; t++)
                {
                    if(bord[i,t] == value) return false;
                }
            }

            //Verticaal en horizontaal kijken of het getal er al staat, zo ja: return false
            for (int i = 0; i <= 8; i++)
            {
                if (bord[x, i] == value || bord[i, y] == value) return false;
            }

            //als het getal aan alle regels voldoet, return true
            return true;
        }


    }
}
