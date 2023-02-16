using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LF_SetAnimalState : Node
{
    private AAnimal _animal;
    private EAnimalStates _stateToSet;

    #region Constructors
    public LF_SetAnimalState()
    {
        _stateToSet = EAnimalStates.None;
    }

    public LF_SetAnimalState(AAnimal animal, EAnimalStates stateToSet)
    {
        this._animal = animal;
        this._stateToSet = stateToSet;
    }
    #endregion

    #region Method
    public override ENodeState CalculateState()
    {
        _animal.State = _stateToSet;
        return ENodeState.SUCCESS;
    }
    #endregion
}
