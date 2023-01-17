using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckForEnemyInAttackRange : Node
{
    private Transform _thisTransform;
    private float _range;
    private Animator _animator;

    public CheckForEnemyInAttackRange()
    {
        _thisTransform = null;
        _range = 3f;
        _animator = null;
    }

    public CheckForEnemyInAttackRange(Transform transform, float range, Animator animator)
    {
        _thisTransform = transform;
        _range = range;
        _animator = animator;
    }

    public override ENodeState CalculateState()
    {
        object tmp = GetData("target");
        if (tmp is null)
        {
            SetAnimationBool(_animator, "IsAttacking", false);
            return state = ENodeState.FAILURE;
        }

        Transform target = (Transform)tmp;
        if (Vector3.Distance(_thisTransform.position, target.position) <= _range)
        {
            return state = ENodeState.SUCCESS;
        }

        SetAnimationBool(_animator, "IsAttacking", false);
        return state = ENodeState.FAILURE;
    }
}
