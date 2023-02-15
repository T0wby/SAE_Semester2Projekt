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
            //nodeWindowWraps.Add(TranslateToNWW(nodeWindowWrap, nodeWindows[i]));
            nodeWindowWrap = TranslateBasicInfoToSave(nodeWindowWrap, nodeWindows[i]);
            nodeWindowWraps.Add(nodeWindowWrap);
        }

        TranslateParentInfoToSave(nodeWindowWraps, nodeWindows);

        TranslateChildInfoToSave(nodeWindowWraps, nodeWindows);

        for (int i = 0; i < connectedWindows.Count; i++)
        {
            if (connectedWindows[i].Parent == null && connectedWindows[i].Child == null)
                continue;

            WindowConnectionsWrap singleConnectionsWrap = new WindowConnectionsWrap();
            if (connectedWindows[i].Parent != null)
            {
                //NodeWindowWrap nodeWindowWrap = new NodeWindowWrap();
                //singleConnectionsWrap.Parent = TranslateToNWW(nodeWindowWrap, connectedWindows[i].Parent);
                singleConnectionsWrap.Parent = SearchMatchingNode(connectedWindows[i].Parent, nodeWindowWraps);
            }
            if (connectedWindows[i].Child != null)
            {
                //NodeWindowWrap nodeWindowWrap = new NodeWindowWrap();
                //singleConnectionsWrap.Child = TranslateToNWW(nodeWindowWrap, connectedWindows[i].Child);
                singleConnectionsWrap.Child = SearchMatchingNode(connectedWindows[i].Child, nodeWindowWraps);
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

        // Setting all Nodes
        for (int i = 0; i < nodeWindowListWrap._nodeWindowWraps.Count; i++)
        {
            NodeWindow nodeWindow = new NodeWindow();
            nodeWindow = TranslateBasicInfoBack(nodeWindowListWrap._nodeWindowWraps[i], nodeWindow);

            nodeWindows.Add(nodeWindow);
        }

        TranslateParentInfoBack(nodeWindowListWrap._nodeWindowWraps, nodeWindows);

        TranslateChildInfoBack(nodeWindowListWrap._nodeWindowWraps, nodeWindows);

        // Setting Connection list
        for (int i = 0; i < nodeWindows.Count; i++)
        {
            for (int x = 0; x < nodeWindows[i].Children.Count; x++)
            {
                connectedWindows.Add(new WindowConnections(nodeWindows[i], nodeWindows[i].Children[x]));
            }
        }

        return nodeWindows;
    }

    private static NodeWindowWrap TranslateBasicInfoToSave(NodeWindowWrap nodeWindowWrap, NodeWindow nodeWindow)
    {
        if (nodeWindowWrap == null)
        {
            nodeWindowWrap = new NodeWindowWrap();
        }

        nodeWindowWrap.WindowRect = nodeWindow.WindowRect;
        nodeWindowWrap.Name = nodeWindow.Name;
        nodeWindowWrap.HasParent = nodeWindow.HasParent;

        return nodeWindowWrap;
    }

    private static void TranslateParentInfoToSave(List<NodeWindowWrap> nodeWindowWraps, List<NodeWindow> nodeWindows)
    {
        for (int i = 0; i < nodeWindows.Count; i++)
        {
            if (!nodeWindows[i].HasParent) continue;

            nodeWindowWraps[i].ParentWindowRect = nodeWindows[i].Parent.WindowRect;
            nodeWindowWraps[i].ParentName = nodeWindows[i].Parent.Name;
        }
    }

    private static void TranslateChildInfoToSave(List<NodeWindowWrap> nodeWindowWraps, List<NodeWindow> nodeWindows)
    {
        for (int i = 0; i < nodeWindows.Count; i++)
        {
            List<NodeWindow> children = nodeWindows[i].Children;
            int childCount = children.Count;
            if (childCount <= 0) continue;

            nodeWindowWraps[i].Children = new List<NodeWindowWrap>();

            for (int x = 0; x < childCount; x++)
            {
                NodeWindowWrap child = SearchMatchingNode(children[x], nodeWindowWraps);

                if (child != null)
                    nodeWindowWraps[i].Children.Add(child);
            }
        }
    }

    private static NodeWindow TranslateBasicInfoBack(NodeWindowWrap nodeWindowWrap, NodeWindow nodeWindow)
    {
        if (nodeWindow == null)
        {
            nodeWindow = new NodeWindow();
        }

        nodeWindow.WindowRect = nodeWindowWrap.WindowRect;
        nodeWindow.Name = nodeWindowWrap.Name;
        nodeWindow.HasParent = nodeWindowWrap.HasParent;

        return nodeWindow;
    }

    private static void TranslateParentInfoBack(List<NodeWindowWrap> nodeWindowWraps, List<NodeWindow>  nodeWindows)
    {
        for (int i = 0; i < nodeWindowWraps.Count; i++)
        {
            if (!nodeWindowWraps[i].HasParent) continue;

            for (int x = 0; x < nodeWindows.Count; x++)
            {
                if (nodeWindowWraps[i].ParentName == nodeWindows[x].Name && nodeWindowWraps[i].ParentWindowRect == nodeWindows[x].WindowRect)
                {
                    nodeWindows[i].Parent = nodeWindows[x];
                    break;
                }
            }
        }
    }

    private static void TranslateChildInfoBack(List<NodeWindowWrap> nodeWindowWraps, List<NodeWindow> nodeWindows)
    {
        for (int i = 0; i < nodeWindowWraps.Count; i++)
        {
            List<NodeWindowWrap> children = nodeWindowWraps[i].Children;
            int childCount = children.Count;
            if (childCount <= 0) continue;

            for (int x = 0; x < childCount; x++)
            {
                NodeWindow child = SearchMatchingNode(children[x], nodeWindows);

                if(child != null)
                    nodeWindows[i].Children.Add(child);
            }
        }
    }

    private static NodeWindow SearchMatchingNode(NodeWindowWrap nodeWindowWrap, List<NodeWindow> nodeWindows)
    {
        for (int i = 0; i < nodeWindows.Count; i++)
        {
            if (nodeWindowWrap.Name == nodeWindows[i].Name && nodeWindowWrap.WindowRect == nodeWindows[i].WindowRect)
            {
                return nodeWindows[i];
            }
        }
        return null;
    }
    private static NodeWindowWrap SearchMatchingNode(NodeWindow nodeWindow, List<NodeWindowWrap> nodeWindowWraps)
    {
        for (int i = 0; i < nodeWindowWraps.Count; i++)
        {
            if (nodeWindow.Name == nodeWindowWraps[i].Name && nodeWindow.WindowRect == nodeWindowWraps[i].WindowRect)
            {
                return nodeWindowWraps[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Searches for a matching node in all active nodeWindows, to set the correct reference
    /// </summary>
    /// <param name="nodeWindows">List of all Nodes</param>
    /// <param name="child">Node used to compare</param>
    /// <returns>Returns a reference to the found Node</returns>
    private static NodeWindow SearchForChildNode(List<NodeWindow> nodeWindows, NodeWindow child)
    {
        for (int i = 0; i < nodeWindows.Count; i++)
        {
            if (nodeWindows[i].WindowRect == child.WindowRect && nodeWindows[i].Name == child.Name)
            {
                return nodeWindows[i];
            }
        }
        Debug.Log("No matching Child found");
        return null;
    }

    private static void SetParentNodes(List<NodeWindow> nodeWindows)
    {
        for (int i = 0; i < nodeWindows.Count; i++)
        {
            int childCount = nodeWindows[i].Children.Count;
            if (childCount <= 0) continue;

            for (int x = 0; x < childCount; x++)
            {
                SearchForChildNode(nodeWindows, nodeWindows[i].Children[x]).Parent = nodeWindows[i];
            }
        }
    }


    // Old Translation
    /** 

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

        nodeWindow.WindowRect = nodeWindowWrap.WindowRect;
        nodeWindow.Name = nodeWindowWrap.Name;
        nodeWindow.HasParent = nodeWindowWrap.HasParent;

        if (nodeWindowWrap.Parent != null)
            nodeWindow.Parent = TranslateToNW(nodeWindowWrap.Parent, nodeWindow.Parent);

        List<NodeWindow> children = new List<NodeWindow>();

        for (int i = 0; i < nodeWindowWrap.Children.Count; i++)
        {
            NodeWindow node = new NodeWindow();
            children.Add(TranslateToNWChild(nodeWindowWrap.Children[i], node, nodeWindow));
        }
        nodeWindow.Children = children;
        return nodeWindow;
    }

    /// <summary>
    /// Take information from a NodeWindowWrap class and put it into a NodeWindow class, but set the parent extra to avoid a stack overflow
    /// </summary>
    /// <param name="nodeWindowWrap">Class to read from</param>
    /// <param name="nodeWindow">Class to fill</param>
    /// <param name="parent">Parent of the class to fill</param>
    /// <returns>Returns the filled class</returns>
    private static NodeWindow TranslateToNWChild(NodeWindowWrap nodeWindowWrap, NodeWindow nodeWindow, NodeWindow parent)
    {
        if (nodeWindow == null)
        {
            nodeWindow = new NodeWindow();
        }

        nodeWindow.WindowRect = nodeWindowWrap.WindowRect;
        nodeWindow.Name = nodeWindowWrap.Name;
        nodeWindow.HasParent = nodeWindowWrap.HasParent;

        if (parent != null)
            nodeWindow.Parent = parent;

        List<NodeWindow> children = new List<NodeWindow>();

        for (int i = 0; i < nodeWindowWrap.Children.Count; i++)
        {
            NodeWindow node = new NodeWindow();
            children.Add(TranslateToNWChild(nodeWindowWrap.Children[i], node, nodeWindow));
        }
        nodeWindow.Children = children;
        return nodeWindow;
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

        nodeWindowWrap.WindowRect = nodeWindow.WindowRect;
        nodeWindowWrap.Name = nodeWindow.Name;
        nodeWindowWrap.HasParent = nodeWindow.HasParent;

        if (nodeWindow.Parent != null)
            nodeWindowWrap.Parent = TranslateToNWW(nodeWindowWrap.Parent, nodeWindow.Parent);

        List<NodeWindowWrap> children = new List<NodeWindowWrap>();

        for (int i = 0; i < nodeWindow.Children.Count; i++)
        {
            NodeWindowWrap node = new NodeWindowWrap();
            children.Add(TranslateToNWWChild(node, nodeWindow.Children[i], nodeWindowWrap));
        }
        nodeWindowWrap.Children = children;

        return nodeWindowWrap;
    }

    /// <summary>
    /// Take information from a NodeWindow class and put it into a NodeWindowWrap class, but set the parent extra to avoid a stack overflow
    /// </summary>
    /// <param name="nodeWindowWrap">Class to fill</param>
    /// <param name="nodeWindow">Class to read from</param>
    /// <param name="parent">Parent of the class to fill</param>
    /// <returns>Returns the filled class</returns>
    private static NodeWindowWrap TranslateToNWWChild(NodeWindowWrap nodeWindowWrap, NodeWindow nodeWindow, NodeWindowWrap parent)
    {
        if (nodeWindowWrap == null)
        {
            nodeWindowWrap = new NodeWindowWrap();
        }

        nodeWindowWrap.WindowRect = nodeWindow.WindowRect;
        nodeWindowWrap.Name = nodeWindow.Name;
        nodeWindowWrap.HasParent = nodeWindow.HasParent;

        if (parent != null)
            nodeWindowWrap.Parent = parent;

        List<NodeWindowWrap> children = new List<NodeWindowWrap>();

        for (int i = 0; i < nodeWindow.Children.Count; i++)
        {
            NodeWindowWrap node = new NodeWindowWrap();
            children.Add(TranslateToNWWChild(node, nodeWindow.Children[i], nodeWindowWrap));
        }
        nodeWindowWrap.Children = children;

        return nodeWindowWrap;
    }
    **/
}
