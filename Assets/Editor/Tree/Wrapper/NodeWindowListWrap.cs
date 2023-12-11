using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class NodeWindowListWrap
{
    public List<NodeWindowWrap> _nodeWindowWraps;
    public List<WindowConnectionsWrap> _connections;

    public NodeWindowListWrap()
	{
        _nodeWindowWraps = null;
    }
	public NodeWindowListWrap(List<NodeWindowWrap> nodeWindowWraps, List<WindowConnectionsWrap> connections)
	{
        _nodeWindowWraps = nodeWindowWraps;
        _connections = connections;
    }
}
