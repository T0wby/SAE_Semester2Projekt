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

    public Flocking(Transform transform, Transform[] waypoints, NavMeshAgent agent)
    {
        _thisTransform = transform;
        _waypoints = waypoints;
        _thisAnimator = transform.GetComponent<Animator>();
        _agent = agent;
    }

    public override ENodeState CalculateState()
    {

        return state = ENodeState.RUNNING;
    }

    
}
