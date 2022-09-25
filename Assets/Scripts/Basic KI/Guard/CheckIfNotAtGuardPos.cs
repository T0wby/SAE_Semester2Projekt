using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckIfNotAtGuardPos : Node
{
    private Transform _thisTransform;
    private Transform _guardpoint;


    public CheckIfNotAtGuardPos(Transform transform, Transform guardpoint)
    {
        _thisTransform = transform;
        _guardpoint = guardpoint;
    }

    public override ENodeState CalculateState()
    {
        if (_thisTransform.position == _guardpoint.position)
        {
            return state = ENodeState.FAILURE;
        }

        return state = ENodeState.SUCCESS;

    }
}
