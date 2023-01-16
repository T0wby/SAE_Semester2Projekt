using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class NodeWindow
{
    private Rect _windowRect;
    private Node _windowNode;

    public NodeWindow()
    {
        _windowRect = new Rect();
        _windowNode = null;
    }

    public NodeWindow(Rect windowRect, Node windowNode)
    {
        _windowRect = windowRect;
        _windowNode = windowNode;
    }
}
