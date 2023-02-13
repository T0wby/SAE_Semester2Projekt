using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ConnectionHandler;

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
        for (int i = 0; i < nodeWindows.Count; i++)
        {
            for (int x = 0; x < nodeWindows[i].Children.Count; x++)
            {
                connectedWindows.Add(new WindowConnections(nodeWindows[i], nodeWindows[i].Children[x]));
            }
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
            // infinite loop, use different with parent=this
            children.Add(TranslateToNWWChild(node, nodeWindow.Children[i], nodeWindowWrap));
        }
        nodeWindowWrap._children = children;

        return nodeWindowWrap;
    }

    private static NodeWindowWrap TranslateToNWWChild(NodeWindowWrap nodeWindowWrap, NodeWindow nodeWindow, NodeWindowWrap parent)
    {
        if (nodeWindowWrap == null)
        {
            nodeWindowWrap = new NodeWindowWrap();
        }

        nodeWindowWrap._windowRect = nodeWindow.WindowRect;
        nodeWindowWrap._name = nodeWindow.Name;
        nodeWindowWrap._hasParent = nodeWindow.HasParent;

        if (parent != null)
            nodeWindowWrap._parent = parent;

        List<NodeWindowWrap> children = new List<NodeWindowWrap>();

        for (int i = 0; i < nodeWindow.Children.Count; i++)
        {
            NodeWindowWrap node = new NodeWindowWrap();
            // infinite loop, use different with parent=this
            children.Add(TranslateToNWWChild(node, nodeWindow.Children[i], nodeWindowWrap));
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
            children.Add(TranslateToNWChild(nodeWindowWrap._children[i], node, nodeWindow));
        }
        nodeWindow.Children = children;
        return nodeWindow;
    }

    private static NodeWindow TranslateToNWChild(NodeWindowWrap nodeWindowWrap, NodeWindow nodeWindow, NodeWindow parent)
    {
        if (nodeWindow == null)
        {
            nodeWindow = new NodeWindow();
        }

        nodeWindow.WindowRect = nodeWindowWrap._windowRect;
        nodeWindow.Name = nodeWindowWrap._name;
        nodeWindow.HasParent = nodeWindowWrap._hasParent;

        if (parent != null)
            nodeWindow.Parent = parent;

        List<NodeWindow> children = new List<NodeWindow>();

        for (int i = 0; i < nodeWindowWrap._children.Count; i++)
        {
            NodeWindow node = new NodeWindow();
            children.Add(TranslateToNWChild(nodeWindowWrap._children[i], node, nodeWindow));
        }
        nodeWindow.Children = children;
        return nodeWindow;
    }
}
