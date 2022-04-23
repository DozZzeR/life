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
        private Graphics graphics;
        private int resolution;
        private GameEngine engine;
        public Form1()
        {
            InitializeComponent();
        }

        private void StartGame() 
        {
            if (timer1.Enabled)
                return;

            nudDensity.Enabled = false;
            nudResolution.Enabled = false;
            resolution = (int)nudResolution.Value;

            engine = new GameEngine(
                rows: pictureBox1.Height / resolution,
                cols: pictureBox1.Width / resolution,
                density: (int)nudDensity.Minimum + (int)nudDensity.Maximum - (int)nudDensity.Value
                );

            Text = $"Generation: {engine.currentGeneration}";

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

        private void DrawNextGeneration() 
        {
            graphics.Clear(Color.Black);
            bool[,] field = engine.GetCurrentGeneration();

            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y])
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution - 1, resolution - 1);
                }
            }
            pictureBox1.Refresh();
            Text = $"Generation: {engine.currentGeneration}";
            engine.NextGeneration();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawNextGeneration();
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
            if (!timer1.Enabled)
                return;

            int x = e.Location.X / resolution;
            int y = e.Location.Y / resolution;

            if (e.Button == MouseButtons.Left)
                engine.AddCell(x, y);
            if (e.Button == MouseButtons.Right)
                engine.RemoveCell(x, y);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = $"Generation: 0";
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
