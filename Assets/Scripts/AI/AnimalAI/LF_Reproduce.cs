using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

public class LF_Reproduce : Node
{
    private AAnimal _animal;
    private AAnimal _partner;
    private AnimalAISettings _settings;
    private string _dataSet;
    private float _totalChance;

    #region Constructors
    public LF_Reproduce()
    {

    }

    public LF_Reproduce(AAnimal animal, AnimalAISettings settings, string dataSet)
    {
        _animal = animal;
        _settings = settings;
        _dataSet = dataSet;
    }
    #endregion

    #region Method
    public override ENodeState CalculateState()
    {
        _partner = (AAnimal)GetData(_dataSet);

        ResettingStates();

        return TryingToReproduce(); ;
    }

    private ENodeState TryingToReproduce()
    {
        _totalChance = _animal.ReproduceChance + _partner.ReproduceChance;
        if (_totalChance < Random.Range(0,1))
        {
            return ENodeState.FAILURE;
        }
        else
        {
            ReproduceTime();
            return ENodeState.SUCCESS;
        }
    }

    private void ResettingStates()
    {
        _partner.State = EAnimalStates.None;
        _animal.State = EAnimalStates.None;
    }
    #endregion

    #region Enumerator

    private async void ReproduceTime()
    {
        await Task.Delay(9000);
        _animal.Reproduce();
    }

    #endregion
}
