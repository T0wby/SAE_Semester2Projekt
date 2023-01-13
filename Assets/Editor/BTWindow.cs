using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BTWindow : EditorWindow
{
    private static BTWindow _bTWindow;
    private TabDrawer _tabDrawer;

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
    }

    private void OnGUI()
    {
        _tabDrawer.DrawTabs();

        switch (_tabDrawer.CurrentTab)
        {
            case Tabs.Preset:
                break;
            case Tabs.CurrentTree:
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
}
