using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ConsistentHashing
{
    public class ConsistentHash<T> where T : class
    {
        // Hash circle
        private SortedDictionary<ulong, VirtualNode<T>> circle;

        // Mapping of physical nodes to virtual nodes to make life easier
        private Dictionary<ulong, IList<ulong>> virtualNodeMap;

        public ConsistentHash(IEnumerable<T> nodes, int replicateCount)
        {
            if (nodes == null || nodes.Any() == false)
                throw new ArgumentException("No node provided!");

            if (replicateCount < 0)
                throw new ArgumentException("Invalid number of replicate!");

            this.circle = new SortedDictionary<ulong, VirtualNode<T>>();
            this.virtualNodeMap = new Dictionary<ulong, IList<ulong>>();

            foreach (T node in nodes)
            {
                AddNode(node, replicateCount);
            }
        }

        public void AddNode(T pNode, int replicateCount = 0)
        {
            if (replicateCount < 0)
                throw new ArgumentException("Invalid number of replicate!");

            string pNodeHash = pNode.GetHashCode().ToString();
            ulong pNodeKey = pNodeHash.MurmurHash();
            int existingReplicatesCount = 0;

            if (virtualNodeMap.ContainsKey(pNodeKey) == true)
                existingReplicatesCount = virtualNodeMap[pNodeKey].Count();
            else
                this.virtualNodeMap.Add(pNodeKey, new List<ulong>());

            for (int i = 1; i <= replicateCount; i++)
            {
                VirtualNode<T> vNode = new VirtualNode<T>(pNode, existingReplicatesCount + i);

                ulong vNodeKey = vNode.Key.MurmurHash();

                this.circle.Add(vNodeKey, vNode);
                this.virtualNodeMap[pNodeKey].Add(vNodeKey);
            }
        }

        public void RemoveNode(T pNode)
        {
            if (!this.virtualNodeMap.Any())
                return;

            string pNodeHash = pNode.GetHashCode().ToString();
            ulong pNodeKey = pNodeHash.MurmurHash();

            if (this.virtualNodeMap.ContainsKey(pNodeKey))
            {
                foreach (ulong vNodeKey in virtualNodeMap[pNodeKey])
                {
                    this.circle.Remove(vNodeKey);
                }
                this.virtualNodeMap.Remove(pNodeKey);
            }
        }

        public void ReportToFile()
        {
            if (this.circle.Count == 0)
                return;

            Dictionary<string, ulong> memory = new Dictionary<string, ulong>();

            StringBuilder sb = new StringBuilder();
            foreach (var kvp in circle)
            {
                string hash = kvp.Value.PhysicalNode.GetHashCode().ToString();
                if (memory.ContainsKey(hash) == false)
                {
                    ulong key = hash.MurmurHash();
                    memory.Add(hash, key);
                }

                sb.AppendLine(memory[hash] + " & " + kvp.Key);
            }

            System.IO.File.WriteAllText("out.txt", sb.ToString());
        }
    }
}
