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
            nodeWindowWraps.Add(new NodeWindowWrap(nodeWindows[i]));
        }

        for (int i = 0; i < connectedWindows.Count; i++)
        {
            if (connectedWindows[i].Parent == null && connectedWindows[i].Child == null)
                continue;

            WindowConnectionsWrap singleConnectionsWrap = new WindowConnectionsWrap();
            if (connectedWindows[i].Parent != null)
            {
                singleConnectionsWrap.Parent = new NodeWindowWrap(connectedWindows[i].Parent);
            }
            if (connectedWindows[i].Child != null)
            {
                singleConnectionsWrap.Child = new NodeWindowWrap(connectedWindows[i].Child);
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
            NodeWindowWrap nodeWindowWrap = new NodeWindowWrap();
            nodeWindowWrap = nodeWindowListWrap._nodeWindowWraps[i];
            nodeWindow.WindowRect = nodeWindowWrap._windowRect;
            nodeWindow.WindowNode = nodeWindowWrap._windowNode;
            nodeWindow.HasParent = nodeWindowWrap._hasParent;
            nodeWindow.Parent = nodeWindowWrap._parent;
            nodeWindow.Children = nodeWindowWrap._children;

            nodeWindows.Add(nodeWindow);
        }
        for (int i = 0; i < nodeWindowListWrap._connections.Count; i++)
        {
            // Parent
            NodeWindow nodeWindowParent = new NodeWindow();
            NodeWindowWrap nodeWindowWrapParent = new NodeWindowWrap();
            nodeWindowWrapParent = nodeWindowListWrap._connections[i].Parent;
            if (nodeWindowWrapParent != null)
            {
                nodeWindowParent.WindowRect = nodeWindowWrapParent._windowRect;
                nodeWindowParent.WindowNode = nodeWindowWrapParent._windowNode;
                nodeWindowParent.HasParent = nodeWindowWrapParent._hasParent;
                nodeWindowParent.Parent = nodeWindowWrapParent._parent;
                nodeWindowParent.Children = nodeWindowWrapParent._children;
            }
            

            // Child
            NodeWindow nodeWindowChild = new NodeWindow();
            NodeWindowWrap nodeWindowWrapChild = new NodeWindowWrap();
            nodeWindowWrapChild = nodeWindowListWrap._connections[i].Child;
            if (nodeWindowWrapChild != null)
            {
                nodeWindowChild.WindowRect = nodeWindowWrapChild._windowRect;
                nodeWindowChild.WindowNode = nodeWindowWrapChild._windowNode;
                nodeWindowChild.HasParent = nodeWindowWrapChild._hasParent;
                nodeWindowChild.Parent = nodeWindowWrapChild._parent;
                nodeWindowChild.Children = nodeWindowWrapChild._children;
            }
            
            connectedWindows.Add(new WindowConnections(nodeWindowParent, nodeWindowChild));
        }

        return nodeWindows;
    }
}
