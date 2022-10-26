using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckIfHungry : Node
{
    private Villager _villager;

    public CheckIfHungry(Villager villager)
    {
            _villager = villager;
    }

    public override ENodeState CalculateState()
    {
        if (_villager.Hunger < 50f)
        {
            Debug.Log($"{_villager.name} hunger: {_villager.Hunger}");
            return ENodeState.SUCCESS;
        }

        return ENodeState.FAILURE;
    }
}
