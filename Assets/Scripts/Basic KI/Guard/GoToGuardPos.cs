using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class GoToGuardPos : Node
{
    private Transform _thisTransform;
    private Transform _guardpoint;
    private NavMeshAgent _agent;

    public GoToGuardPos(Transform transform, Transform guardpoint, NavMeshAgent agent)
    {
        _thisTransform = transform;
        _guardpoint = guardpoint;
        _agent = agent;
        _agent.speed = GuardBT.speed;
    }

    public override ENodeState CalculateState()
    {
        if (Vector3.Distance(_thisTransform.position, _guardpoint.position) < 0.01f)
        {
            _thisTransform.position = _guardpoint.position;
        }
        else
        {
            //_thisTransform.position = Vector3.MoveTowards(_thisTransform.position, _guardpoint.position, GuardBT.speed * Time.deltaTime);
            //_thisTransform.LookAt(_guardpoint.position);
            _agent.destination = _guardpoint.position;
            return state = ENodeState.RUNNING;
        }

        return state = ENodeState.FAILURE;
    }
}
