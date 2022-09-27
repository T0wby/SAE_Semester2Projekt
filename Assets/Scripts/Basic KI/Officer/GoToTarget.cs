using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class GoToTarget : Node
{
    private static int _enemyLayerMask = 1 << 6;
    private Transform _thisTransform;
    private Collider[] _colliders;
    private NavMeshAgent _agent;

    public GoToTarget(Transform transform, NavMeshAgent agent)
    {
        _thisTransform = transform;
        _agent = agent;
    }

    public override ENodeState CalculateState()
    {
        Transform targetTransform = (Transform)GetData("target");
        if (Vector3.Distance(_thisTransform.position, targetTransform.position) > 0.01f)
        {
            _agent.SetDestination(targetTransform.position);

            //_thisTransform.position = Vector3.MoveTowards(_thisTransform.position, targetTransform.position, OfficerBT.speed * Time.deltaTime);
            //_thisTransform.LookAt(targetTransform.position);
        }

        return state = ENodeState.RUNNING;
    }
}
