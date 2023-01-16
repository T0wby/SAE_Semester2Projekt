using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using BehaviorTree;

public class NodeCreationDrawer
{
    private List<Type> _compositeNodes;
    private List<Type> _leafNodes;

    public NodeCreationDrawer()
    {
        _compositeNodes= new List<Type>();
        _leafNodes= new List<Type>();

        FindScripts();
    }

    private void FindScripts()
    {
        string[] guidsC = AssetDatabase.FindAssets("CompositesList");
        string[] guidsL = AssetDatabase.FindAssets("LeafList");

        List<Type> nodeTypes;
        ScriptableNodeTypes nodeTypesSO;

        foreach (string guid in guidsC)
        {
            nodeTypesSO = AssetDatabase.LoadAssetAtPath<ScriptableNodeTypes>(AssetDatabase.GUIDToAssetPath(guid));
            nodeTypes = nodeTypesSO.ClassTypes;

            foreach (Type type in nodeTypes)
            {
                if (type == null)
                {
                    Debug.LogWarning($"ClassType is null");
                    continue;
                }

                if (!_compositeNodes.Contains(type))
                    _compositeNodes.Add(type);
                    
            }
        }

        foreach (string guid in guidsL)
        {
            nodeTypesSO = AssetDatabase.LoadAssetAtPath<ScriptableNodeTypes>(AssetDatabase.GUIDToAssetPath(guid));
            nodeTypes = nodeTypesSO.ClassTypes;

            foreach (Type type in nodeTypes)
            {
                if (type == null)
                {
                    Debug.LogWarning($"ClassType is null");
                    continue;
                }

                if (!_leafNodes.Contains(type))
                    _leafNodes.Add(type);
            }
        }
    }

    public void DrawNodeCreationButtons(WindowDrawer windowDrawer)
    {
        DrawButtons(_compositeNodes, windowDrawer, 5);
        DrawButtons(_leafNodes, windowDrawer, 205);
    }

    private void DrawButtons(List<Type> typeNodes, WindowDrawer windowDrawer, int xPos)
    {
        for (int i = 0; i < typeNodes.Count; i++)
        {
            if (GUI.Button(new Rect(xPos, 40 * i + 30, 150, 40), typeNodes[i].FullName))
            {
                windowDrawer.AddWindow(50, 50, (Node)Activator.CreateInstance(typeNodes[i]));
            }
        }
    }
}
