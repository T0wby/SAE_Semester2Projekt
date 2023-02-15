using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

[System.Serializable]
public class NodeWindow
{
    private Rect _windowRect;
    private string _name;
    private bool _hasParent;
    private NodeWindow _parent;
    private List<NodeWindow> _children;

    public Rect WindowRect { get => _windowRect; set => _windowRect = value; }
    public bool HasParent { get => _hasParent; set => _hasParent = value; }
    public List<NodeWindow> Children { get => _children; set => _children = value; }
    public NodeWindow Parent { get => _parent; set => _parent = value; }
    public string Name { get => _name; set => _name = value; }

    public NodeWindow()
    {
        _windowRect = new Rect();
        _hasParent = false;
        _children = new List<NodeWindow>();
        _parent = null;
        _name = null;
    }

    public NodeWindow(Rect windowRect, string windowName)
    {
        _windowRect = windowRect;
        _hasParent = false;
        _children = new List<NodeWindow>();
        _parent = null;
        _name = windowName;
    }


}
