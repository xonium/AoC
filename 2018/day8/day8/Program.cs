using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day8
{
    class Program
    {
        public static string[] Input { get; set; }

        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader("../../../input.txt"))
            {
                Input = sr.ReadToEnd().Split(' ');
            }

            int pos = 0;
            var tree = CreateTree(Input.Clone() as string[], out pos);

        }

        private static Node CreateTree(string[] v, out int position)
        {
            position = 0;
            if (v.Length < 2) return null;

            var returnNode = new Node
            {
                QuantityOfChildNodes = int.Parse(v[0]),
                QuantityOfMetaDataEntries = int.Parse(v[1]),
                Children = new List<Node>(),
                Metadata = new List<int>()
            };

            if(returnNode.QuantityOfChildNodes == 0)
            {
                for (int i = 1; i <= returnNode.QuantityOfMetaDataEntries; i++)
                {
                    returnNode.Metadata.Add(int.Parse(v[1 + i]));
                    position = 4 + i;
                }

                return returnNode;
            }
            else
            {
                for (int i = 0; i < returnNode.QuantityOfChildNodes; i++)
                {
                    int currPosition = 0;
                    CreateTree(v.Skip(2).Take(v.Length - returnNode.QuantityOfMetaDataEntries).ToArray(), out currPosition);

                    var node = CreateTree(v.Skip(currPosition).Take(v.Length - returnNode.QuantityOfMetaDataEntries).ToArray(), out currPosition);

                    returnNode.Children.Add(node);
                }
            }

            return returnNode;
        }
    }

    class Node
    {
        public int QuantityOfChildNodes { get; set; }

        public int QuantityOfMetaDataEntries { get; set; }

        public List<Node> Children { get; set; }

        public List<int> Metadata { get; set; }
    }

}
