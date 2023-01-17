using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BTWindow : EditorWindow
{
    private static BTWindow _bTWindow;
    private TabDrawer _tabDrawer;
    private NodeCreationDrawer _nodeCreation;
    private WindowDrawer _windowDrawer;


    private Rect _windowRect = new Rect(50,50,150,50);

    [MenuItem("Tools/BTWindow")]
    private static void ShowWindow()
    {
        _bTWindow = (BTWindow)EditorWindow.GetWindow(typeof(BTWindow));
        _bTWindow.titleContent = new GUIContent();
        _bTWindow.maxSize = new Vector2(1000, 1000);
        _bTWindow.minSize = new Vector2(500, 500);
    }

    private void OnEnable()
    {
        _tabDrawer= new TabDrawer();
        _nodeCreation = new NodeCreationDrawer();
        _windowDrawer = new WindowDrawer(_nodeCreation.CompositeNodes);
    }

    private void OnGUI()
    {
        _tabDrawer.DrawTabs();

        switch (_tabDrawer.CurrentTab)
        {
            case Tabs.Preset:
                _nodeCreation.DrawNodeCreationButtons(_windowDrawer);

                break;
            case Tabs.CurrentTree:
                _windowDrawer.RedrawWindows(this);
                break;
            case Tabs.Settings:
                if (GUI.Button(new Rect(50,50,100,100),"Save"))
                {

                }
                if (GUI.Button(new Rect(50, 175, 100, 100), "Load"))
                {

                }
                break;
            default:
                break;
        }
    }

    private void DrawWindow(int idx)
    {
        GUI.DragWindow();

        if (GUI.Button(new Rect(0, 0, 20, 20), "X"))
        {

        }
    }
}
