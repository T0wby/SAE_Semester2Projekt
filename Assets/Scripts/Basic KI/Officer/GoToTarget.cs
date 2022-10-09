using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class GoToTarget : Node
{
    private Transform _thisTransform;
    private NavMeshAgent _agent;

    public GoToTarget(Transform transform, NavMeshAgent agent)
    {
        _thisTransform = transform;
        _agent = agent;
    }

    public override ENodeState CalculateState()
    {
        Transform targetTransform = (Transform)GetData("target");
        if (Vector3.Distance(_thisTransform.position, targetTransform.position) > 0.1f)
        {
            if (_agent.destination != targetTransform.position)
                _agent.destination = targetTransform.position;
        }

        return state = ENodeState.RUNNING;
    }
}
