using UnityEngine;
using BehaviorTree;

public class LF_EnemyIsCertainType<T> : Node where T : BehaviorTree.MyTree
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
