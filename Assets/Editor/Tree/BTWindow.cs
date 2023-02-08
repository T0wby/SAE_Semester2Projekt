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
    private StatusBarDrawer _statusDrawer;

    private float _panXPreset = 0;
    private float _panYPreset = 0;

    private Rect _windowRect = new Rect(50,50,150,50);

    [MenuItem("Tools/BTWindow")]
    private static void ShowWindow()
    {
        _bTWindow = (BTWindow)GetWindow(typeof(BTWindow));
        _bTWindow.titleContent = new GUIContent("BehaviourTree Window");
        _bTWindow.maxSize = new Vector2(2000, 2000);
        _bTWindow.minSize = new Vector2(700, 700);
    }

    private void OnEnable()
    {
        _tabDrawer= new TabDrawer();
        _statusDrawer = new StatusBarDrawer();
        _nodeCreation = new NodeCreationDrawer();
        _windowDrawer = new WindowDrawer(_nodeCreation.CompositeNodes);
    }

    private void OnGUI()
    {
        _tabDrawer.DrawTabs();
        _statusDrawer.DrawLabel(_bTWindow.position.width);
        _windowDrawer.SetStatusReference(_statusDrawer);

        switch (_tabDrawer.CurrentTab)
        {
            case Tabs.Preset:
                _nodeCreation.DrawNodeCreationButtons(_windowDrawer);
                break;
            case Tabs.CurrentTree:
                GUI.BeginGroup(new Rect(_panXPreset, _panYPreset, _bTWindow.maxSize.x * 5, _bTWindow.maxSize.y * 5));
                _windowDrawer.RedrawWindows(this);
                GUI.EndGroup();
                break;
            case Tabs.Settings:
                if (GUI.Button(new Rect(50,50,100,100),"Save"))
                {
                    SaveTab.SaveTree(_windowDrawer.NodeWindows, "Test.json");
                }
                if (GUI.Button(new Rect(50, 175, 100, 100), "Load"))
                {
                    SaveTab.LoadTree(_windowDrawer.NodeWindows, "Test.json");
                }
                break;
            default:
                break;
        }


        if (Event.current.type == EventType.MouseDrag)
        {
            _panXPreset += Event.current.delta.x;
            _panYPreset += Event.current.delta.y;
        }

        Repaint();
    }
}
