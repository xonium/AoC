using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace day18
{
    class Program
    {
        private const int SIZE = 50;
        static void Main(string[] args)
        {
            var fieldMatrix = new char[SIZE][];

            using (StreamReader sr = new StreamReader("../../../input.txt"))
            {
                var inputString = sr.ReadToEnd();
                var initialStateString = inputString.Split(Environment.NewLine);

                var splitted = inputString.Split(Environment.NewLine);
                for (int i = 0; i < splitted.Length; i++)
                {
                    var row = splitted[i].Trim();
                    fieldMatrix[i] = Enumerable.Repeat('.', SIZE).ToArray();

                    var p = 0;
                    foreach (var charInRow in row)
                    {
                        fieldMatrix[i][p] = charInRow;
                        p++;
                    }
                }
            }


            for (int i = 0; i < 1000; i++)
            {
                PrintTheField(fieldMatrix);
                fieldMatrix = GetNewState(fieldMatrix);
                Console.WriteLine(Environment.NewLine + "------------------------------------------------" + Environment.NewLine);
            }

            var rowIndex = 0;
            var t = 0;
            var l = 0;
            foreach (var row in fieldMatrix)
            {
                var charIndex = 0;
                foreach (var charInRow in row)
                {

                    t = t + (charInRow == '|' ? 1 : 0);
                    l = l + (charInRow == '#' ? 1 : 0);
                    charIndex++;
                }

                rowIndex++;
            }
            

            Console.WriteLine($"t:{t} * l:{l} = {t * l}");
            Console.ReadLine();
        }

        private static void PrintTheField(char[][] fieldMatrix)
        {
            foreach (var row in fieldMatrix)
            {
                foreach (var charInRow in row)
                {
                    Console.Write(charInRow);
                }

                Console.Write(Environment.NewLine);
            }

            
        }

        private static char[][] GetNewState(char[][] fieldMatrix)
        {
            var returnValue = new char[SIZE][];

            for (int i = 0; i < returnValue.Length; i++)
            {
                returnValue[i] = Enumerable.Repeat('.', SIZE).ToArray();
            }

            var rowIndex = 0;
            foreach(var row in fieldMatrix)
            {
                var charIndex = 0;
                foreach (var charInRow in row)
                {
                    returnValue[rowIndex][charIndex] = GetFieldStatusFromAdjacentFields(fieldMatrix, rowIndex, charIndex);
                    charIndex++;
                }
                
                rowIndex++;
            }


            return returnValue;
        }

        private static char GetFieldStatusFromAdjacentFields(char[][] fieldMatrix, int rowIndex, int charIndex)
        {
            var currentValue = fieldMatrix[rowIndex][charIndex];
            int t = 0;
            int o = 0;
            int l = 0;

            #region adjecentValues
            //ROW - 1
            if (rowIndex - 1 >= 0)
            {
                if (charIndex - 1 >= 0)
                {
                    o = o + (fieldMatrix[rowIndex - 1][charIndex - 1] == '.' ? 1 : 0);
                    l = l + (fieldMatrix[rowIndex - 1][charIndex - 1] == '#' ? 1 : 0);
                    t = t + (fieldMatrix[rowIndex - 1][charIndex - 1] == '|' ? 1 : 0);
                }

                o = o + (fieldMatrix[rowIndex - 1][charIndex] == '.' ? 1 : 0);
                l = l + (fieldMatrix[rowIndex - 1][charIndex] == '#' ? 1 : 0);
                t = t + (fieldMatrix[rowIndex - 1][charIndex] == '|' ? 1 : 0);

                if (charIndex + 1 < fieldMatrix[rowIndex - 1].Length)
                {
                    o = o + (fieldMatrix[rowIndex - 1][charIndex + 1] == '.' ? 1 : 0);
                    l = l + (fieldMatrix[rowIndex - 1][charIndex + 1] == '#' ? 1 : 0);
                    t = t + (fieldMatrix[rowIndex - 1][charIndex + 1] == '|' ? 1 : 0);
                }
            }


            //ROW + 0
            if (charIndex - 1 >= 0)
            {
                o = o + (fieldMatrix[rowIndex][charIndex - 1] == '.' ? 1 : 0);
                l = l + (fieldMatrix[rowIndex][charIndex - 1] == '#' ? 1 : 0);
                t = t + (fieldMatrix[rowIndex][charIndex - 1] == '|' ? 1 : 0);
            }

            if (charIndex + 1 < fieldMatrix[rowIndex].Length)
            {
                o = o + (fieldMatrix[rowIndex][charIndex + 1] == '.' ? 1 : 0);
                l = l + (fieldMatrix[rowIndex][charIndex + 1] == '#' ? 1 : 0);
                t = t + (fieldMatrix[rowIndex][charIndex + 1] == '|' ? 1 : 0);
            }

            //ROW + 1
                if (rowIndex + 1 < fieldMatrix.Length)
            {
                if (charIndex - 1 >= 0)
                {
                    o = o + (fieldMatrix[rowIndex + 1][charIndex - 1] == '.' ? 1 : 0);
                    l = l + (fieldMatrix[rowIndex + 1][charIndex - 1] == '#' ? 1 : 0);
                    t = t + (fieldMatrix[rowIndex + 1][charIndex - 1] == '|' ? 1 : 0);
                }

                o = o + (fieldMatrix[rowIndex + 1][charIndex] == '.' ? 1 : 0);
                l = l + (fieldMatrix[rowIndex + 1][charIndex] == '#' ? 1 : 0);
                t = t + (fieldMatrix[rowIndex + 1][charIndex] == '|' ? 1 : 0);

                if (charIndex + 1 < fieldMatrix[rowIndex + 1].Length)
                {
                    o = o + (fieldMatrix[rowIndex + 1][charIndex + 1] == '.' ? 1 : 0);
                    l = l + (fieldMatrix[rowIndex + 1][charIndex + 1] == '#' ? 1 : 0);
                    t = t + (fieldMatrix[rowIndex + 1][charIndex + 1] == '|' ? 1 : 0);
                }
            }
            #endregion


            if (currentValue == '.')
            {
                if (t >= 3)
                {
                    return '|';
                }
            }

            if (currentValue == '|')
            {
                if (l >= 3)
                {
                    return '#';
                }
            }

            if (currentValue == '#')
            {
                if (l >= 1 && t >= 1)
                {
                    return '#';
                }

                return '.';
            }

            return currentValue;
        }


    }
}
