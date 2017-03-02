using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            
            while(Console.ReadKey().Key != ConsoleKey.Q)
            {
                Generator generator = new Generator();

                int[,] m = generator.Generate(4,4,3,5);

                for (int iRow = m.GetLength(0) - 1; iRow >= 0; iRow--)
                {
                    Console.WriteLine();
                    for (int iCol = 0; iCol < m.GetLength(1); iCol++)
                    {
                        Console.Write(m[iRow, iCol] + " ");
                    }
                }
                Console.WriteLine();
            }

        }

      
    }

    public class Generator
    {
        public int[,] Generate(int rows, int cols, int minComponents, int maxComponents)
        {
            _m = new int[rows, cols];
            _minComponents = minComponents;
            _maxComponents = maxComponents;
           

            GenerateMain();
            GenerateGarages();

            return _m;
        
        }

        private void GenerateMain()
        {
            int centerH = _m.GetLength(0) / 2;

            // do the general random shape
            Next(0, centerH, 1);

            // correct the front row
            if (GetCountInRow(0) == 1)
            {
                if (GetRandom() < 0.5f)
                {
                    if (_m[0, centerH - 1] == 0) _m[0, centerH - 1] = 1;
                    else if (_m[0, centerH + 1] == 0) _m[0, centerH + 1] = 1;
                }
                else
                {
                    if (_m[0, centerH + 1] == 0) _m[0, centerH + 1] = 1;
                    else if (_m[0, centerH - 1] == 0) _m[0, centerH - 1] = 1;
                }
            }

            // assign the font entry segment
            _m[0, centerH] = 4; // entry
        }

        private void GenerateGarages()
        {
            // assign where the garage or carport is.
            // It is either on the corner or sticking out on the side in the middle of a side of hte house.

            // if the entre side row is empty then stick the side garage there or a covered carport
            int garagePosition;
            if (GetCountInCol(0) == 0 && GetCountInCol(1) == Height)
            {
                garagePosition =(int)( (float)Height * GetRandom());
                _m[garagePosition, 0] = GetRandom() < 0.5 ? 2 : 3;
            }
            else if (GetCountInCol(Width - 1) == 0 && GetCountInCol(Width - 2) == Height)
            {
                garagePosition = (int)((float)Height * GetRandom());
                _m[garagePosition, Width - 1] = GetRandom() < 0.5 ? 2 : 3;
            }
            else
            {
                // put a garage in the front corner
                if (_m[0, 0] == 0 && _m[0, 1] == 1)
                {
                    _m[0, 0] = (int)((float)Height * GetRandom());
                }
                else if (_m[0, Width - 1] == 0 && _m[0, Width - 2] == 1)
                {
                    _m[0, Width - 1] = (int)((float)Height * GetRandom());
                } 
            }

        }

        int[,] _m;
        int _minComponents;
        int _maxComponents;
        private int Width { get { return _m.GetLength(1); } }
        private int Height { get { return _m.GetLength(0); } }

        private int Count
        {
            get
            {
                int result = 0;
                for (int iRow = _m.GetLength(0) - 1; iRow >= 0; iRow--)
                {
                    for (int iCol = 0; iCol <_m.GetLength(1); iCol++)
                    {
                        if (_m[iRow, iCol] > 0) result++; 
                    }
                }
                return result;
            }
        }

        private int GetCountInRow(int row)
        {
            int result = 0;
                
            for (int iCol = 0; iCol < _m.GetLength(1); iCol++)
            {
                if (_m[row, iCol] > 0) result++;
            }
                
            return result;
            
        }

        private int GetCountInCol(int col)
        {
            int result = 0;

            for (int iRow = 0; iRow < _m.GetLength(0); iRow++)
            {
                if (_m[iRow, col] > 0) result++;
            }

            return result;

        }

        System.Random _random = new System.Random(DateTime.Now.Millisecond);

        private float GetRandom()
        {
            float r = (float)_random.NextDouble();
            //Console.WriteLine(r.ToString("0.0"));
            return r;
        }

        private void Next(int currentRow, int currentCol, int componenetCount)
        {

            _m[currentRow, currentCol] = 1;
            componenetCount++;

            if (Count < _maxComponents && CanGoBack(currentRow, currentCol) && GetRandom() > 0.33f)
            {
                Next(++currentRow, currentCol, componenetCount);
            }
            if (Count < _maxComponents && CanGoLeft(currentRow, currentCol) && GetRandom() > 0.5f)
            { 
                Next(currentRow, --currentCol, componenetCount);
            }
            if (Count < _maxComponents && CanGoRight(currentRow, currentCol) && GetRandom() > 0.5f)
            {
                Next(currentRow, ++currentCol, componenetCount);
            }
            if (Count < _maxComponents && CanGoForward(currentRow, currentCol) && GetRandom() > 0.5f)
            {
                Next(--currentRow, currentCol, componenetCount);
            }
        }

        bool CanGoLeft(int currentRow, int currentCol)
        {
            if (currentCol == 0) return false;
            if (_m[currentRow, currentCol - 1] > 0) return false;
            return true;
        }

        bool CanGoRight(int currentRow, int currentCol)
        {
            if (currentCol == Width - 1) return false;
            if (_m[currentRow, currentCol + 1] > 0) return false;
            return true;
        }

        bool CanGoBack(int currentRow, int currentCol)
        {
            if (currentRow == Height - 1) return false;
            if (_m[currentRow + 1, currentCol] > 0) return false;
            return true;
        }

        bool CanGoForward(int currentRow, int currentCol)
        {
            if (currentRow == 0) return false;
            if (_m[currentRow - 1, currentCol] > 0) return false;
            return true;
        }

    }

}
