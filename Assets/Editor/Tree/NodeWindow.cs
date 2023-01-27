using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeWindow
{
    private Rect _windowRect;
    private Node _windowNode;
    private bool _hasParent;
    private NodeWindow _parent;
    private List<NodeWindow> _children;

    public Rect WindowRect { get => _windowRect; set => _windowRect = value; }
    public Node WindowNode { get => _windowNode; set => _windowNode = value; }
    public bool HasParent { get => _hasParent; set => _hasParent = value; }
    public List<NodeWindow> Children { get => _children;}
    public NodeWindow Parent { get => _parent; set => _parent = value; }

    public NodeWindow()
    {
        _windowRect = new Rect();
        _windowNode = null;
        _hasParent = false;
        _children = new List<NodeWindow>();
        _parent = null;
    }

    public NodeWindow(Rect windowRect, Node windowNode)
    {
        _windowRect = windowRect;
        _windowNode = windowNode;
        _hasParent = false;
        _children = new List<NodeWindow>();
        _parent = null;
    }


}