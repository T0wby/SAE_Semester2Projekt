using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine.AI;
using UnityEngine;

public class Flee : Node
{
    private Transform _thisTransform;
    private NavMeshAgent _agent;

    public Flee(Transform transform, NavMeshAgent agent)
    {
        _thisTransform = transform;
        _agent = agent;
    }

    public override ENodeState CalculateState()
    {
        if (_agent.speed != VillagerBT.fleeSpeed)
            _agent.speed = VillagerBT.fleeSpeed;

        Transform targetTransform = (Transform)GetData("target");

        if (Vector3.Distance(_thisTransform.position, targetTransform.position) > VillagerBT.safeRange)
            return ENodeState.FAILURE;

        _agent.destination = _thisTransform.position + (_thisTransform.position - targetTransform.position).normalized;

        return ENodeState.RUNNING;
    }

}
