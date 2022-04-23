using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life
{
    public partial class Form1 : Form
    {
        private int currentGeneration;
        private Graphics graphics;
        private int resolution;
        private bool[,] field;
        private int rows;
        private int cols;
        public Form1()
        {
            InitializeComponent();
        }

        private void StartGame() 
        {
            if (timer1.Enabled)
                return;

            currentGeneration = 0;
            Text = $"Generation: {currentGeneration}";

            nudDensity.Enabled = false;
            nudResolution.Enabled = false;
            resolution = (int)nudResolution.Value;
            rows = pictureBox1.Height / resolution;
            cols = pictureBox1.Width / resolution;
            field = new bool[cols, rows];

            Random rnd = new Random();
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = rnd.Next((int)nudDensity.Value) == 0;
                }
            }

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }

        private void StopGame()
        {
            if (!timer1.Enabled)
                return;
            timer1.Stop();
            nudDensity.Enabled = true;
            nudResolution.Enabled = true;
        }

        private void NextGeneration() 
        {
            graphics.Clear(Color.Black);

            var newField = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neighboursCount = CountNeightbours(x, y);
                    var hasLife = field[x, y];

                    if (!hasLife && neighboursCount == 3)
                        newField[x, y] = true;
                    else if (hasLife && (neighboursCount < 2 || neighboursCount > 3))
                        newField[x, y] = false;
                    else 
                        newField[x, y] = field[x, y];

                    if (hasLife)
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution, resolution);
                }
            }
            field = newField;
            pictureBox1.Refresh();
            Text = $"Generation: {++currentGeneration}";
        }

        private int CountNeightbours(int x, int y)
        {
            int count = 0;
            for (int i = -1; i < 2; i++) 
            {
                for (int j = -1; j < 2; j++) {
                    int col = (x + i + cols) % cols;
                    int row = (y + j + rows) % rows;
                    bool isSelf = col == x && row == y;
                    bool hasLife = field[col, row];
                    if (hasLife && !isSelf) { 
                        count++;
                    }
                }
            }
            return count;
        }

        private void AddRemoveRectangle(MouseEventArgs e)
        {
            if (!timer1.Enabled)
                return;
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                int x = e.Location.X / resolution;
                int y = e.Location.Y / resolution;
                if (ValidateMousePosition(x, y))
                    field[x, y] = e.Button == MouseButtons.Left;
            }
        }

        private bool ValidateMousePosition(int x, int y)
        { 
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            AddRemoveRectangle(e);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            AddRemoveRectangle(e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = $"Generation: {currentGeneration}";
            timer1.Interval = 200 / trackBar1.Value;
            labelSpeed.Text = $"Speed: {trackBar1.Value}";
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Interval = 200 / trackBar1.Value;
            labelSpeed.Text = $"Speed: {trackBar1.Value}";
            timer1.Start();
        }
    }
}
