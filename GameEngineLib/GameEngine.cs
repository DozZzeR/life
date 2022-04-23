using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineLib
{
    public class GameEngine
    {
        public uint currentGeneration { get; private set; }

        private bool[,] field;
        private readonly int rows;
        private readonly int cols;

        public GameEngine(int rows, int cols, int density)
        {
            this.rows = rows;
            this.cols = cols;
            field = new bool[cols, rows];

            Random rnd = new Random();
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = rnd.Next(density) == 0;
                }
            }
        }

        private int CountNeightbours(int x, int y)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int col = (x + i + cols) % cols;
                    int row = (y + j + rows) % rows;
                    bool isSelf = col == x && row == y;
                    bool hasLife = field[col, row];
                    if (hasLife && !isSelf)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public void NextGeneration()
        {
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
                }
            }
            field = newField;
            currentGeneration++;
        }

        public bool[,] GetCurrentGeneration()
        {
            bool[,] res = new bool[cols, rows];
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    res[x, y] = field[x, y];
                }
            }
            return res;
        }

        private bool ValidateCellPosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }

        private void UptdateCell(int x, int y, bool state)
        {
            if (ValidateCellPosition(x, y))
            {
                field[x, y] = state;
            }
        }

        public void AddCell(int x, int y)
        {
            UptdateCell(x, y, state: true);
        }

        public void RemoveCell(int x, int y)
        {
            UptdateCell(x, y, state: false);
        }
    }
}
