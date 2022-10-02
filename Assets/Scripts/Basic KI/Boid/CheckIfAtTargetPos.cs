using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckIfAtTargetPos : Node
{
    private Transform _thisTransform;
    private Transform _targetPos;


    public CheckIfAtTargetPos(Transform transform, Transform targetPos)
    {
        _thisTransform = transform;
        _targetPos = targetPos;
    }

    public override ENodeState CalculateState()
    {
        if (_thisTransform.position == _targetPos.position)
        {
            return state = ENodeState.FAILURE;
        }

        return state = ENodeState.SUCCESS;

    }
}
