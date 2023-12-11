using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using BehaviorTree;
using static Codice.Client.BaseCommands.BranchExplorer.ExplorerData.BrExTreeBuilder.BrExFilter;

public class NodeCreationDrawer
{
    #region Fields
    private List<Type> _compositeNodes;
    private List<Type> _leafNodes;
    #endregion

    #region Properties
    public List<Type> CompositeNodes { get => _compositeNodes; }
    #endregion

    #region Constructor
    public NodeCreationDrawer()
    {
        _compositeNodes = new List<Type>();
        _leafNodes = new List<Type>();

        FindScripts();
    } 
    #endregion

    /// <summary>
    /// Searches for two ScriptableObjects and sets the two different NodeTypes
    /// </summary>
    private void FindScripts()
    {
        string[] guidsC = AssetDatabase.FindAssets("CompositesList");
        string[] guidsL = AssetDatabase.FindAssets("LeafList");

        SearchForClasses(guidsC, _compositeNodes);
        SearchForClasses(guidsL, _leafNodes);
    }

    /// <summary>
    /// Checks if the given array of strings are ClassTypes that exist in the Project and adds it to the list
    /// </summary>
    /// <param name="soGUID">Globally Unique Identifier from the ScriptableObject</param>
    /// <param name="types">List of NodeTypes</param>
    private void SearchForClasses(string[] soGUID, List<Type> types)
    {
        List<Type> nodeTypes;
        ScriptableNodeTypes nodeTypesSO;

        foreach (string guid in soGUID)
        {
            // Load asset from the given GUID
            nodeTypesSO = AssetDatabase.LoadAssetAtPath<ScriptableNodeTypes>(AssetDatabase.GUIDToAssetPath(guid));
            nodeTypes = nodeTypesSO.ClassTypes;

            foreach (Type type in nodeTypes)
            {
                if (type == null)
                {
                    Debug.LogWarning($"ClassType is null");
                    continue;
                }

                if (!types.Contains(type))
                    types.Add(type);
            }
        }
    }

    /// <summary>
    /// Creates the buttons for both types of nodes
    /// </summary>
    /// <param name="windowDrawer">WindowDrawer, which handels the logic of the buttons</param>
    public void DrawNodeCreationButtons(WindowDrawer windowDrawer)
    {
        DrawButtons(_compositeNodes, windowDrawer, 5);
        DrawButtons(_leafNodes, windowDrawer, 205);
    }

    /// <summary>
    /// Creates NodeCreation Buttons
    /// </summary>
    /// <param name="typeNodes">List of Nodes to be drawn</param>
    /// <param name="windowDrawer">WindowDrawer, which handels the logic of the buttons</param>
    /// <param name="xPos">Start position for X</param>
    private void DrawButtons(List<Type> typeNodes, WindowDrawer windowDrawer, int xPos)
    {
        int nextLine = 1;
        int yPos = 0;
        for (int i = 0; i < typeNodes.Count; i++)
        {
            // Every tenth Button goes to the next line
            if(i%11 == 10)
            {
                nextLine++;
                yPos = 0;
            }
                

            if (GUI.Button(new Rect(xPos * nextLine, 40 * yPos + 30, 170, 40), typeNodes[i].FullName))
            {
                windowDrawer.AddWindow(50, 50, typeNodes[i].Name);
            }
            yPos++;
        }
    }
}
