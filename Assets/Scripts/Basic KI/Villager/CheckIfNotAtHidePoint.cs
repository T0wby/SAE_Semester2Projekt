using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class CheckIfNotAtHidePoint : Node
{
    #region Fields

    private Transform _thisTransform;
    private Animator _animator;
    private object _hideDestination;

    #endregion

    #region Constructor

    public CheckIfNotAtHidePoint(Transform transform, Animator animator)
    {
        _thisTransform = transform;
        _animator = animator;
    }

    #endregion

    #region Loop
    public override ENodeState CalculateState()
    {
        _hideDestination = GetData("hideDestination");

        if (_hideDestination != null && Vector3.Distance(_thisTransform.position, (Vector3)_hideDestination) > 4f)
        {
            Debug.Log("Not at hide point");
            return ENodeState.SUCCESS;
        }
        SetAnimationBool(_animator, "IsWalking", false);
        GetRoot(this).DeleteData("hideDestination");

        return ENodeState.FAILURE;
    }
    #endregion
}
