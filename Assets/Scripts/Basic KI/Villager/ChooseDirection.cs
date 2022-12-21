using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class ChooseDirection : Node 
{
    private HideAI _hideAI;

    public ChooseDirection(HideAI hideAI)
	{
        _hideAI = hideAI;
    }

    public override ENodeState CalculateState()
	{
        CheckForNewDirection();

        return ENodeState.SUCCESS;
	}

    private void CheckForNewDirection()
    {
        if (_hideAI.CurrentWalkTime >= _hideAI.MaxWalkTime)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            Node root = GetRoot(this);
            root.SetData("randomDirection", randomDirection);

            _hideAI.CurrentWalkTime = 0f;
        }
    }
}
