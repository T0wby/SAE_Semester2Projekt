using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Microsoft.Win32.SafeHandles;
using UnityEngine;

public class LF_SetAlertState : Node
{
    #region Fields
    private HideAI _hideAI;
    private bool _isAlerted;
    #endregion

    #region Constructors
    public LF_SetAlertState()
    {
        _hideAI = null;
        _isAlerted = false;
    }

    /// <summary>
    /// Sets the alert bool of a HideAI
    /// </summary>
    /// <param name="hideAI">HideAI in question</param>
    /// <param name="isAlerted">Bool to change to</param>
    public LF_SetAlertState(HideAI hideAI, bool isAlerted)
    {
        _hideAI = hideAI;
        _isAlerted = isAlerted;
    } 
    #endregion

    public override ENodeState CalculateState()
    {
        _hideAI.HasSeenEnemy = _isAlerted;

        return _isAlerted ? ENodeState.SUCCESS : ENodeState.FAILURE;
    }
}
