using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeWindowWrap
{
    public Rect _windowRect;
    public Node _windowNode;
    public bool _hasParent;
    public NodeWindow _parent;
    public List<NodeWindow> _children;


    public NodeWindowWrap()
    {

    }

	public NodeWindowWrap(NodeWindow nodeWindow)
	{
        _windowRect = nodeWindow.WindowRect;
        _windowNode = nodeWindow.WindowNode;
        _hasParent = nodeWindow.HasParent;
        _parent = nodeWindow.Parent;
        _children = nodeWindow.Children;
    }
}
