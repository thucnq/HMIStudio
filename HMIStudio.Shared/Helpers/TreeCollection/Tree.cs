using System;

namespace HMIStudio.Shared.TreeView.Collections
{
    public class Tree<T> : TreeNode<T>
        where T : new()
    {
        public Tree() { }

        public Tree(T RootValue)
        {
            Value = RootValue;
        }
    }
}