using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Microsoft.Win32.SafeHandles;
using UnityEngine;

public class LF_SetAlertState : Node
{
    private HideAI _hideAI;
    private bool _isAlerted;

    public LF_SetAlertState(HideAI hideAI, bool isAlerted)
    {
        _hideAI = hideAI;
        _isAlerted = isAlerted;
    }

    public override ENodeState CalculateState()
    {
        _hideAI.HasSeenEnemy = _isAlerted;

        return _isAlerted ? ENodeState.SUCCESS : ENodeState.FAILURE;
    }
}
