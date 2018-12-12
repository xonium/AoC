using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day12
{
    public class Program
    {
        static void Main(string[] args)
        {
            var initialState = new List<Pot>();
            var rules = new List<Rule>();

            var initialStateString = string.Empty;

            using (StreamReader sr = new StreamReader("../../../input.txt"))
            {
                var inputString = sr.ReadToEnd();
                initialStateString = inputString.Substring(14, inputString.IndexOf(Environment.NewLine, StringComparison.Ordinal)-14);
                var splitted = inputString.Split(Environment.NewLine);

                for (int i = 2; i < splitted.Length; i++)
                {
                    rules.Add(new Rule(splitted[i]));
                }
            }

            foreach (var potChar in initialStateString.Trim())
            {
                initialState.Add(new Pot(potChar));
            }

            var generationGenerator = new GenerationGenerator(rules, 25);

            var generations = generationGenerator.Generate(initialState, 21);
            int count = generationGenerator.CountPots(generations.Last(), generationGenerator.Offset);

            Print.Do(generations, count);

            Console.ReadLine();
            /*Console.WriteLine("-- starting with part two --");

            var generationGeneratorPartTwo = new GenerationGenerator(rules, 100000);
            //var generationsPartTwo = generationGeneratorPartTwo.Generate(initialState, 50000000001);
            var generationsPartTwo = generationGeneratorPartTwo.Generate(initialState, 100000);

            Console.WriteLine(generationGeneratorPartTwo.CountPots(generationsPartTwo.Last(), generationGeneratorPartTwo.Offset));

            Console.WriteLine("-- end part two --");
            Console.ReadLine();*/
        }
    }

    public class Pot
    {
        public Pot()
        {
            ContainsPlant = false;
            Input = '.';
        }

        public char Input {get;set;}

        public Pot(char input)
        {
            ContainsPlant = input == '#';
            Input = ContainsPlant ? '#' : '.';
        }
        public bool ContainsPlant {
            get;set;
        }
    }

    public class Rule
    {
        private string _ruleString { get; set; }

        private List<Pot> RuleToMatch { get; set; }

        public Pot Generates { get; set; }
        public Rule(string rule)
        {
            _ruleString = rule;
            RuleToMatch = new List<Pot>();

            var splittedRule = rule.Split(" => ");
            foreach (var s in splittedRule[0])
            {
                RuleToMatch.Add(new Pot(s));
            }

            Generates = new Pot(splittedRule[1][0]);
        }
        
        public bool IsMatch(List<Pot> inputpots)
        {
            if (inputpots[0].ContainsPlant == RuleToMatch[0].ContainsPlant &&
                inputpots[1].ContainsPlant == RuleToMatch[1].ContainsPlant &&
                inputpots[2].ContainsPlant == RuleToMatch[2].ContainsPlant &&
                inputpots[3].ContainsPlant == RuleToMatch[3].ContainsPlant &&
                inputpots[4].ContainsPlant == RuleToMatch[4].ContainsPlant)
            {                
                return true;
            }

            return false;
        }
    }

    public class GenerationGenerator
    {
        public int Offset { get; set; }

        private  List<Rule> _rules { get; set; }
        public GenerationGenerator(List<Rule> rules, int offset)
        {
            _rules = rules;
            Offset = offset;
        }

        public int CountPots(List<Pot> pots, int offset)
        {
            int index = -offset;
            var result = 0;
            foreach (var pot in pots)
            {
                if (pot.ContainsPlant)
                {
                    result += index;
                }

                index++;
            }

            return result;
        }

        public List<List<Pot>> Generate(List<Pot> inputState, long generations)
        {
            var result = new List<List<Pot>>();
            
            var currentArrayList = new List<Pot>();

            for (int i = 0; i < Offset*2; i++)
            {
                currentArrayList.Add(new Pot());
            }

            currentArrayList.InsertRange(Offset, inputState);
            var currentArray = currentArrayList.ToArray();
            var nextGenerationArray = currentArrayList.ToArray();
            result.Add(nextGenerationArray.ToList());

            for (int g = 1; g < generations; g++)
            {
                for (int i = 0; i < currentArray.Length; i++)
                {
                    var pots = new List<Pot>()
                    {
                        i - 2 < 0 ? new Pot('.') : currentArray[i - 2],
                        i - 1 < 0 ? new Pot('.') : currentArray[i - 1],
                        currentArray[i],
                        i + 1 >= currentArray.Length ? new Pot('.') : currentArray[i + 1],
                        i + 2 >= currentArray.Length ? new Pot('.') : currentArray[i + 2]
                    };


                    foreach (var rule in _rules)
                    {
                        if (rule.IsMatch(pots))
                        {
                            nextGenerationArray[i] = rule.Generates;
                            break;
                        }
                    }
                }
                
                result.Add(nextGenerationArray.ToList());
                currentArray = nextGenerationArray.ToArray();
            }

            return result;
        }

    }

    public static class Print
    {
        public static void Do(List<List<Pot>> generations, int pots, int offset=25)
        {
            string counterTenString = "    ";
            for (int i = 0; i < offset; i++)
            {
                counterTenString += " ";
            }
            int p = 0;
            for (int i = 0; i < generations.First().Count; i++)
            {
                if (i % 10 == 0)
                {
                    if(p == 0)
                    {
                        counterTenString += " ";
                    }
                    else if(p >= 10)
                    {
                        counterTenString += p.ToString().First();
                    }
                    else { 
                    counterTenString += p;
                    }
                    p++;
                }
                else
                {
                    counterTenString += " ";
                }
            }

            Console.WriteLine(counterTenString);

            string counterZeroString = "    ";
            for (int i = 0; i < offset; i++)
            {
                counterZeroString += " ";
            }
            for (int i = 0; i < generations.First().Count; i++)
            {
                if(i % 10 == 0)
                {
                    counterZeroString += "0";
                }
                else
                {
                    counterZeroString += " ";
                }
            }

            Console.WriteLine(counterZeroString);

            int g = 0;
            foreach (var generation in generations)
            {
                if (g < 10)
                {
                    Console.Write($" {g}: ");
                }
                else
                {
                    Console.Write($"{g}: ");
                }
                foreach (var pot in generation)
                {
                    Console.Write(pot.ContainsPlant ? '#' : '.');
                }

                g++;
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + pots);
        }
    }
}
