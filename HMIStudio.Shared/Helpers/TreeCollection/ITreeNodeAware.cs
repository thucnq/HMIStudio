using System;

namespace HMIStudio.Shared.TreeView.Collections
{
    public interface ITreeNodeAware<T>
        where T : new()
    {
        TreeNode<T> Node { get; set; }
    }
}