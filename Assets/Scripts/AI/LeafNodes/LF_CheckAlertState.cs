using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LF_CheckAlertState : Node
{
    private HideAI _hideAI;

    public LF_CheckAlertState()
    {
        _hideAI = null;
    }

    /// <summary>
    /// Check if the AI has seen an enemy
    /// </summary>
    /// <param name="hideAI">AI to check</param>
    public LF_CheckAlertState(HideAI hideAI)
    {
        _hideAI = hideAI;
    }

    public override ENodeState CalculateState()
    {
        if (_hideAI.HasSeenEnemy)
            return ENodeState.SUCCESS;

        return ENodeState.FAILURE;
    }
}
