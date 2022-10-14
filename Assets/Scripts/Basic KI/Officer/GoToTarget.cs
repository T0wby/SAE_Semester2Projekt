using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class GoToTarget : Node
{
    private Transform _thisTransform;
    private NavMeshAgent _agent;
    private BasicKISettings _settings;

    public GoToTarget(Transform transform, NavMeshAgent agent, BasicKISettings settings)
    {
        _thisTransform = transform;
        _agent = agent;
        _settings = settings;
    }

    public override ENodeState CalculateState()
    {
        Transform targetTransform = (Transform)GetData("target");

        if (_agent.speed != _settings.RunSpeed)
            _agent.speed = _settings.RunSpeed;

        if (Vector3.Distance(_thisTransform.position, targetTransform.position) > 0.1f)
        {
            if (_agent.destination != targetTransform.position)
                _agent.destination = targetTransform.position;
        }

        return state = ENodeState.RUNNING;
    }
}
