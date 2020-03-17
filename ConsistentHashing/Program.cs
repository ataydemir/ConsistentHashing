using System;
using System.Collections.Generic;

namespace ConsistentHashing
{
    class Program
    {
        static void Main(string[] args)
        {
            IList<string> nodes = new List<string>();

            for (int i = 0; i < 10; i++)
                nodes.Add("Node#" + i);


            ConsistentHash<string> consistentHash = new ConsistentHash<string>(nodes, 200);
            consistentHash.ReportToFile();
        }
    }
}
