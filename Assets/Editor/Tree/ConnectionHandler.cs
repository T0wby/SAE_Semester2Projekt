using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ConnectionHandler
{
    private bool _addConnection;
    private NodeWindow _parent;

    private List<WindowConnections> _connectedWindows;

    public List<WindowConnections> ConnectedWindows { get => _connectedWindows; set => _connectedWindows = value; }

    public ConnectionHandler()
    {
        _addConnection = true;
        _parent = new NodeWindow();
        _connectedWindows = new List<WindowConnections>();
        SettingsDrawer.ConnectionHandler = this;
    }

    public void DrawConnections()
    {
        foreach (WindowConnections item in _connectedWindows)
        {
            DrawNodeCurve(item.Parent.WindowRect, item.Child.WindowRect);
        }
    }

    private void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width * 0.5f, start.y + start.height, 0);
        Vector3 endPos = new Vector3(end.x + end.width * 0.5f, end.y, 0);
        Vector3 startTan = startPos;
        Vector3 endTan = endPos;

        Color shadow = new Color(0, 0, 0, 0.06f);
        for (int i = 0; i < 3; i++)
        {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadow, null, (i + 1) * 5);
        }

        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1f);
    }

    public void SetParentNode(NodeWindow nodeWindow, bool addConnection)
    {
        this._addConnection = addConnection;
        _parent = nodeWindow;
    }

    public void ConnectNodes(NodeWindow nodeWindow)
    {
        WindowConnections connections = new WindowConnections(_parent, nodeWindow);

        if (nodeWindow.HasParent || _parent.WindowNode == null || _parent.WindowNode == nodeWindow.WindowNode) return;

        _connectedWindows.Add(connections);

        nodeWindow.Parent = _parent;
        nodeWindow.HasParent = true;
        _parent.Children.Add(nodeWindow);
        _parent = null;
    }

    public void DisconnectNodes(NodeWindow parent, NodeWindow child)
    {
        WindowConnections connections;

        if (!child.HasParent || parent == null || parent.WindowNode == child.WindowNode) return;

        for (int i = _connectedWindows.Count - 1; i >= 0; i--)
        {
            connections = _connectedWindows[i];

            if (connections.Parent == parent && connections.Child == child)
            {
                _connectedWindows.RemoveAt(i);
                child.HasParent = false;
                child.Parent = null;
                break;
            }
        }
        parent.Children.Remove(child);
        _parent = null;
    }

    public void UpdateConnection(NodeWindow nodeWindow)
    {
        if (_addConnection)
        {
            ConnectNodes(nodeWindow);
        }
        else
        {
            DisconnectNodes(_parent, nodeWindow);
        }
    }

    [System.Serializable]
    public class WindowConnections
    {
        public NodeWindow Parent;
        public NodeWindow Child;

        public WindowConnections(NodeWindow parent, NodeWindow child)
        {
            Parent = parent;
            Child = child;
        }
    }
}
