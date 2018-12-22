using System;
using System.IO;
using System.Text;

namespace day5
{
    class Program
    {
        static char[] InputValue { get; set; }

        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader("../../../input.txt"))
            {
                InputValue = sr.ReadToEnd().ToCharArray();
            }
            

            var result = HandleInput(InputValue.Clone() as char[], null).Length;

       
            Console.WriteLine();
            Console.WriteLine("-------------------------");
            Console.WriteLine("part1 : " + result);
            Console.WriteLine("-------------------------");

            var alpha = "abcdefghijklmnopqrstuvxyzåäö";
            foreach(var alphaChar in alpha)
            {
                var alphaResult = HandleInput(InputValue.Clone() as char[], alphaChar).Length;
                Console.WriteLine($"{alphaChar} = {alphaResult}");
            }

            Console.ReadLine();
        }

        private static string HandleInput(char[] input, char? removeSpecialChar)
        {
            var found = false;
            for (int i = 0; i < input.Length; i++)
            {
                if (i + 1 < input.Length)
                {
                    if (input[i] == '*') continue;

                    if (char.IsUpper(input[i]))
                    {
                        var loweredValue = char.ToLower(input[i]);
                        if (input[i + 1] == loweredValue)
                        {
                            input[i] = '*';
                            input[i + 1] = '*';
                            found = true;
                        }
                    }
                    else
                    {
                        var upperedValue = char.ToUpper(input[i]);
                        if (input[i + 1] == upperedValue)
                        {
                            input[i] = '*';
                            input[i + 1] = '*';
                            found = true;
                        }
                    }

                    if(removeSpecialChar != null)
                    {
                        if(char.ToLower(input[i]) == removeSpecialChar)
                        {
                            input[i] = '*';
                        }
                    }
                }
            }

            var joined = string.Join("", input);
            var apakaka = joined.Replace("*", string.Empty).Trim();

            if (found)
            {
                return HandleInput(apakaka.ToCharArray(), removeSpecialChar);
            }

            return string.Join("", input);
        }
    }
}
