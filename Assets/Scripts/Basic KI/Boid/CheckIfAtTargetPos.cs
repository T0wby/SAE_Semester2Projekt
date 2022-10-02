using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckIfAtTargetPos : Node
{
    private Transform _thisTransform;
    private Vector3? _targetPos;


    public CheckIfAtTargetPos(Transform transform)
    {
        _thisTransform = transform;
        GetRoot(this).SetData("boidDestination", Vector3.zero);
    }

    public override ENodeState CalculateState()
    {
        _targetPos = (Vector3?)GetData("boidDestination");

        if (Vector3.Distance(_thisTransform.position, (Vector3)_targetPos) < 0.3f)
            return state = ENodeState.FAILURE;

        return state = ENodeState.SUCCESS;

    }
}
