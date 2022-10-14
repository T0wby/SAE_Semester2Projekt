using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class GoToPos : Node
{
    private Transform _thisTransform;
    private Transform _targetTransform;
    private NavMeshAgent _agent;
    private BasicKISettings _settings;

    public GoToPos(Transform transform, Transform targetTransform, NavMeshAgent agent, BasicKISettings settings)
    {
        _thisTransform = transform;
        _targetTransform = targetTransform;
        _agent = agent;
        _settings = settings;
    }

    public override ENodeState CalculateState()
    {
        if (_agent.speed != _settings.WalkSpeed)
            _agent.speed = _settings.WalkSpeed;

        if (Vector3.Distance(_thisTransform.position, _targetTransform.position) < 0.01f)
        {
            _thisTransform.position = _targetTransform.position;
        }
        else
        {
            _agent.destination = _targetTransform.position;
            return state = ENodeState.RUNNING;
        }

        return state = ENodeState.FAILURE;
    }
}
