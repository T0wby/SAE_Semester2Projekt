using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WindowConnectionsWrap
{
    public NodeWindowWrap Parent;
    public NodeWindowWrap Child;

    public WindowConnectionsWrap()
    {
        Parent = null;
        Child = null;
    }

    public WindowConnectionsWrap(NodeWindowWrap parent, NodeWindowWrap child)
	{
        Parent = parent;
        Child = child;
    }
}
