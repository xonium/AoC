using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day3
{
    class Program
    {
        static List<FabricClaim> FabricClaims { get; set; }
        static int NumberOfOverlappingClaims { get; set; }
        static char[][] Fabric { get; set; }
        const int FABRICSIZE = 1000;

        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader("../../../input.txt"))
            {
                FabricClaims = new List<FabricClaim>();

                var inputString = sr.ReadToEnd();
                var inputValues = inputString.Split(Environment.NewLine)
                                    .Where(y => y != string.Empty)
                                    .Select(x => x.Trim());

                var ids = inputValues.Select(x => x.Split('@')[0].Trim());
                var pos = inputValues
                            .Select(x => x.Substring(x.IndexOf('@') + 1, x.IndexOf(':') - (x.IndexOf('@')+1)).Trim()
                            .Split(',').ToArray()).ToArray();
                var val = inputValues
                            .Select(x => x.Substring(x.IndexOf(':') + 1).Trim()
                            .Split('x').ToArray()).ToArray();

                var i = 0;
                foreach(var id in ids)
                {
                    var f = new FabricClaim() {
                        Id = id,
                        StartPosition = new Position { X = int.Parse(pos[i][0]), Y= int.Parse(pos[i][1]) },
                        Size = new Size { Wide = int.Parse(val[i][0]), Tall = int.Parse(val[i][1]) }
                    };

                    FabricClaims.Add(f);
                    i++;
                }

            }
            InitFabric();
            PlanFabricClaims();
            //PrintFabricClaims();

            Console.WriteLine("Number of Claims overlapping: " + NumberOfOverlappingClaims);

            var claimsNotOverLapping = CheckAllClaimsAfterPlanning();

            foreach(var claimNotOverLapping in claimsNotOverLapping)
            {
                Console.WriteLine("Claim not overlapping: " + claimNotOverLapping.Id);
            }

            Console.ReadLine();
        }

        private static List<FabricClaim> CheckAllClaimsAfterPlanning()
        {
            var claimNotOverLapping = new List<FabricClaim>();
            
            foreach (var claim in FabricClaims)
            {
                var overlaps = false;

                for (int t = 0; t < claim.Size.Tall; t++)
                {
                    for (int w = 0; w < claim.Size.Wide; w++)
                    {
                        if (Fabric[claim.StartPosition.X + w][claim.StartPosition.Y + t] == '*')
                        {
                            overlaps = true;
                        }
                    }
                }

                if(!overlaps)
                {
                    claimNotOverLapping.Add(claim);
                }
            }

            return claimNotOverLapping;
        }

        private static void PrintFabricClaims()
        {
            for (int y = 0; y < Fabric.Length; y++)
            {
                for (int x = 0; x < Fabric[y].Length; x++)
                {
                    Console.Write(Fabric[y][x]);
                }

                Console.WriteLine();
            }
        }

        private static void PlanFabricClaims()
        {
            foreach(var claim in FabricClaims)
            {
                for(int t = 0; t < claim.Size.Tall; t++)
                {
                    for (int w = 0; w < claim.Size.Wide; w++)
                    {
                        if(Fabric[claim.StartPosition.X+w][claim.StartPosition.Y+t] == '.')
                        {
                            Fabric[claim.StartPosition.X + w][claim.StartPosition.Y + t] = 'X';
                        }
                        else if (Fabric[claim.StartPosition.X + w][claim.StartPosition.Y + t] == 'X')
                        {
                            Fabric[claim.StartPosition.X + w][claim.StartPosition.Y + t] = '*';
                            NumberOfOverlappingClaims++;
                        }
                    }
                }
            }

        }

        static void InitFabric()
        {
            Fabric = new char[FABRICSIZE][];
            for (int i = 0; i < Fabric.Length; i++)
            {
                Fabric[i] = Enumerable.Repeat('.', FABRICSIZE).ToArray();
            }
        }
    }

    public class Size
    {
        public int Wide { get; set; }
        public int Tall { get; set; }
    }

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class FabricClaim
    {
        public string Id { get; set; }
        public Position StartPosition { get; set; }
        public Size Size { get; set; }
    }
}
