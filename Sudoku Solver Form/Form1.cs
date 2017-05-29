using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku_Solver_Form
{
    public partial class Form1 : Form
    {
        private List<Control> textBoxes = new List<Control>();
        private SudokuSolver sudokuSolver = new SudokuSolver();
        private int[,] indexen = new int[9, 9];
        private int[,] bord = new int[9, 9];

        public Form1()
        {
            InitializeComponent();
            GetAllControl();
            int indexCount = 0;
            for (int x = 0; x <= 8; x++)
            {
                for (int y = 0; y <= 8; y++)
                {
                    indexen[y, x] = indexCount;
                    indexCount++;
                }
            }
        }

        private void solveButton_Click(object sender, EventArgs e)
        {
            int textBoxIndex = 0;
            for (int x = 0; x <= 8; x++)
            {
                for (int y = 0; y <= 8; y++)
                {
                    string txt = textBoxes[textBoxIndex].Text;
                    int.TryParse(txt, out bord[x, y]);
                    textBoxIndex++;
                }
            }

            DateTime dt = DateTime.Now;
            bord = sudokuSolver.SolveBoard(bord);
            richTextBox1.Text = (DateTime.Now.Second - dt.Second).ToString() + "s en " + sudokuSolver.BackTracks.ToString();

            int indexCount = 0;
            for (int x = 0; x <= 8; x++)
            {
                for (int y = 0; y <= 8; y++)
                {
                    textBoxes[indexCount].Text = bord[x, y].ToString();
                    indexCount++;
                }
            }


        }

        private void GetAllControl()
        {
            foreach (Control x in this.Controls)
            {
                if (x is TextBox)
                {
                    textBoxes.Add(x);
                }
            }
        }
    }
}
