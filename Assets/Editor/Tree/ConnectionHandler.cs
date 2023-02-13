using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Codice.CM.Common.CmCallContext;

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

        //if (nodeWindow.HasParent || _parent.WindowNode == null || _parent.WindowNode == nodeWindow.WindowNode) return;
        if (nodeWindow.HasParent || _parent == null || _parent == nodeWindow) return;

        _connectedWindows.Add(connections);

        nodeWindow.Parent = _parent;
        nodeWindow.HasParent = true;
        _parent.Children.Add(nodeWindow);
        _parent = null;
    }

    public void DisconnectNodes(NodeWindow parent, NodeWindow child)
    {
        WindowConnections connections;

        if (!child.HasParent || parent == null || parent == child) return;

        for (int i = _connectedWindows.Count - 1; i >= 0; i--)
        {
            connections = _connectedWindows[i];

            if (connections.Parent == parent && connections.Child == child)
            {
                _connectedWindows.RemoveAt(i);
                child.HasParent = false;
                child.Parent.Children.Remove(parent);
                child.Parent = null;
                break;
            }
        }
        parent.Children.Remove(child);
        _parent = null;
    }

    public void DisconnectAllNodes(List<NodeWindow> nodeWindows)
    {
        WindowConnections connections;
        for (int i = 0; i < nodeWindows.Count; i++)
        {
            NodeWindow current = nodeWindows[i];
            if (!current.HasParent) continue;

            for (int y = _connectedWindows.Count - 1; y >= 0; y--)
            {
                connections = _connectedWindows[y];
                if (connections.Parent == current.Parent && connections.Child == current)
                {
                    _connectedWindows.RemoveAt(y);
                    current.HasParent = false;
                    current.Parent.Children.Remove(current);
                    current.Parent = null;

                    break;
                }
            }
        }
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
