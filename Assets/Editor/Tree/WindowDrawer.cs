using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using System;
using UnityEditor;

public class WindowDrawer
{
    #region Fields
    // List of all active NodeWindows in the CurrentTree tab
    private List<NodeWindow> _nodeWindows;
    private readonly Vector2 _nodeWindowSize = new Vector2(250, 80);

    private int _selectedRootNodeInt = -1;
    private NodeWindow _rootNode;

    private List<Type> _compositeNodeTypes;
    private string[] _composites;

    private ConnectionHandler _connectionHandler;
    private StatusBarDrawer _statusBarDrawer;
    #endregion

    #region Properties
    public List<NodeWindow> NodeWindows { get => _nodeWindows; set => _nodeWindows = value; }
    #endregion

    #region Constructor
    public WindowDrawer(List<Type> compositeNodeTypes)
    {
        _nodeWindows = new List<NodeWindow>();
        _compositeNodeTypes = compositeNodeTypes;
        _composites = _compositeNodeTypes.ConvertAll(type => type.ToString()).ToArray();
        _rootNode = new NodeWindow(new Rect(50, 50, _nodeWindowSize.x, _nodeWindowSize.y), _compositeNodeTypes.Count > 0 ? _compositeNodeTypes[0].Name : null);
        _connectionHandler = new ConnectionHandler();
    }
    #endregion

    /// <summary>
    /// Creates a new window in the CurrentTree Tab and adds it to the overall list
    /// </summary>
    /// <param name="xPos">xPos of the created NodeWindow</param>
    /// <param name="yPos">yPos of the created NodeWindow</param>
    /// <param name="nodeName">name of the created NodeWindow</param>
    public void AddWindow(int xPos, int yPos, string nodeName)
    {
        _nodeWindows.Add(new NodeWindow(new Rect(xPos, yPos, _nodeWindowSize.x, _nodeWindowSize.y), nodeName));
    }

    /// <summary>
    /// Redraws all windows their position and connections
    /// </summary>
    /// <param name="bTWindow">Editorwindow to draw</param>
    public void RedrawWindows(BTWindow bTWindow)
    {
        bTWindow.BeginWindows();

        _rootNode.WindowRect = GUI.Window(-1, _rootNode.WindowRect, DrawRootNodeWindow, "Root");

        for (int i = 0; i < _nodeWindows.Count; i++)
        {
            _nodeWindows[i].WindowRect = GUI.Window(i, _nodeWindows[i].WindowRect, DrawNodeWindow, _nodeWindows[i].Name);
        }

        bTWindow.EndWindows();

        _connectionHandler.DrawConnections();
    }

    /// <summary>
    /// Sets the StatusBarDrawer
    /// </summary>
    /// <param name="statusBarDrawer">StatusBarDrawer to set</param>
    public void SetStatusReference(StatusBarDrawer statusBarDrawer)
    {
        _statusBarDrawer = statusBarDrawer;
    }

    /// <summary>
    /// Create a NodeWindow which functions as the root and can't be deleted
    /// </summary>
    /// <param name="id"></param>
    private void DrawRootNodeWindow(int id)
    {
        if (_selectedRootNodeInt != (_selectedRootNodeInt = EditorGUILayout.Popup(_selectedRootNodeInt, _composites)))
        {
            _rootNode.Name = _compositeNodeTypes[_selectedRootNodeInt].Name;
        }
        DrawAddAndRemoveButton(_rootNode);

        GUI.DragWindow();
    }

    /// <summary>
    /// Create a new NodeWindow
    /// </summary>
    /// <param name="idx">window id</param>
    private void DrawNodeWindow(int idx)
    {
        if (GUI.Button(new Rect(0, 0, 20, 20), "X"))
        {

            int safecount = 0;

            // Remove every child from the deleted Node
            while (_nodeWindows[idx].Children.Count > 0)
            {
                if (safecount > 100)
                    break;
                _connectionHandler.SetParentNode(_nodeWindows[idx], false);
                _connectionHandler.UpdateConnection(_nodeWindows[idx].Children[0]);
                safecount++;
            }

            // Update the connection and delete lines
            _connectionHandler.SetParentNode(_nodeWindows[idx].Parent, false);
            _connectionHandler.UpdateConnection(_nodeWindows[idx]);

            // Remove from active window list
            _nodeWindows.RemoveAt(idx);
            return;
        }

        DrawAddAndRemoveButton(_nodeWindows[idx]);

        if (GUI.Button(new Rect(0, 47.5f, 25, 15), "="))
        {
            _connectionHandler.UpdateConnection(_nodeWindows[idx]);
            _statusBarDrawer.SetCurrentStatus(Status.Idle);
        }

        GUI.DragWindow();
    }

    /// <summary>
    /// Draws two buttons on each individual Node Window, that add or remove connections to other Node Windows
    /// </summary>
    /// <param name="nodeWindow">Reference to the NodeWindow itself</param>
    private void DrawAddAndRemoveButton(NodeWindow nodeWindow)
    {
        if (GUI.Button(new Rect(_nodeWindowSize.x - 110, 40, 100, 15), "Add"))
        {
            _connectionHandler.SetParentNode(nodeWindow, true);
            _statusBarDrawer.SetCurrentStatus(Status.Adding);
        }
        if (GUI.Button(new Rect(_nodeWindowSize.x - 110, 60, 100, 15), "Remove"))
        {
            _connectionHandler.SetParentNode(nodeWindow, false);
            _statusBarDrawer.SetCurrentStatus(Status.Removing);
        }
    }
}
