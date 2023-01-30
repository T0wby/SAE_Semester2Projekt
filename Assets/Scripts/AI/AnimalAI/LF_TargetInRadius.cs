using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LF_TargetInRadius : Node
{
    private AnimalSearchArea _animalSearchArea;
    private ETargetTypes _eTargetType;
    private Transform _thisTransform;
    private AAnimal _animal;

    #region Constructors
    public LF_TargetInRadius()
    {
        _eTargetType = ETargetTypes.None;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="animalSearchArea">Reference to the search area component</param>
    /// <param name="targetType">Type that you are searching for</param>
    public LF_TargetInRadius(Transform thisTransform, AAnimal animal, AnimalSearchArea animalSearchArea, ETargetTypes targetType)
    {
        _thisTransform = thisTransform;
        _animal = animal;
        _animalSearchArea = animalSearchArea;
        _eTargetType = targetType;
    }
    #endregion

    #region Method
    public override ENodeState CalculateState()
    {
        switch (_eTargetType)
        {
            case ETargetTypes.Grass:
                return CheckGrass(_animalSearchArea.GrassInRange);
            case ETargetTypes.Animal:
                return CheckAnimal(_animalSearchArea.AnimalInRange);
            case ETargetTypes.Water:
                return CheckWater(_animalSearchArea.WaterInRange);
            default:
                _animal.RandomMove = true;
                return ENodeState.FAILURE;
        }
    }

    private ENodeState CheckGrass(List<Grass> grass)
    {
        if (grass.Count > 0)
        {
            GetRoot(this).SetData("_eatTarget", grass[0]);
            GetRoot(this).SetData("_eatTargetTransform", grass[0].transform);
            _animal.RandomMove = false;
            return ENodeState.SUCCESS;
        }
        else
            return ENodeState.FAILURE;
    }

    private ENodeState CheckAnimal(List<AAnimal> animals)
    {
        if (animals.Count > 0)
        {
            if (CheckAnimalAvailability(animals))
                return ENodeState.SUCCESS;
            return ENodeState.FAILURE;
        }
        else
            return ENodeState.FAILURE;
    }

    private ENodeState CheckWater(List<GameObject> water)
    {
        if (water.Count > 0)
        {
            GetRoot(this).SetData("_waterTarget", water[0].transform);
            _animal.RandomMove = false;
            return ENodeState.SUCCESS;
        }
        else
            return ENodeState.FAILURE;
    }

    /// <summary>
    /// Checks if any of the found Animals is in a state to reproduce
    /// </summary>
    /// <param name="animals">List of found animals</param>
    /// <returns>Returns if a succesfull target was found</returns>
    private bool CheckAnimalAvailability(List<AAnimal> animals)
    {
        foreach (AAnimal animal in animals)
        {
            if (animal.State == EAnimalStates.ReproduceReady)
            {
                SetAnimalTarget(animal, _thisTransform.gameObject.GetComponent<AAnimal>(), "_reproduceTransform");
                animal.State = EAnimalStates.Engaged;
                GetRoot(this).SetData("_reproduceTransform", animal.transform);
                _animal.RandomMove = false;
                animal.RandomMove = false;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Set the target transform on the found animal
    /// </summary>
    /// <param name="targetedAnimal">Animal to set the target from</param>
    /// <param name="transformAnimal">Animal that this Node is on</param>
    /// <param name="key">string used to set the target</param>
    private void SetAnimalTarget(AAnimal targetedAnimal, AAnimal transformAnimal, string key)
    {
        if (transformAnimal == null)
        {
            Debug.Log("AAnimal component missing");
            return;
        }

        Node root = targetedAnimal.gameObject.GetComponent<RabbitBT>().Root;
        root.SetData(key, transformAnimal.transform);
    }
    #endregion
}
