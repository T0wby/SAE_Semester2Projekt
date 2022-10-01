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
    private Transform[] _waypoints;
    private int _currentWaypointIndex;
    private BoidMovement _boidMovement;

    public Flocking(Transform transform, Transform[] waypoints, NavMeshAgent agent, BoidMovement boidMovement)
    {
        _thisTransform = transform;
        _waypoints = waypoints;
        _thisAnimator = transform.GetComponent<Animator>();
        _agent = agent;
        _boidMovement = boidMovement;
    }

    public override ENodeState CalculateState()
    {
        //TODO: Use mouse right to set agent destination
        _agent.destination = _waypoints[0].position;
        _agent.Move(_boidMovement.CurrentVelocity.normalized);
        return state = ENodeState.RUNNING;
    }

    
}
