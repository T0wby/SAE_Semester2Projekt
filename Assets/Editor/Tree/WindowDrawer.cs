using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using System;
using UnityEditor;

public class WindowDrawer
{
    private List<NodeWindow> _nodeWindows;
    private readonly Vector2 _nodeWindowSize = new Vector2(250, 80);

    private int _selectedRootNodeInt = -1;
    private NodeWindow _rootNode;

    private List<Type> _compositeNodeTypes;
    private string[] _composites;

    private ConnectionHandler _connectionHandler;
    private StatusBarDrawer _statusBarDrawer;

    public List<NodeWindow> NodeWindows { get => _nodeWindows; }

    public WindowDrawer(List<Type> compositeNodeTypes)
    {
        _nodeWindows = new List<NodeWindow>();
        _compositeNodeTypes = compositeNodeTypes;
        _composites = _compositeNodeTypes.ConvertAll(type => type.ToString()).ToArray();
        //_rootNode = new NodeWindow(new Rect(50, 50, _nodeWindowSize.x, _nodeWindowSize.y), _compositeNodeTypes.Count > 0 ? (Node)Activator.CreateInstance(_compositeNodeTypes[0]) : null);
        _rootNode = new NodeWindow(new Rect(50, 50, _nodeWindowSize.x, _nodeWindowSize.y), _compositeNodeTypes.Count > 0 ? _compositeNodeTypes[0].Name : null);
        _connectionHandler = new ConnectionHandler();
    }

    //public void AddWindow(int xPos, int yPos, Node node)
    //{
    //    _nodeWindows.Add(new NodeWindow(new Rect(xPos, yPos, _nodeWindowSize.x, _nodeWindowSize.y), node));
    //}
    public void AddWindow(int xPos, int yPos, string nodeName)
    {
        _nodeWindows.Add(new NodeWindow(new Rect(xPos, yPos, _nodeWindowSize.x, _nodeWindowSize.y), nodeName));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bTWindow"></param>
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
            //_rootNode.WindowNode = (Node)Activator.CreateInstance(_compositeNodeTypes[_selectedRootNodeInt]);
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

            while (_nodeWindows[idx].Children.Count > 0)
            {
                if (safecount > 100)
                    break;
                _connectionHandler.SetParentNode(_nodeWindows[idx], false);
                _connectionHandler.UpdateConnection(_nodeWindows[idx].Children[0]);
                safecount++;
            }

            _connectionHandler.SetParentNode(_nodeWindows[idx].Parent, false);
            _connectionHandler.UpdateConnection(_nodeWindows[idx]);

            _nodeWindows.RemoveAt(idx);
            return;
        }

        DrawAddAndRemoveButton(_nodeWindows[idx]);

        if (GUI.Button(new Rect(0, 47.5f, 15, 15), "="))
        {
            _connectionHandler.UpdateConnection(_nodeWindows[idx]);
            _statusBarDrawer.SetCurrentStatus(Status.Idle);
        }

        GUI.DragWindow();
    }

    /// <summary>
    /// Draws two buttons on each individual Node Window, that add or remove connections to other Node Windows
    /// </summary>
    /// <param name="nodeWindow"></param>
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
