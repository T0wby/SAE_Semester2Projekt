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
    private BoidMovement _boidMovement;
    private Animator _animator;

    public Avoid(Transform transform, NavMeshAgent agent, BasicKISettings settings, BoidMovement boidMovement, Animator animator)
    {
        _thisTransform = transform;
        _agent = agent;
        _settings = settings;
        _boidMovement = boidMovement;
        _animator = animator;
    }

    public override ENodeState CalculateState()
	{
        if (_agent.speed != _settings.RunSpeed)
            _agent.speed = _settings.RunSpeed;

        AvoidTarget();
        SetAnimationBool(_animator, "IsWalking", true);
        return state = ENodeState.RUNNING;
    }

    private void AvoidTarget()
    {
        _targetTransform = (Transform)GetData("target");

        Vector3 diff = (_thisTransform.position - _targetTransform.position) - _boidMovement.CurrentVelocity;
        _agent.velocity = diff * Time.deltaTime;
        _agent.velocity = Vector3.ClampMagnitude(_agent.velocity, _settings.RunSpeed);
    }
}
