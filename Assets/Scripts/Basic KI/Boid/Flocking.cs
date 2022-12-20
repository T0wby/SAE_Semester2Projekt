using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class Flocking : Node
{
    private Transform _thisTransform;
    private Animator _animator;
    private NavMeshAgent _agent;
    private BoidMovement _boidMovement;
    private BoidSettings _settings;
    private Vector3 _destination;

    public Flocking(Transform transform, NavMeshAgent agent, BoidMovement boidMovement, BoidSettings settings, Animator animator)
    {
        _thisTransform = transform;
        _animator = transform.GetComponent<Animator>();
        _agent = agent;
        _boidMovement = boidMovement;
        _settings = settings;
        _animator = animator;
    }

    public override ENodeState CalculateState()
    {
        if (_agent.speed != _settings.WalkSpeed)
            _agent.speed = _settings.WalkSpeed;

        SetAgentDestination();
        ManipulateAgentMovement();
        SetAnimationBool(_animator, "IsWalking", true);

        return state = ENodeState.RUNNING;
    }

    private void SetAgentDestination()
    {
        _destination = (Vector3)GetData("boidDestination");
        if (_agent.destination != _destination)
            _agent.destination = _destination;
    }

    private void ManipulateAgentMovement()
    {
        _agent.Move(_boidMovement.CurrentVelocity);
    }
}
