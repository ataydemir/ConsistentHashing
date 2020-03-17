using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ConsistentHashing
{
    public class VirtualNode<T> : INode<T> where T : class
    {
        private readonly T physicalNode;
        public T PhysicalNode
        {
            get { return this.physicalNode; }
        }

        private readonly int replicaIndex;
        public int ReplicaIndex
        {
            get { return this.replicaIndex; }
        }

        public string Key
        {
            get { return this.physicalNode.GetHashCode() + "_" + this.replicaIndex ; }
        }


        public VirtualNode(T physicalNode, int replicaIndex)
        {
            this.physicalNode = physicalNode;
            this.replicaIndex = replicaIndex;
        }
    }
}
