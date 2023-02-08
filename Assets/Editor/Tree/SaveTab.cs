using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveTab
{
    private static string _filepath = $"{Application.dataPath}/";
    public static void SaveTree(List<NodeWindow> nodeWindows, string name)
    {
        List<NodeWindowWrap> nodeWindowWraps = new List<NodeWindowWrap>();

        for (int i = 0; i < nodeWindows.Count; i++)
        {
            nodeWindowWraps.Add(new NodeWindowWrap(nodeWindows[i]));
        }

        NodeWindowListWrap nodeWindowListWrap = new NodeWindowListWrap(nodeWindowWraps);

        string windowsJson = JsonUtility.ToJson(nodeWindowListWrap, true);

        using (StreamWriter writer = File.CreateText(_filepath + name))
        {
            writer.Write(windowsJson);
        }
    }

    public static List<NodeWindow> LoadTree(List<NodeWindow> nodeWindows, string name)
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

        return nodeWindows;
    }
}
