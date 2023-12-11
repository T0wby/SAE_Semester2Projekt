using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class LF_CheckForEnemyInAttackRange : Node
{
    #region Fields
    private Transform _thisTransform;
    private float _range;
    private Animator _animator;
    private float _sqrDist;
    #endregion

    #region Constructors
    public LF_CheckForEnemyInAttackRange()
    {
        _thisTransform = null;
        _range = 3f;
        _animator = null;
    }

    /// <summary>
    /// Checks if the found enemy is in range for an attack
    /// </summary>
    /// <param name="transform">The own transform</param>
    /// <param name="range">Range to check</param>
    /// <param name="animator">Own animator</param>
    public LF_CheckForEnemyInAttackRange(Transform transform, float range, Animator animator)
    {
        _thisTransform = transform;
        _range = range;
        _animator = animator;
    } 
    #endregion

    public override ENodeState CalculateState()
    {
        object tmp = GetData("target");
        if (tmp is null)
        {
            SetAnimationBool(_animator, "IsAttacking", false);
            return state = ENodeState.FAILURE;
        }

        Transform target = (Transform)tmp;

        _sqrDist = (target.position - _thisTransform.position).sqrMagnitude;
        if (_sqrDist <= (_range * _range))
        {
            return state = ENodeState.SUCCESS;
        }

        SetAnimationBool(_animator, "IsAttacking", false);
        return state = ENodeState.FAILURE;
    }
}
