using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeWindowWrap
{
    public Rect WindowRect;
    public string Name;
    public bool HasParent;
    public NodeWindowWrap Parent;
    public List<NodeWindowWrap> Children;


    public NodeWindowWrap()
    {
        Parent = null;
    }
}
