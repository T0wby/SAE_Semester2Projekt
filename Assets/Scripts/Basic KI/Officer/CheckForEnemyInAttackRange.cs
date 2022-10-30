using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckForEnemyInAttackRange : Node
{
    private Transform _thisTransform;
    private float _range;
    private Animator _animator;

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
            SetAnimationState(_animator, "IsAttacking", false);
            return state = ENodeState.FAILURE;
        }

        Transform target = (Transform)tmp;
        if (Vector3.Distance(_thisTransform.position, target.position) <= _range)
        {
            return state = ENodeState.SUCCESS;
        }

        SetAnimationState(_animator, "IsAttacking", false);
        return state = ENodeState.FAILURE;
    }

    /// <summary>
    /// Changes a bool value of an animator
    /// </summary>
    /// <param name="animator">Used animator</param>
    /// <param name="paramName">Exact name of the bool</param>
    /// <param name="state">bool value it should change to</param>
    private void SetAnimationState(Animator animator, string paramName, bool state)
    {
        if (animator.GetBool(paramName) != state)
        {
            animator.SetBool(paramName, state);
        }
    }
}
