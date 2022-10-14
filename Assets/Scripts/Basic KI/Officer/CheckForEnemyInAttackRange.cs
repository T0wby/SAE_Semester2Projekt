using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckForEnemyInAttackRange : Node
{
    private Transform _thisTransform;
    private float _range;

    public CheckForEnemyInAttackRange(Transform transform, float range)
    {
        _thisTransform = transform;
        _range = range;
    }

    public override ENodeState CalculateState()
    {
        object tmp = GetData("target");
        if (tmp is null)
            return state = ENodeState.FAILURE;

        Transform target = (Transform)tmp;
        if (Vector3.Distance(_thisTransform.position, target.position) <= _range)
        {
            return state = ENodeState.SUCCESS;
        }

        return state = ENodeState.FAILURE;
    }
}
