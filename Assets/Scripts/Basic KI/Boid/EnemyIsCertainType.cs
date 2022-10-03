using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class EnemyIsCertainType<T> : Node where T : BehaviorTree.Tree
{
	public EnemyIsCertainType() {}

    public override ENodeState CalculateState()
	{
        object tmp = GetData("target");

        if (tmp is null)
            return state = ENodeState.FAILURE;

        Transform target = (Transform)tmp;
        if (target.gameObject.GetComponent<T>())
            return state = ENodeState.SUCCESS;

        return state = ENodeState.FAILURE;
    }
}
