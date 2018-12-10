using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader("../../../input.txt"))
            {
                var inputString = sr.ReadToEnd();
                var inputValues = inputString.Split(Environment.NewLine)
                                    .Where(y => y != string.Empty)
                                    .Select(x => int.Parse(x));

                var resultingFrequency = 0;
                var frequencyHistory = new List<int>();
                var foundDuplicateFrequency = false;
                var frequencyDuplicate = 0;

                foreach(var inputValue in inputValues)
                {
                    resultingFrequency += inputValue;
                }

                Console.WriteLine($"Part 1: {resultingFrequency}");

                resultingFrequency = 0;
                while (!foundDuplicateFrequency)
                {
                    foreach (var inputValue in inputValues)
                    {
                        resultingFrequency += inputValue;

                        if(!frequencyHistory.Contains(resultingFrequency))
                        {
                            frequencyHistory.Add(resultingFrequency);
                        }
                        else
                        {
                            foundDuplicateFrequency = true;
                            frequencyDuplicate = resultingFrequency;
                            break;
                        }
                    }
                }

                Console.WriteLine($"Part 2: {frequencyDuplicate}");
            }

            Console.ReadLine();
        }
    }
}
