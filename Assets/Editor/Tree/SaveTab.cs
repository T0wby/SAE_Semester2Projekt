using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveTab
{
    private static string _filepath = $"{Application.dataPath}/";
    public static void SaveTree(List<NodeWindow> nodeWindows, string name)
    {
        string windowsJson = JsonUtility.ToJson(nodeWindows, true);

        using (StreamWriter writer = File.CreateText(_filepath + name))
        {
            writer.Write(windowsJson);
        }
    }

    public static List<NodeWindow> LoadTree(List<NodeWindow> nodeWindows, string name)
    {
        if (!File.Exists(_filepath))
            return nodeWindows;

        string windows = string.Empty;

        using (StreamReader reader = File.OpenText(_filepath))
        {
            windows = reader.ReadToEnd();
        }

        JsonUtility.FromJsonOverwrite(windows, nodeWindows);
        return nodeWindows;
    }
}
