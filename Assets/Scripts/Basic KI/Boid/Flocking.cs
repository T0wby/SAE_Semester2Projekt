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
    private Vector3? _mousePos;

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
        if (CheckIfTargetOnNavmesh() is not null)
            _agent.destination = (Vector3)CheckIfTargetOnNavmesh();
        _agent.Move(_boidMovement.CurrentVelocity.normalized);
        return state = ENodeState.RUNNING;
    }

    private Vector3? CheckIfTargetOnNavmesh()
    {
        _mousePos = MousePosition.GetMousePosition();

        if (_mousePos is not null)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition((Vector3)_mousePos, out hit, 1f, NavMesh.AllAreas))
            {
                return hit.position;
            }
            return null;
        }
        return null;
    }
}
