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
            var rulesDictionary = new Dictionary<string, char>();

            var initialStateString = string.Empty;

            using (StreamReader sr = new StreamReader("../../../input.txt"))
            {
                var inputString = sr.ReadToEnd();
                initialStateString = inputString.Substring(14, inputString.IndexOf(Environment.NewLine, StringComparison.Ordinal)-14);
                var splitted = inputString.Split(Environment.NewLine);

                for (int i = 2; i < splitted.Length; i++)
                {
                    var theRule = new Rule(splitted[i]);
                    rulesDictionary.Add(theRule.RuleString, theRule.GeneratesChar);
                }
            }

            foreach (var potChar in initialStateString.Trim())
            {
                initialState.Add(new Pot(potChar));
            }

            var generationGenerator = new GenerationGenerator(rulesDictionary, 4);

            var generations = generationGenerator.Generate(initialState, 21);
            long count = generationGenerator.CountPots(generations.Last(), generationGenerator.Offset);

            Print.Do(generations, count);

            Console.ReadLine();
            Console.WriteLine("-- starting with part two --");

            var generationGeneratorPartTwo = new GenerationGenerator(rulesDictionary, 4);
            //var generationsPartTwo = generationGeneratorPartTwo.Generate(initialState, 50000000001);
            var generationsPartTwo = generationGeneratorPartTwo.Generate(initialState, 50000000001);

            Console.WriteLine(generationGeneratorPartTwo.CountPots(generationsPartTwo.Last(), generationGeneratorPartTwo.Offset));

            Console.WriteLine("-- end part two --");
            Console.ReadLine();
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

        public string RuleString { get; set; }

        public char GeneratesChar { get; set; }

        public Rule(string rule)
        {
            _ruleString = rule;
            RuleToMatch = new List<Pot>();

            var splittedRule = rule.Split(" => ");
            foreach (var s in splittedRule[0])
            {
                RuleToMatch.Add(new Pot(s));
            }
            RuleString = splittedRule[0];
            Generates = new Pot(splittedRule[1][0]);
            GeneratesChar = splittedRule[1][0];
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
        public long Offset { get; set; }
        public long HowMuchOffset { get; set; }

        private Dictionary<string, char> _rules { get; set; }
        public GenerationGenerator(Dictionary<string, char> rules, int offset)
        {
            _rules = rules;
            Offset = offset;
        }

        public long CountPots(List<Pot> pots, long offset)
        {
            long index = offset-HowMuchOffset;
            long result = 0;
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

            currentArrayList.InsertRange(unchecked((int)Offset), inputState);
            var currentArray = currentArrayList.ToArray();
            var nextGenerationArray = currentArrayList.ToArray();
            result.Add(nextGenerationArray.ToList());

            for (long g = 1; g < generations; g++)
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

                    if (_rules.TryGetValue($"{pots[0].Input}{pots[1].Input}{pots[2].Input}{pots[3].Input}{pots[4].Input}", out char rulesGenerates))
                    {
                        nextGenerationArray[i] = new Pot(rulesGenerates);
                    }
                }

                var nextGenerationList = nextGenerationArray.ToList();
                var first = nextGenerationList.Take(6);
                if(first.All(x => !x.ContainsPlant))
                {
                    nextGenerationList.RemoveRange(0, 4);
                    Offset = Offset + 4;
                    HowMuchOffset += 4;
                }
                
                var last = nextGenerationList.TakeLast(4);
                if (last.Any(x => x.ContainsPlant))
                {
                    var lastArray = last.ToArray();
                    for (int i = lastArray.Length - 1; i > 0; i--)
                    {
                        if (lastArray[i].ContainsPlant)
                        {
                            var itemsToAdd = i + 1;
                            for (int p = 0; p < itemsToAdd; p++)
                            {
                                nextGenerationList.Add(new Pot('.'));
                            }
                        }
                    }
                }

                nextGenerationArray = nextGenerationList.ToArray();
                currentArray = nextGenerationList.ToArray();
            }

            HowMuchOffset = HowMuchOffset - 4;
            result.Add(nextGenerationArray.ToList());

            return result;
        }

    }

    public static class Print
    {
        public static void Do(List<List<Pot>> generations, long pots, long offset=25)
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
