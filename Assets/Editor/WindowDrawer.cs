using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class WindowDrawer
{
    private List<NodeWindow> _nodeWindows;
    private readonly Vector2 _nodeWindowSize = new Vector2(250, 80);

    public WindowDrawer()
    {
        _nodeWindows= new List<NodeWindow>();
    }

    public void AddWindow(int xPos, int yPos, Node node)
    {
        _nodeWindows.Add(new NodeWindow(new Rect(xPos, yPos, _nodeWindowSize.x, _nodeWindowSize.y), node));
    }
}
