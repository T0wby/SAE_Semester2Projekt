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
    private BasicKISettings _settings;

    public Avoid(Transform transform, NavMeshAgent agent, BasicKISettings settings)
    {
        _thisTransform = transform;
        _agent = agent;
        _settings = settings;
    }

    public override ENodeState CalculateState()
	{
        if (_agent.speed != _settings.RunSpeed)
            _agent.speed = _settings.RunSpeed;

        _targetTransform = (Transform)GetData("target");
        _agent.Move((_thisTransform.position - _targetTransform.position)*0.005f);
        return state = ENodeState.RUNNING;
    }
}
