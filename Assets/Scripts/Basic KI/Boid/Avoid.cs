using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class Avoid : Node
{
    private Transform _thisTransform;
    private Transform _targetTransform;
    private NavMeshAgent _agent;
    private BoidSettings _settings;

    public Avoid(Transform transform, NavMeshAgent agent, BoidSettings settings)
    {
        _thisTransform = transform;
        _agent = agent;
        _settings = settings;
    }

public override ENodeState CalculateState()
	{
        //TODO: Add a different Speed for Avoiding maybe
        if (_agent.speed != _settings.Speed)
            _agent.speed = _settings.Speed;

        _targetTransform = (Transform)GetData("target");
        _agent.Move((_thisTransform.position - _targetTransform.position).normalized);
        return state = ENodeState.RUNNING;
    }
}
