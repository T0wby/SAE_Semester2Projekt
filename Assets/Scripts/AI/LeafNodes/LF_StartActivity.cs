using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using UnityEngine;

public class LF_StartActivity : Node
{
    private AAnimal _thisAnimal;
    private AAnimal _partnerAnimal;
    private AnimalSearchArea _animalSearchArea;
    private EAnimalStates _eAnimalState;
    private float _totalChance;

    #region Constructors
    public LF_StartActivity()
    {
        _eAnimalState = EAnimalStates.None;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="thisAnimal">Animal to perform the activity on</param>
    /// <param name="targetType">Type of activity you wish to perform</param>
    public LF_StartActivity(AAnimal thisAnimal, EAnimalStates state, AnimalSearchArea animalSearchArea)
    {
        _thisAnimal = thisAnimal;
        _eAnimalState = state;
        _animalSearchArea = animalSearchArea;
    }
    #endregion

    #region Method
    public override ENodeState CalculateState()
    {
        switch (_eAnimalState)
        {
            case EAnimalStates.Eat:
                RemoveTargetFromList(_animalSearchArea, _eAnimalState);
                _thisAnimal.Eat((Grass)GetData("_eatTarget"));
                return ENodeState.SUCCESS;
            case EAnimalStates.Drink:
                RemoveTargetFromList(_animalSearchArea, _eAnimalState);
                _thisAnimal.Drink();
                return ENodeState.SUCCESS;
            case EAnimalStates.Engaged:
                RemoveTargetFromList(_animalSearchArea, _eAnimalState);
                TryingToReproduce();
                break;
            default:
                Debug.Log($"Activity failed due to State beeing: {_eAnimalState}");
                return ENodeState.FAILURE;
        }
        return ENodeState.FAILURE;
    }

    private ENodeState TryingToReproduce()
    {
        Transform partnerTransform= (Transform)GetData("_reproduceTransform");
        _partnerAnimal = partnerTransform.GetComponent<AAnimal>();

        if (_animalSearchArea.AnimalInRange.Contains(_partnerAnimal))
            _animalSearchArea.AnimalInRange.Remove(_partnerAnimal);

        _totalChance = _thisAnimal.ReproduceChance + _partnerAnimal.ReproduceChance;
        if (_totalChance < Random.Range(0, 1))
        {
            ResettingStates();
            return ENodeState.FAILURE;
        }
        else
        {
            ResettingStates();
            ReproduceTime();
            return ENodeState.SUCCESS;
        }
    }

    private void ResettingStates()
    {
        _partnerAnimal.State = EAnimalStates.None;
        _thisAnimal.State = EAnimalStates.None;
    }

    private async void ReproduceTime()
    {
        await Task.Delay(9000);
        _thisAnimal.Reproduce();
    }

    private void RemoveTargetFromList(AnimalSearchArea animalSearchArea, EAnimalStates state)
    {
        switch (state)
        {
            case EAnimalStates.Eat:
                animalSearchArea.GrassInRange.RemoveAt(0);
                break;
            case EAnimalStates.Drink:
                animalSearchArea.WaterInRange.RemoveAt(0);
                break;
            default:
                break;
        }
    }

    #endregion
}
