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
    private float _radius;
    private float _speed;

    public Patrol(Transform transform, Transform[] waypoints, NavMeshAgent agent, float radius, float speed)
    {
        _thisTransform = transform;
        _waypoints = waypoints;
        _thisAnimator = transform.GetComponent<Animator>();
        _agent = agent;
        _radius = radius;
        _speed = speed;
    }

    public override ENodeState CalculateState()
    {

        if (_previousWaypointIndex != _currentWaypointIndex)
        {
            Transform waypoint = _waypoints[_currentWaypointIndex];
            _previousWaypointIndex = _currentWaypointIndex;
            //_destination = new Vector3(waypoint.position.x + Random.Range(-_radius, _radius), waypoint.position.y, waypoint.position.z + Random.Range(-_radius, _radius));
            _destination = waypoint.position;
        }


        if (Vector3.Distance(_thisTransform.position, _destination) < 3f)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
        }
        else if (_agent.destination != _destination)
        {
            _agent.destination = _destination;
        }

        return state = ENodeState.RUNNING;
    }

}
