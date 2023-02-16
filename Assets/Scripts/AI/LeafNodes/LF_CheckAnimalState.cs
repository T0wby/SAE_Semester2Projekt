using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class LF_CheckAnimalState : Node
{
    private AAnimal _animal;
    private EAnimalStates _stateToCheck;
    private bool _isEqual;

    #region Constructors
    public LF_CheckAnimalState()
    {
        _stateToCheck = EAnimalStates.None;
        _isEqual = true;
    }

    public LF_CheckAnimalState(AAnimal animal, EAnimalStates stateToCheck, bool isEqual)
    {
        this._animal = animal;
        this._stateToCheck = stateToCheck;
        this._isEqual = isEqual;
    }
    #endregion

    #region Method
    public override ENodeState CalculateState()
    {
        if (_isEqual)
        {
            if (_animal.State == _stateToCheck)
                return ENodeState.SUCCESS;

            return ENodeState.FAILURE;
        }
        else
        {
            if (_animal.State != _stateToCheck)
                return ENodeState.SUCCESS;

            return ENodeState.FAILURE;
        }
    } 
    #endregion
}
