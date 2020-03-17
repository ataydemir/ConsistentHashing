using System;
using System.Collections.Generic;
using System.Text;

namespace ConsistentHashing
{
    interface INode<T> where T : class
    {
        string Key { get; }
    }
}