using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using UnityEngine;

public class LF_StartActivity : Node
{
    #region Fields
    private AAnimal _thisAnimal;
    private AAnimal _partnerAnimal;
    private AnimalSearchArea _animalSearchArea;
    private EAnimalStates _eAnimalState;
    private float _totalChance; 
    #endregion

    #region Constructors
    public LF_StartActivity()
    {
        _eAnimalState = EAnimalStates.None;
    }

    /// <summary>
    /// Starts an activity fpr one of the states in EAnimalStates
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
                _thisAnimal.Eat((Grass)GetData("_eatTarget"));
                _animalSearchArea.GrassInRange.RemoveAll(grass => grass == null);
                return ENodeState.SUCCESS;
            case EAnimalStates.Drink:
                Transform wtarget = (Transform)GetData("_waterTarget");
                _thisAnimal.Drink();
                _animalSearchArea.WaterInRange.Remove(wtarget.gameObject);
                return ENodeState.SUCCESS;
            case EAnimalStates.Engaged:
                TryingToReproduce();
                break;
            default:
                Debug.Log($"Activity failed due to State beeing: {_eAnimalState}");
                return ENodeState.FAILURE;
        }
        return ENodeState.FAILURE;
    }

    /// <summary>
    /// Gets the partner information and checkts if the reproduce try was a success or not
    /// </summary>
    /// <returns>Failure or Success</returns>
    private ENodeState TryingToReproduce()
    {
        Transform partnerTransform= (Transform)GetData("_reproduceTransform");
        _partnerAnimal = partnerTransform.GetComponent<AAnimal>();

        if (_animalSearchArea.AnimalInRange.Contains(_partnerAnimal))
            _animalSearchArea.AnimalInRange.Remove(_partnerAnimal);

        _totalChance = _thisAnimal.ReproduceChance + _partnerAnimal.ReproduceChance;
        if (_totalChance < Random.Range(0, 100))
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

    /// <summary>
    /// Resetts the state and ReproduceUrge of the AAnimal
    /// </summary>
    private void ResettingStates()
    {
        _partnerAnimal.State = EAnimalStates.None;
        _partnerAnimal.ReproduceUrge = 0f;
        _thisAnimal.State = EAnimalStates.None;
        _thisAnimal.ReproduceUrge = 0f;
    }

    /// <summary>
    /// If the Reproducetry was a success we instantiate a new rabbit after a set delay
    /// </summary>
    private async void ReproduceTime()
    {
        await Task.Delay(9000);
        _thisAnimal.Reproduce();
    }
    #endregion
}
