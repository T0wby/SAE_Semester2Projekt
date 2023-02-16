using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class LF_GoToTarget : Node
{
    private Transform _thisTransform;
    private NavMeshAgent _agent;
    private BasicKISettings _settings;
    private Animator _animator;

    public LF_GoToTarget()
    {

    }

    public LF_GoToTarget(Transform transform, NavMeshAgent agent, BasicKISettings settings, Animator animator)
    {
        _thisTransform = transform;
        _agent = agent;
        _settings = settings;
        _animator = animator;
    }

    public override ENodeState CalculateState()
    {
        Transform targetTransform = (Transform)GetData("target");

        //SetAnimationBool(_animator, "IsWalking", true);

        if (_agent.speed != _settings.RunSpeed)
            _agent.speed = _settings.RunSpeed;

        if (Vector3.Distance(_thisTransform.position, targetTransform.position) > 0.1f)
        {
            if (_agent.destination != targetTransform.position)
                _agent.destination = targetTransform.position;
        }
            //SetAnimationBool(_animator, "IsWalking", false);

        return state = ENodeState.RUNNING;
    }
}
