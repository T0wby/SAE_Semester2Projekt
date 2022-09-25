using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckForEnemyInAttackRange : Node
{
    private static int _enemyLayerMask = 1 << 9;
    private Transform _thisTransform;

    public CheckForEnemyInAttackRange(Transform transform)
    {
        _thisTransform = transform;
    }

    public override ENodeState CalculateState()
    {
        object tmp = GetData("target");
        if (tmp is null)
            return state = ENodeState.FAILURE;

        Transform target = (Transform)tmp;
        if (Vector3.Distance(_thisTransform.position, target.position) <= OfficerBT.attackRange)
        {
            return state = ENodeState.SUCCESS;
        }

        return state = ENodeState.FAILURE;
    }
}
