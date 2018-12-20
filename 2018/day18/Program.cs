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

                fieldMatrix = GetNewState(fieldMatrix);

                //PrintTheField(fieldMatrix);
                //Console.WriteLine(Environment.NewLine + "------------------------------------------------" + Environment.NewLine);
                //CountObjects(fieldMatrix, i);
                //Console.WriteLine(Environment.NewLine + "------------------------------------------------" + Environment.NewLine);

                CountObjects(fieldMatrix, i);
                //Console.Clear();
            }

            Console.ReadLine();

            var results = new List<int>() { 176900 , 183084, 189630, 197938, 205737, 216216, 215877,
                215096, 215160, 217728, 217672, 219726, 214878, 189088,
                191540, 199593, 199064,199283 ,186550,182252,176468,174028, 170016, 167445, 161214, 164666, 165599,171970 };
            var counter = 0;
            var theAnswer = 0;
            for (long i = 423; i <= 1000000000; i++)
            {
                if (counter >= results.Count)
                {
                    counter = 0;
                }

                counter++;
            }

            Console.WriteLine("THE ANSWER:" + results[counter]);
            Console.ReadLine();
        }

        private static void CountObjects(char[][] fieldMatrix, int index)
        {
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
            
            Console.WriteLine($"t:{t} * l:{l} = {t * l} index:{index}");
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
