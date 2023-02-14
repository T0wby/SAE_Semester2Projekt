using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class SettingsDrawer
{
    private static ConnectionHandler _connectionHandler;
    private static string _fileName = "FileName";
    private static GUIContent _label = new GUIContent("Save/Load Name: ");

    public static ConnectionHandler ConnectionHandler { get => _connectionHandler; set => _connectionHandler = value; }
    public static string FileName { get => _fileName; set => _fileName = value; }

    public static void DrawSettings(WindowDrawer windowDrawer)
    {
        if (GUI.Button(new Rect(50, 50, 100, 100), "Save"))
        {
            SaveTab.SaveTree(windowDrawer.NodeWindows, _connectionHandler.ConnectedWindows, $"{_fileName}.json");
        }
        _fileName = EditorGUI.TextField(new Rect(200, 150, 300, 20), _label, _fileName);
        if (GUI.Button(new Rect(50, 175, 100, 100), "Load"))
        {
            SaveTab.LoadTree(windowDrawer.NodeWindows, _connectionHandler.ConnectedWindows, $"{_fileName}.json");
        }
        if (GUI.Button(new Rect(50, 300, 100, 100), "Diconnect All"))
        {
            _connectionHandler.DisconnectAllNodes(BTWindow._bTWindow.WindowDrawer.NodeWindows);
        }
    }
}
