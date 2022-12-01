using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckIfNotAtGuardPos : Node
{
    private Transform _thisTransform;
    private Transform _guardpoint;
    private Animator _animator;


    public CheckIfNotAtGuardPos(Transform transform, Transform guardpoint, Animator animator)
    {
        _thisTransform = transform;
        _guardpoint = guardpoint;
        _animator = animator;
    }

    public override ENodeState CalculateState()
    {
        //if (_thisTransform.position.x == _guardpoint.position.x && _thisTransform.position.z == _guardpoint.position.z)
        //{
        //    SetAnimationBool(_animator, "IsWalking", false);
        //    return state = ENodeState.FAILURE;
        //}

        if (Vector3.SqrMagnitude(_guardpoint.position - _thisTransform.position) < 0.5f * 0.5f)
        {
            SetAnimationBool(_animator, "IsWalking", false);
            return state = ENodeState.FAILURE;
        }

        return state = ENodeState.SUCCESS;

    }
}
