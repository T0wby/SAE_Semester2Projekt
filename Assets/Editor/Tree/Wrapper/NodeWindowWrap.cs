using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeWindowWrap
{
    public Rect _windowRect;
    //public Node _windowNode;
    public string _name;
    public bool _hasParent;
    public NodeWindowWrap _parent;
    public List<NodeWindowWrap> _children;


    public NodeWindowWrap()
    {
        _parent = null;
    }

	//public NodeWindowWrap(NodeWindow nodeWindow)
	//{
 //       _windowRect = nodeWindow.WindowRect;
 //       //_windowNode = nodeWindow.WindowNode;
 //       _name = nodeWindow.Name;
 //       _hasParent = nodeWindow.HasParent;
 //       _parent = nodeWindow.Parent;
 //       _children = nodeWindow.Children;
 //   }
}
