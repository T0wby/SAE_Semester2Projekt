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
        if (_thisTransform.position.x == _guardpoint.position.x && _thisTransform.position.z == _guardpoint.position.z)
        {
            SetAnimationState(_animator, "IsWalking", false);
            return state = ENodeState.FAILURE;
        }

        return state = ENodeState.SUCCESS;

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
