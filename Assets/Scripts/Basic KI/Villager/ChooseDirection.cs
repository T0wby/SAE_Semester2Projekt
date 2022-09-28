using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class ChooseDirection : Node
{
    private VillagerBT _thisVillagerBT;

    public ChooseDirection(VillagerBT villagerBT)
	{
        _thisVillagerBT = villagerBT;
    }

    public override ENodeState CalculateState()
	{
        if (_thisVillagerBT.CurrentWalkTime >= _thisVillagerBT.MaxWalkTime)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;

            Node root = GetRoot(this);
            root.SetData("randomDirection", randomDirection);

            _thisVillagerBT.CurrentWalkTime = 0f;
            _thisVillagerBT.MaxWalkTime = Random.Range(5f, 10f);
        }
        

        return ENodeState.SUCCESS;
	}

}
