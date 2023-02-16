using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using BehaviorTree;

public class LF_PatrolWait : Node
{
    #region Fields
    private Transform _thisTransform;
    private Animator _thisAnimator;
    private NavMeshAgent _agent;
    private Transform[] _waypoints;
    private int _currentWaypointIndex;
    private int _previousWaypointIndex = -1;
    private Vector3 _destination;


    private float _waitTime = 1f;
    private float _waitCounter = 0f;
    private float _radius;
    private float _speed;
    private bool _waiting = false;
    private Animator _animator;
    #endregion

    #region Constructor
    public LF_PatrolWait(Transform transform, Transform[] waypoints, NavMeshAgent agent, float radius, float speed, Animator animator)
    {
        _thisTransform = transform;
        _waypoints = waypoints;
        _thisAnimator = transform.GetComponent<Animator>();
        _agent = agent;
        _radius = radius;
        _speed = speed;
        _animator = animator;
    }
    #endregion

    public override ENodeState CalculateState()
    {
        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime)
                _waiting = false;
        }
        else
        {
            if (_previousWaypointIndex != _currentWaypointIndex)
            {
                Transform waypoint = _waypoints[_currentWaypointIndex];
                _previousWaypointIndex = _currentWaypointIndex;
                _destination = new Vector3(waypoint.position.x + Random.Range(-_radius, _radius), waypoint.position.y, waypoint.position.z + Random.Range(-_radius, _radius));
            }


            if (Vector3.Distance(_thisTransform.position, _destination) < 1f)
            {
                _waitCounter = 0f;
                _waiting = true;
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
            }
            else if (_agent.destination != _destination)
            {
                _agent.destination = _destination;
            }
        }
        return state = ENodeState.RUNNING;
    }
}
