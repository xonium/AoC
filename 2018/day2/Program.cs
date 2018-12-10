using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day2
{
    class Program
    {
        private static IEnumerable<string> inputValues;

        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader("../../../input.txt"))
            {
                var inputString = sr.ReadToEnd();
                inputValues = inputString.Split(Environment.NewLine)
                                    .Where(y => y != string.Empty)
                                    .Select(x => x.Trim());
            }

            var alpha = "abcdefghijklmnopqrstuvwxyz";
            var numberOfBoxesWithTwo = 0;
            var numberOfBoxesWithThree = 0;

            foreach (var inputValue in inputValues)
            {
                var foundTwo = false;
                var foundThree = false;
                foreach (var letter in alpha)
                {
                    var result = inputValue.Count(x => x == letter);
                    if (result == 2 && !foundTwo)
                    {
                        numberOfBoxesWithTwo++;
                        foundTwo = true;
                    }
                    else if (result == 3 && !foundThree)
                    {
                        numberOfBoxesWithThree++;
                        foundThree = true;
                    }
                }
            }

            Console.WriteLine($"Part 1: {numberOfBoxesWithTwo} * {numberOfBoxesWithThree} = {numberOfBoxesWithTwo * numberOfBoxesWithThree}");

            var inputArray = inputValues.ToArray();

            for (int i=0; i < inputArray.Length; i++)
            {  
                for (int p = 0; p < inputArray.Length; p++)
                {
                    if (i == p) continue;

                    int atPos = 0;
                    int numberOfErrors = 0;
                    int lastPosOfError = 0;
                    foreach(var charInFirst in inputArray[i])
                    {
                        if (charInFirst != inputArray[p][atPos])
                        {
                            numberOfErrors++;
                            lastPosOfError = atPos;
                            if (numberOfErrors > 1)
                            {
                                break;
                            }
                        }

                        atPos++;
                    }

                    if (numberOfErrors == 1)
                    {
                        var remaining = inputArray[i].Remove(lastPosOfError, 1);
                        Console.WriteLine($"Part 2: {remaining}");
                    }
                }
            }

            Console.ReadLine();
        }

    }
}
