using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class GoToTarget : Node
{
    private static int _enemyLayerMask = 1 << 6;
    private Transform _thisTransform;
    private Collider[] _colliders;

    public GoToTarget(Transform transform)
    {
        _thisTransform = transform; 
    }

    public override ENodeState CalculateState()
    {
        Transform targetTransform = (Transform)GetData("target");
        if (Vector3.Distance(_thisTransform.position, targetTransform.position) > 0.01f)
        {
            _thisTransform.position = Vector3.MoveTowards(_thisTransform.position, targetTransform.position, OfficerBT.speed * Time.deltaTime);
            _thisTransform.LookAt(targetTransform.position);
        }

        return state = ENodeState.RUNNING;
    }
}
