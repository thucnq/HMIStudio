﻿using System;

namespace HMIStudio.Shared.TreeView.Collections
{
    public enum UpDownTraversalType
    {
        TopDown,
        BottomUp
    }

    public enum DepthBreadthTraversalType
    {
        DepthFirst,
        BreadthFirst
    }

    public enum NodeChangeType
    {
        NodeAdded,
        NodeRemoved
    }

    public enum NodeRelationType
    {
        Ancestor,
        Parent,
        Self,
        Child,
        Descendant
    }
}