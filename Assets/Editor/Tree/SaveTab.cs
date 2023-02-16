using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ConnectionHandler;

public static class SaveTab
{
    private static string _filepath = $"{Application.dataPath}/";

    #region Functions
    #region Saving/Loading
    /// <summary>
    /// Saves the current tree in a JSON file
    /// </summary>
    /// <param name="nodeWindows">List of all current active NodeWindows</param>
    /// <param name="connectedWindows">List of all current active Connections between the windows</param>
    /// <param name="name">Name of the savefile</param>
    public static void SaveTree(List<NodeWindow> nodeWindows, List<WindowConnections> connectedWindows, string name)
    {
        List<NodeWindowWrap> nodeWindowWraps = new List<NodeWindowWrap>();
        List<WindowConnectionsWrap> windowConnectionsWraps = new List<WindowConnectionsWrap>();

        #region Translate all current active Windows to wrapper class
        for (int i = 0; i < nodeWindows.Count; i++)
        {
            NodeWindowWrap nodeWindowWrap = new NodeWindowWrap();
            nodeWindowWrap = TranslateBasicInfoToSave(nodeWindowWrap, nodeWindows[i]);
            nodeWindowWraps.Add(nodeWindowWrap);
        }

        TranslateParentInfoToSave(nodeWindowWraps, nodeWindows);

        TranslateChildInfoToSave(nodeWindowWraps, nodeWindows);
        #endregion

        #region Translate all connections to a wrapper class and add to a wrapper list
        for (int i = 0; i < connectedWindows.Count; i++)
        {
            if (connectedWindows[i].Parent == null && connectedWindows[i].Child == null)
                continue;

            WindowConnectionsWrap singleConnectionsWrap = new WindowConnectionsWrap();
            if (connectedWindows[i].Parent != null)
            {
                singleConnectionsWrap.Parent = SearchMatchingNode(connectedWindows[i].Parent, nodeWindowWraps);
            }
            if (connectedWindows[i].Child != null)
            {
                singleConnectionsWrap.Child = SearchMatchingNode(connectedWindows[i].Child, nodeWindowWraps);
            }

            windowConnectionsWraps.Add(singleConnectionsWrap);
        }
        #endregion

        // Create wrapper List
        NodeWindowListWrap nodeWindowListWrap = new NodeWindowListWrap(nodeWindowWraps, windowConnectionsWraps);

        // Write List into a JSON file
        string windowsJson = JsonUtility.ToJson(nodeWindowListWrap, true);

        using (StreamWriter writer = File.CreateText(_filepath + name))
        {
            writer.Write(windowsJson);
        }
    }

    /// <summary>
    /// Load a saved tree from a JSON file
    /// </summary>
    /// <param name="nodeWindows">List of all current active NodeWindows</param>
    /// <param name="connectedWindows">List of all current active Connections between the windows</param>
    /// <param name="name">Name of the file you wish to load</param>
    /// <returns>List of Loaded NodeWindows</returns>
    public static List<NodeWindow> LoadTree(List<NodeWindow> nodeWindows, List<WindowConnections> connectedWindows, string name)
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

        // Clearing the current active lists
        nodeWindows.Clear();
        connectedWindows.Clear();

        // Setting all Nodes
        for (int i = 0; i < nodeWindowListWrap._nodeWindowWraps.Count; i++)
        {
            NodeWindow nodeWindow = new NodeWindow();
            nodeWindow = TranslateBasicInfoBack(nodeWindowListWrap._nodeWindowWraps[i], nodeWindow);

            nodeWindows.Add(nodeWindow);
        }

        TranslateParentInfoBack(nodeWindowListWrap._nodeWindowWraps, nodeWindows);

        TranslateChildInfoBack(nodeWindowListWrap._nodeWindowWraps, nodeWindows);

        // Setting Connection list
        for (int i = 0; i < nodeWindows.Count; i++)
        {
            for (int x = 0; x < nodeWindows[i].Children.Count; x++)
            {
                connectedWindows.Add(new WindowConnections(nodeWindows[i], nodeWindows[i].Children[x]));
            }
        }

