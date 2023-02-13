using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ConnectionHandler;
using static Unity.VisualScripting.Metadata;

public static class SaveTab
{
    private static string _filepath = $"{Application.dataPath}/";
    public static void SaveTree(List<NodeWindow> nodeWindows, List<WindowConnections> connectedWindows, string name)
    {
        List<NodeWindowWrap> nodeWindowWraps = new List<NodeWindowWrap>();
        List<WindowConnectionsWrap> windowConnectionsWraps = new List<WindowConnectionsWrap>();

        for (int i = 0; i < nodeWindows.Count; i++)
        {
            NodeWindowWrap nodeWindowWrap = new NodeWindowWrap();
            nodeWindowWraps.Add(TranslateToNWW(nodeWindowWrap, nodeWindows[i]));
        }

        for (int i = 0; i < connectedWindows.Count; i++)
        {
            if (connectedWindows[i].Parent == null && connectedWindows[i].Child == null)
                continue;

            WindowConnectionsWrap singleConnectionsWrap = new WindowConnectionsWrap();
            if (connectedWindows[i].Parent != null)
            {
                NodeWindowWrap nodeWindowWrap = new NodeWindowWrap();
                singleConnectionsWrap.Parent = TranslateToNWW(nodeWindowWrap, connectedWindows[i].Parent);
            }
            if (connectedWindows[i].Child != null)
            {
                NodeWindowWrap nodeWindowWrap = new NodeWindowWrap();
                singleConnectionsWrap.Child = TranslateToNWW(nodeWindowWrap, connectedWindows[i].Child);
            }

            windowConnectionsWraps.Add(singleConnectionsWrap);
        }

        NodeWindowListWrap nodeWindowListWrap = new NodeWindowListWrap(nodeWindowWraps, windowConnectionsWraps);

        string windowsJson = JsonUtility.ToJson(nodeWindowListWrap, true);

        using (StreamWriter writer = File.CreateText(_filepath + name))
        {
            writer.Write(windowsJson);
        }
    }

    public static List<NodeWindow> LoadTree(List<NodeWindow> nodeWindows, List<WindowConnections> connectedWindows, string name)
    {
        if (!File.Exists(_filepath + name))
            return nodeWindows;

        NodeWindowListWrap nodeWindowListWrap = new NodeWindowListWrap();
        string windows = string.Empty;

        using (StreamReader reader = File.OpenText(_filepath + name))
        {
            windows = reader.ReadToEnd();
        }

        JsonUtility.FromJsonOverwrite(windows, nodeWindowListWrap);

        nodeWindows.Clear();
        connectedWindows.Clear();

        for (int i = 0; i < nodeWindowListWrap._nodeWindowWraps.Count; i++)
        {
            NodeWindow nodeWindow = new NodeWindow();
            nodeWindow = TranslateToNW(nodeWindowListWrap._nodeWindowWraps[i], nodeWindow);

            nodeWindows.Add(nodeWindow);
        }
        for (int i = 0; i < nodeWindowListWrap._connections.Count; i++)
        {
            // Parent
            NodeWindowWrap nodeWindowWrapParent = nodeWindowListWrap._connections[i].Parent;
            NodeWindow nodeWindowParent = new NodeWindow();

            if (nodeWindowWrapParent != null)
            {
                nodeWindowParent = TranslateToNW(nodeWindowListWrap._connections[i].Parent, nodeWindowParent);
            }
            

            // Child
            NodeWindowWrap nodeWindowWrapChild = nodeWindowListWrap._connections[i].Child;
            NodeWindow nodeWindowChild = new NodeWindow();

            if (nodeWindowWrapChild != null)
            {
                nodeWindowChild = TranslateToNW(nodeWindowListWrap._connections[i].Child, nodeWindowChild);
            }
            
            connectedWindows.Add(new WindowConnections(nodeWindowParent, nodeWindowChild));
        }

        return nodeWindows;
    }

    /// <summary>
    /// Take information from a NodeWindow class and put it into a NodeWindowWrap class
    /// </summary>
    /// <param name="nodeWindowWrap">Class to fill</param>
    /// <param name="nodeWindow">Class to read from</param>
    /// <returns>Returns the filled class</returns>
    private static NodeWindowWrap TranslateToNWW(NodeWindowWrap nodeWindowWrap, NodeWindow nodeWindow)
    {
        if (nodeWindowWrap == null)
        {
            nodeWindowWrap = new NodeWindowWrap();
        }

        nodeWindowWrap._windowRect = nodeWindow.WindowRect;
        //nodeWindow.WindowNode = nodeWindowWrap._windowNode;
        nodeWindowWrap._name = nodeWindow.Name;
        nodeWindowWrap._hasParent = nodeWindow.HasParent;

        if(nodeWindow.Parent != null)
            nodeWindowWrap._parent = TranslateToNWW(nodeWindowWrap._parent, nodeWindow.Parent);

        List<NodeWindowWrap> children = new List<NodeWindowWrap>();

        for (int i = 0; i < nodeWindow.Children.Count; i++)
        {
            NodeWindowWrap node = new NodeWindowWrap();
            children.Add(TranslateToNWW(node, nodeWindow.Children[i]));
        }
        nodeWindowWrap._children = children;

        return nodeWindowWrap;
    }

    /// <summary>
    /// Take information from a NodeWindowWrap class and put it into a NodeWindow class
    /// </summary>
    /// <param name="nodeWindowWrap">Class to read from</param>
    /// <param name="nodeWindow">Class to fill</param>
    /// <returns>Returns the filled class</returns>
    private static NodeWindow TranslateToNW(NodeWindowWrap nodeWindowWrap, NodeWindow nodeWindow)
    {
        if (nodeWindow == null)
        {
            nodeWindow = new NodeWindow();
        }

        nodeWindow.WindowRect = nodeWindowWrap._windowRect;
        nodeWindow.Name = nodeWindowWrap._name;
        nodeWindow.HasParent = nodeWindowWrap._hasParent;

        if(nodeWindowWrap._parent != null)
            nodeWindow.Parent = TranslateToNW(nodeWindowWrap._parent, nodeWindow.Parent);

        List<NodeWindow> children = new List<NodeWindow>();

        for (int i = 0; i < nodeWindowWrap._children.Count; i++)
        {
            NodeWindow node = new NodeWindow();
            children.Add(TranslateToNW(nodeWindowWrap._children[i], node));
        }
        nodeWindow.Children = children;
        return nodeWindow;
    }
}
