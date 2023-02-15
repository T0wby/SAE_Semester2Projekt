using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ConnectionHandler;

public static class SaveTab
{
    private static string _filepath = $"{Application.dataPath}/";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nodeWindows"></param>
    /// <param name="connectedWindows"></param>
    /// <param name="name"></param>
    public static void SaveTree(List<NodeWindow> nodeWindows, List<WindowConnections> connectedWindows, string name)
    {
        List<NodeWindowWrap> nodeWindowWraps = new List<NodeWindowWrap>();
        List<WindowConnectionsWrap> windowConnectionsWraps = new List<WindowConnectionsWrap>();

        for (int i = 0; i < nodeWindows.Count; i++)
        {
            NodeWindowWrap nodeWindowWrap = new NodeWindowWrap();
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
                singleConnectionsWrap.Parent = SearchMatchingNode(connectedWindows[i].Parent, nodeWindowWraps);
            }
            if (connectedWindows[i].Child != null)
            {
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nodeWindows"></param>
    /// <param name="connectedWindows"></param>
    /// <param name="name"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nodeWindowWrap"></param>
    /// <param name="nodeWindow"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nodeWindowWraps"></param>
    /// <param name="nodeWindows"></param>
    private static void TranslateParentInfoToSave(List<NodeWindowWrap> nodeWindowWraps, List<NodeWindow> nodeWindows)
    {
        for (int i = 0; i < nodeWindows.Count; i++)
        {
            if (!nodeWindows[i].HasParent) continue;

            nodeWindowWraps[i].ParentWindowRect = nodeWindows[i].Parent.WindowRect;
            nodeWindowWraps[i].ParentName = nodeWindows[i].Parent.Name;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nodeWindowWraps"></param>
    /// <param name="nodeWindows"></param>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nodeWindowWrap"></param>
    /// <param name="nodeWindow"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nodeWindowWraps"></param>
    /// <param name="nodeWindows"></param>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nodeWindowWraps"></param>
    /// <param name="nodeWindows"></param>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nodeWindowWrap"></param>
    /// <param name="nodeWindows"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nodeWindow"></param>
    /// <param name="nodeWindowWraps"></param>
    /// <returns></returns>
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
}