        return nodeWindows;
    }
    #endregion

    #region Translations
    #region Translate from NodeWindow to NodeWindowWrap
    /// <summary>
    /// Translate the basic info from a NodeWindow object to a NodeWindowWrap object
    /// </summary>
    /// <param name="nodeWindowWrap">NodeWindowWrap to translate into</param>
    /// <param name="nodeWindow">NodeWindow to translate from</param>
    /// <returns>Translated NodeWindowWrap object</returns>
    private static NodeWindowWrap TranslateBasicInfoToSave(NodeWindowWrap nodeWindowWrap, NodeWindow nodeWindow)
    {
        if (nodeWindowWrap == null)
        {
            nodeWindowWrap = new NodeWindowWrap();
        }

        nodeWindowWrap.WindowRect = nodeWindow.WindowRect;
        nodeWindowWrap.Name = nodeWindow.Name;
        nodeWindowWrap.HasParent = nodeWindow.HasParent;

        return nodeWindowWrap;
    }

    /// <summary>
    /// Translate the parent info from all active NodeWindow objects to all previously basic translated NodeWindowWraps
    /// </summary>
    /// <param name="nodeWindowWraps">NodeWindowWraps to translate into</param>
    /// <param name="nodeWindows">NodeWindows to translate from</param>
    private static void TranslateParentInfoToSave(List<NodeWindowWrap> nodeWindowWraps, List<NodeWindow> nodeWindows)
    {
        for (int i = 0; i < nodeWindows.Count; i++)
        {
            if (!nodeWindows[i].HasParent) continue;

            nodeWindowWraps[i].ParentWindowRect = nodeWindows[i].Parent.WindowRect;
            nodeWindowWraps[i].ParentName = nodeWindows[i].Parent.Name;
        }
    }

    /// <summary>
    /// Translate the children info from all active NodeWindow objects to all previously basic translated NodeWindowWraps
    /// </summary>
    /// <param name="nodeWindowWraps">NodeWindowWraps to translate into</param>
    /// <param name="nodeWindows">NodeWindows to translate from</param>
    private static void TranslateChildInfoToSave(List<NodeWindowWrap> nodeWindowWraps, List<NodeWindow> nodeWindows)
    {
        for (int i = 0; i < nodeWindows.Count; i++)
        {
            List<NodeWindow> children = nodeWindows[i].Children;
            int childCount = children.Count;
            if (childCount <= 0) continue;

            nodeWindowWraps[i].Children = new List<NodeWindowWrap>();

            for (int x = 0; x < childCount; x++)
            {
                NodeWindowWrap child = SearchMatchingNode(children[x], nodeWindowWraps);

                if (child != null)
                    nodeWindowWraps[i].Children.Add(child);
            }
        }
    }
    #endregion

    #region Translate from NodeWindowWrap to NodeWindow
    /// <summary>
    /// Translate the basic info from a NodeWindowWrap object to a NodeWindow object
    /// </summary>
    /// <param name="nodeWindowWrap">NodeWindowWrap to translate from</param>
    /// <param name="nodeWindow">NodeWindow to translate into</param>
    /// <returns>Translated NodeWindow object</returns>
    private static NodeWindow TranslateBasicInfoBack(NodeWindowWrap nodeWindowWrap, NodeWindow nodeWindow)
    {
        if (nodeWindow == null)
        {
            nodeWindow = new NodeWindow();
        }

        nodeWindow.WindowRect = nodeWindowWrap.WindowRect;
        nodeWindow.Name = nodeWindowWrap.Name;
        nodeWindow.HasParent = nodeWindowWrap.HasParent;

        return nodeWindow;
    }

    /// <summary>
    /// Translate the parent info from all active NodeWindowWrap objects to all previously basic translated NodeWindows
    /// </summary>
    /// <param name="nodeWindowWraps">NodeWindowWraps to translate from</param>
    /// <param name="nodeWindows">NodeWindows to translate into</param>
    private static void TranslateParentInfoBack(List<NodeWindowWrap> nodeWindowWraps, List<NodeWindow> nodeWindows)
    {
        for (int i = 0; i < nodeWindowWraps.Count; i++)
        {
            if (!nodeWindowWraps[i].HasParent) continue;

            for (int x = 0; x < nodeWindows.Count; x++)
            {
                if (nodeWindowWraps[i].ParentName == nodeWindows[x].Name && nodeWindowWraps[i].ParentWindowRect == nodeWindows[x].WindowRect)
                {
                    nodeWindows[i].Parent = nodeWindows[x];
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Translate the children info from all active NodeWindowWrap objects to all previously basic translated NodeWindows
    /// </summary>
    /// <param name="nodeWindowWraps">NodeWindowWraps to translate from</param>
    /// <param name="nodeWindows">NodeWindows to translate into</param>
    private static void TranslateChildInfoBack(List<NodeWindowWrap> nodeWindowWraps, List<NodeWindow> nodeWindows)
    {
        for (int i = 0; i < nodeWindowWraps.Count; i++)
        {
            List<NodeWindowWrap> children = nodeWindowWraps[i].Children;
            int childCount = children.Count;
            if (childCount <= 0) continue;

            for (int x = 0; x < childCount; x++)
            {
                NodeWindow child = SearchMatchingNode(children[x], nodeWindows);

                if (child != null)
                    nodeWindows[i].Children.Add(child);
            }
        }
    }
    #endregion
    #endregion

    #region Searches
    /// <summary>
    /// Takes a NodeWindowWrap object and looks in a list of NodeWindows for an object with matching basic infos
    /// </summary>
    /// <param name="nodeWindowWrap">NodeWindowWrap object to compare for</param>
    /// <param name="nodeWindows">List of all active NodeWindows</param>
    /// <returns>Returns the matching NodeWindow object</returns>
    private static NodeWindow SearchMatchingNode(NodeWindowWrap nodeWindowWrap, List<NodeWindow> nodeWindows)
    {
        for (int i = 0; i < nodeWindows.Count; i++)
        {
            if (nodeWindowWrap.Name == nodeWindows[i].Name && nodeWindowWrap.WindowRect == nodeWindows[i].WindowRect)
            {
                return nodeWindows[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Takes a NodeWindow object and looks in a list of NodeWindowWraps for an object with matching basic infos
    /// </summary>
    /// <param name="nodeWindow">NodeWindow object to compare for</param>
    /// <param name="nodeWindowWraps">List of all loaded NodeWindowWraps</param>
    /// <returns>Returns the matching NodeWindowWrap object</returns>
    private static NodeWindowWrap SearchMatchingNode(NodeWindow nodeWindow, List<NodeWindowWrap> nodeWindowWraps)
    {
        for (int i = 0; i < nodeWindowWraps.Count; i++)
        {
            if (nodeWindow.Name == nodeWindowWraps[i].Name && nodeWindow.WindowRect == nodeWindowWraps[i].WindowRect)
            {
                return nodeWindowWraps[i];
            }
        }
        return null;
    }  
    #endregion
    #endregion
}
