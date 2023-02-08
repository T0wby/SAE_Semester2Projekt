using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SettingsDrawer
{
    private static ConnectionHandler _connectionHandler;

    public static ConnectionHandler ConnectionHandler { get => _connectionHandler; set => _connectionHandler = value; }

    public static void DrawSettings(WindowDrawer windowDrawer)
    {
        if (GUI.Button(new Rect(50, 50, 100, 100), "Save"))
        {
            SaveTab.SaveTree(windowDrawer.NodeWindows, _connectionHandler.ConnectedWindows, "Test.json");
        }
        if (GUI.Button(new Rect(50, 175, 100, 100), "Load"))
        {
            SaveTab.LoadTree(windowDrawer.NodeWindows, _connectionHandler.ConnectedWindows, "Test.json");
            _connectionHandler.DrawConnections();
        }
    }
}
