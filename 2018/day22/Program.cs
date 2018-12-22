using System;
using System.Linq;

namespace Day22
{
    class Program
    {
        private const int DEPTH = 3339;
        private const int TARGET_X = 10;
        private const int TARGET_Y = 715;
        private const int GEO_TIMES_IF_Y_ZERO = 16807;
        private const int GEO_TIMES_IF_X_ZERO = 48271;
        private const int EROSION_MOD = 20183;

        private static int[][] geomatrix { get; set; }
        private static int[][] erosionmatrix { get; set; }
        private static char[][] cavematrix { get; set; }

        static void Main(string[] args)
        {

            InitMatrices();
            CalculateGeoMatrix();
            CalculateCaveMatrix();
            Print();
            var risk = CalculateRisk();
            
            Console.WriteLine("RISKLEVEL: " + risk);

            Console.ReadLine();
        }

        private static void CalculateGeoMatrix()
        {
            var rowIndex = 0;
            var index = 0;
            foreach(var row in geomatrix)
            {
                foreach(var value in row)
                {
                
                    if((rowIndex == 0 && index == 0) || (rowIndex == TARGET_Y && index == TARGET_X))
                    {
                        geomatrix[rowIndex][index] = 0;
                    }
                    else if(rowIndex == 0)
                    {
                        geomatrix[rowIndex][index] = index * GEO_TIMES_IF_Y_ZERO;
                    }
                    else if (index == 0)
                    {
                        geomatrix[rowIndex][index] = rowIndex * GEO_TIMES_IF_X_ZERO;
                    }
                    else
                    {
                        geomatrix[rowIndex][index] = CalculateErosionAtIndex(rowIndex, index - 1) * CalculateErosionAtIndex(rowIndex - 1, index);
                    }

                    SetErosionAtIndex(rowIndex, index, CalculateErosionAtIndex(rowIndex, index));
                    index++;
                }

                index = 0;
                rowIndex++;
            }
        }

        private static int CalculateErosionAtIndex(int rowIndex, int index)
        {
            return (geomatrix[rowIndex][index] + DEPTH) % EROSION_MOD;
        }

        private static void SetErosionAtIndex(int rowIndex, int index, int erosion)
        {
            erosionmatrix[rowIndex][index] = erosion;
        }

        private static void CalculateCaveMatrix()
        {
            var rowIndex = 0;
            var index = 0;
            foreach (var row in cavematrix)
            {
                foreach (var value in row)
                {
                    if ((rowIndex == 0 && index == 0))
                    {
                        cavematrix[rowIndex][index] = 'M';
                    }
                    else if ((rowIndex== TARGET_Y && index== TARGET_X))
                    {
                        cavematrix[rowIndex][index] = 'T';
                    }
                    else if (erosionmatrix[rowIndex][index] % 3 == 0)
                    {
                        cavematrix[rowIndex][index] = '.';
                    }
                    else if(erosionmatrix[rowIndex][index] % 3 == 1)
                    {
                        cavematrix[rowIndex][index] = '=';
                    }
                    else if (erosionmatrix[rowIndex][index] % 3 == 2)
                    {
                        cavematrix[rowIndex][index] = '|';
                    }
                   
                    index++;
                }

                index = 0;
                rowIndex++;
            }
        }


        private static int CalculateRisk()
        {
            var rowIndex = 0;
            var index = 0;
            var risk = 0;
            foreach (var row in cavematrix)
            {
                foreach (var value in row)
                {
                    if (cavematrix[rowIndex][index] == 'M')
                    {
                        if (erosionmatrix[rowIndex][index] % 3 == 0)
                        {
                            risk = risk + 0;
                        }
                        else if (erosionmatrix[rowIndex][index] % 3 == 1)
                        {
                            risk = risk + 1;
                        }
                        else if (erosionmatrix[rowIndex][index] % 3 == 2)
                        {
                            risk = risk + 2;
                        }
                    }
                    else if (cavematrix[rowIndex][index] == 'T')
                    {
                        if (erosionmatrix[rowIndex][index] % 3 == 0)
                        {
                            risk = risk + 0;
                        }
                        else if (erosionmatrix[rowIndex][index] % 3 == 1)
                        {
                            risk = risk + 1;
                        }
                        else if (erosionmatrix[rowIndex][index] % 3 == 2)
                        {
                            risk = risk + 2;
                        }
                    }
                    else if (cavematrix[rowIndex][index] == '.')
                    {
                        risk = risk + 0;
                    }
                    else if (cavematrix[rowIndex][index] == '=')
                    {
                        risk = risk + 1;
                    }
                    else if (cavematrix[rowIndex][index] == '|')
                    {
                        risk = risk + 2;
                    }

                    index++;
                }

                index = 0;
                rowIndex++;
            }

            return risk;
        }

        private static void Print()
        {
            var rowIndex = 0;
            var index = 0;
            foreach (var row in cavematrix)
            {
                foreach (var value in row)
                {                        
                    Console.Write(cavematrix[rowIndex][index]);
                    index++;
                }

                Console.WriteLine();
                index = 0;
                rowIndex++;
            }
        }

        private static void InitMatrices()
        {
            geomatrix = new int[TARGET_Y+1][];
            for (int i = 0; i < geomatrix.Length; i++)
            {
                geomatrix[i] = Enumerable.Repeat(0, TARGET_X+1).ToArray();
            }

            erosionmatrix = new int[TARGET_Y+1][];
            for (int i = 0; i < erosionmatrix.Length; i++)
            {
                erosionmatrix[i] = Enumerable.Repeat(0, TARGET_X+1).ToArray();
            }

            cavematrix = new char[TARGET_Y+1][];
            for (int i = 0; i < cavematrix.Length; i++)
            {
                cavematrix[i] = Enumerable.Repeat('.', TARGET_X+1).ToArray();
            }
        }

    }
}
