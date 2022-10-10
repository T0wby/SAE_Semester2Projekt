using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class Idle : Node
{
    private NavMeshAgent _agent;

    public Idle(NavMeshAgent agent)
	{
		_agent = agent;
	}

    public override ENodeState CalculateState()
	{
        _agent.Move(Vector3.zero);
        return ENodeState.RUNNING;
	}
}
