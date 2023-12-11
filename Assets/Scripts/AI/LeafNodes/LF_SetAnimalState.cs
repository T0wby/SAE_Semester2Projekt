using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LF_SetAnimalState : Node
{
    #region Fields
    private AAnimal _animal;
    private EAnimalStates _stateToSet; 
    #endregion

    #region Constructors
    public LF_SetAnimalState()
    {
        _stateToSet = EAnimalStates.None;
    }

    /// <summary>
    /// Changes the state of the animal
    /// </summary>
    /// <param name="animal">Animal in question</param>
    /// <param name="stateToSet">State to change to</param>
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
