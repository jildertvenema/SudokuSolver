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
        private static int size = 9;
        private int[,] indexen = new int[size, size];
        private int[,] bord = new int[size, size];
        private DateTime dt;
        private Random random = new Random();

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

            checkBox1.BackColor = this.BackColor;

            label4.Text = "";
            label3.Text = "";
            label2.Text = "";

            sudokuSolver.ScanComplete += delegate
            {
                string DeltaTime = (DateTime.Now - dt).ToString();
                if (DeltaTime.Length > 15) DeltaTime = DeltaTime.Substring(0, 15);

                checkBox1.BackColor = Color.LightGreen;
                label2.Text = DeltaTime;
            };
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

            sudokuSolver.EnableScan = checkBox1.Checked;

            dt = DateTime.Now;
            bord = sudokuSolver.SolveBoard(bord);
            string DeltaTime = (DateTime.Now - dt).ToString();
            if (DeltaTime.Length > 15) DeltaTime = DeltaTime.Substring(0,15);

            label3.Text = DeltaTime;
            labelBackTracks.Text = sudokuSolver.BackTracks.ToString();
            label4.Text = "Solve Time";

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


        private void RandomPos(int value)
        {
            if (bord[random.Next(0, 8), random.Next(0, 8)] == 0)
            {
                bord[random.Next(0, 8), random.Next(0, 8)] = value;
            }
            else
            {
                RandomPos(value);
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

        private void button1_Click(object sender, EventArgs e)
        {
            foreach(Control c in textBoxes)
            {
                c.Text = "";
            }
            label3.Text = "";
            label4.Text = "";
            label2.Text = "";

            labelBackTracks.Text = "";
            checkBox1.BackColor = this.BackColor;
        }

    }
}
