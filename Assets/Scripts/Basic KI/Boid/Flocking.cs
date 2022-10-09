using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class Flocking : Node
{
    private Transform _thisTransform;
    private Animator _thisAnimator;
    private NavMeshAgent _agent;
    private BoidMovement _boidMovement;
    private Vector3? _mousePos;
    private BoidSettings _settings;
    private Vector3 _destination;

    public Flocking(Transform transform, NavMeshAgent agent, BoidMovement boidMovement, BoidSettings settings)
    {
        _thisTransform = transform;
        _thisAnimator = transform.GetComponent<Animator>();
        _agent = agent;
        _boidMovement = boidMovement;
        _settings = settings;
    }

    public override ENodeState CalculateState()
    {
        if (_agent.speed != _settings.WalkSpeed)
            _agent.speed = _settings.WalkSpeed;

        _destination = (Vector3)GetData("boidDestination");
        if (_agent.destination != _destination)
            _agent.destination = _destination;

        _agent.Move(_boidMovement.CurrentVelocity);
        return state = ENodeState.RUNNING;
    }
}
