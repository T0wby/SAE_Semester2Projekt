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

    private float panX = 0;
    private float panY = 0;

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
        _nodeCreation = new NodeCreationDrawer();
        _windowDrawer = new WindowDrawer(_nodeCreation.CompositeNodes);
    }

    private void OnGUI()
    {
        
        GUI.BeginGroup(new Rect(panX, panY, _bTWindow.maxSize.x * 5, _bTWindow.maxSize.y * 5));

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
                    _bTWindow.SaveChanges();
                }
                if (GUI.Button(new Rect(50, 175, 100, 100), "Load"))
                {
                    
                }
                break;
            default:
                break;
        }

        GUI.EndGroup();

        if (Event.current.type == EventType.MouseDrag)
        {
            panX += Event.current.delta.x;
            panY += Event.current.delta.y;
        }

        Repaint();
    }
}
