using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using static Unity.Burst.Intrinsics.X86.Avx;

public class EnemyIsCertainType<T> : Node where T : BehaviorTree.MyTree
{
    public override ENodeState CalculateState()
	{
        object tmp = GetData("target");

        return CheckTarget(tmp);
    }

    private ENodeState CheckTarget(object target)
    {
        if (target is null)
            return state = ENodeState.FAILURE;

        Transform tmpTarget = (Transform)target;
        if (tmpTarget.gameObject.GetComponent<T>())
            return state = ENodeState.SUCCESS;

        return state = ENodeState.FAILURE;
    }
}
