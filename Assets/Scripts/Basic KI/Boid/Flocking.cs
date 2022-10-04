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
    private Vector3? _destination;
    private float _radius;

    public Flocking(Transform transform, NavMeshAgent agent, BoidMovement boidMovement, BoidSettings settings, float radius)
    {
        _thisTransform = transform;
        _thisAnimator = transform.GetComponent<Animator>();
        _agent = agent;
        _boidMovement = boidMovement;
        _settings = settings;
        _radius = radius;
    }

    public override ENodeState CalculateState()
    {
        if (_agent.speed != _settings.Speed)
            _agent.speed = _settings.Speed;

        if (CheckIfTargetOnNavmesh() is not null)
            _agent.destination = (Vector3)CheckIfTargetOnNavmesh();
        _agent.Move(_boidMovement.CurrentVelocity);
        return state = ENodeState.RUNNING;
    }

    private Vector3? CheckIfTargetOnNavmesh()
    {
        _mousePos = MousePosition.GetMousePosition();


        if (_mousePos is not null)
        {
            _destination = RandomizePos();
            GetRoot(this).SetData("boidDestination", _destination);
            NavMeshHit hit;
            if (NavMesh.SamplePosition((Vector3)_destination, out hit, 1f, NavMesh.AllAreas))
            {
                return hit.position;
            }
            return null;
        }
        return null;
    }

    private Vector3? RandomizePos()
    {
        return new Vector3(_mousePos.Value.x + Random.Range(-_radius, _radius), _mousePos.Value.y, _mousePos.Value.z + Random.Range(-_radius, _radius));
    }
}
