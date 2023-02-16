using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Codice.CM.Common.CmCallContext;

[System.Serializable]
public class ConnectionHandler
{
    #region Fields
    private bool _addConnection;
    private NodeWindow _parent;

    // List of all active connections
    private List<WindowConnections> _connectedWindows;
    #endregion

    #region Properties
    public List<WindowConnections> ConnectedWindows { get => _connectedWindows; set => _connectedWindows = value; }
    #endregion

    #region Constructor
    public ConnectionHandler()
    {
        _addConnection = true;
        _parent = new NodeWindow();
        _connectedWindows = new List<WindowConnections>();
        SettingsDrawer.ConnectionHandler = this;
    } 
    #endregion

    /// <summary>
    /// Draws a line between every connection in the list
    /// </summary>
    public void DrawConnections()
    {
        foreach (WindowConnections item in _connectedWindows)
        {
            DrawNodeCurve(item.Parent.WindowRect, item.Child.WindowRect);
        }
    }

    /// <summary>
    /// Draws a Bezier curve from start to end rect
    /// </summary>
    /// <param name="start">Start of the Bezier curve</param>
    /// <param name="end">End of the Bezier curve</param>
    private void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width * 0.5f, start.y + start.height, 0);
        Vector3 endPos = new Vector3(end.x + end.width * 0.5f, end.y, 0);
        Vector3 startTan = startPos;
        Vector3 endTan = endPos;

        Color shadow = new Color(0, 0, 0, 0.06f);
        // Draw a shadow to the line
        for (int i = 0; i < 3; i++)
        {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadow, null, (i + 1) * 5);
        }

        // Draw the line itself over the shadow
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1f);
    }

    /// <summary>
    /// Sets the Parent node of the ConnectionHandler
    /// </summary>
    /// <param name="nodeWindow">Parent node to set</param>
    /// <param name="addConnection">Either connect or disconnect from the parent node</param>
    public void SetParentNode(NodeWindow nodeWindow, bool addConnection)
    {
        this._addConnection = addConnection;
        _parent = nodeWindow;
    }

    /// <summary>
    /// Connects the current Parent node with the given node
    /// </summary>
    /// <param name="nodeWindow">Node that should be connected to the current set Parent</param>
    public void ConnectNodes(NodeWindow nodeWindow)
    {
        WindowConnections connections = new WindowConnections(_parent, nodeWindow);

        // Check if the user tries to connect a node with itself
        if (nodeWindow.HasParent || _parent == null || _parent == nodeWindow) return;

        _connectedWindows.Add(connections);

        nodeWindow.Parent = _parent;
        nodeWindow.HasParent = true;
        _parent.Children.Add(nodeWindow);
        _parent = null;
    }

    /// <summary>
    /// Disconnects the given parent and child. Removes the connection from the connection list
    /// </summary>
    /// <param name="parent">Parent to disconnect from</param>
    /// <param name="child">Child to disconnect from parent</param>
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
                child.Parent = null;
                parent.Children.Remove(child);
                break;
            }
        }
        _parent = null;
    }

    /// <summary>
    /// Disconnects all current connections
    /// </summary>
    /// <param name="nodeWindows">List of all current active NodeWindows</param>
    public void DisconnectAllNodes(List<NodeWindow> nodeWindows)
    {
        WindowConnections connections;
        for (int i = 0; i < nodeWindows.Count; i++)
        {
            NodeWindow current = nodeWindows[i];
            if (current.Children.Count <= 0) continue;

            for (int y = _connectedWindows.Count - 1; y >= 0; y--)
            {
                connections = _connectedWindows[y];
                if (connections.Parent == current && current.Children.Contains(connections.Child))
                {
                    _connectedWindows.RemoveAt(y);
                    current.Children.Remove(connections.Child);
                    connections.Child.Parent = null;
                    connections.Child.HasParent = false;
                }
            }
        }
    }

    /// <summary>
    /// Depending on the current set bool we either connect or disconnect the given NodeWindow and current set parent
    /// </summary>
    /// <param name="nodeWindow">NodeWindow to connect or disconnect</param>
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
