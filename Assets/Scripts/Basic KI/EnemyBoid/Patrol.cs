using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class Patrol : Node
{
    private Transform _thisTransform;
    private Animator _thisAnimator;
    private NavMeshAgent _agent;
    private Transform[] _waypoints;
    private int _currentWaypointIndex;
    private int _previousWaypointIndex = -1;
    private Vector3 _destination;

    public Patrol(Transform transform, Transform[] waypoints, NavMeshAgent agent)
    {
        _thisTransform = transform;
        _waypoints = waypoints;
        _thisAnimator = transform.GetComponent<Animator>();
        _agent = agent;
    }

    public override ENodeState CalculateState()
    {
        ChangeWaypointIndex();
        CheckIfAtTarget();

        return state = ENodeState.RUNNING;
    }

    /// <summary>
    /// Changes the waypointindex if the agent arrived at the current waypoint target
    /// </summary>
    private void ChangeWaypointIndex()
    {
        if (_previousWaypointIndex != _currentWaypointIndex)
        {
            Transform waypoint = _waypoints[_currentWaypointIndex];
            _previousWaypointIndex = _currentWaypointIndex;
            _destination = waypoint.position;
        }
    }

    /// <summary>
    /// Checks if we arrived at the destination and sets new one if needed
    /// </summary>
    private void CheckIfAtTarget()
    {
        if (Vector3.Distance(_thisTransform.position, _destination) < 3f)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
        }
        else if (_agent.destination != _destination)
        {
            _agent.destination = _destination;
        }
    }

}
