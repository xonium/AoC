using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day12
{
    class Program
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

            var generationGenerator = new GenerationGenerator(rules);

            var generations = generationGenerator.Generate(initialState, 21);

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

            Console.WriteLine(generationGenerator.CountPots(generations.Last()));
            Console.ReadLine();
        }
    }

    public class Pot
    {
        public Pot()
        {
            ContainsPlant = false;
        }

        public Pot(char input)
        {
            ContainsPlant = input == '#';
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
        private  List<Rule> _rules { get; set; }
        public GenerationGenerator(List<Rule> rules)
        {
            _rules = rules;
        }

        public int CountPots(List<Pot> pots)
        {
            int index = -3;
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

        public List<List<Pot>> Generate(List<Pot> inputState, int generations)
        {
            var result = new List<List<Pot>>();
            int offset = 3;
            
            var currentArrayList = new List<Pot>();

            for (int i = 0; i < 50; i++)
            {
                currentArrayList.Add(new Pot());
            }

            currentArrayList.InsertRange(offset, inputState);
            var currentArray = currentArrayList.ToArray();
            var nextGenerationArray = currentArrayList.ToArray();
            result.Add(nextGenerationArray.ToList());

            for (int g = 1; g < generations; g++)
            {
                int index = 0;

                for (int i = offset; i < currentArray.Length; i++)
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
}
