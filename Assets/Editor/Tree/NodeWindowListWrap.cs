using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class NodeWindowListWrap
{
    public List<NodeWindowWrap> _nodeWindowWraps;

	public NodeWindowListWrap()
	{
        _nodeWindowWraps = null;
    }
	public NodeWindowListWrap(List<NodeWindowWrap> nodeWindowWraps)
	{
        _nodeWindowWraps = nodeWindowWraps;
    }
}
