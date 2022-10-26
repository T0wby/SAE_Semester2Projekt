using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

//TODO: Might be better to use sqrMagnitude.
public class CheckIfAtTargetPos : Node
{
    private Transform _thisTransform;
    private Vector3? _targetPos;
    private Vector3? _mousePos;
    private Vector3? _destination;
    private float _radius = 2f;
    private BoidSettings _settings;
    private NavMeshAgent _agent;


    public CheckIfAtTargetPos(Transform transform, BoidSettings settings, NavMeshAgent agent)
    {
        _thisTransform = transform;
        _settings = settings;
        _agent = agent;
    }

    public override ENodeState CalculateState()
    {
        _targetPos = CheckIfTargetOnNavmesh();

        //Vector3.SqrMagnitude((Vector3)_targetPos - _thisTransform.position)
        if (_targetPos is null || Vector3.Distance(_thisTransform.position, (Vector3)_targetPos) < 1.5f)
            return state = ENodeState.FAILURE;

        
        //_agent.isStopped = false;
        return state = ENodeState.SUCCESS;

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
