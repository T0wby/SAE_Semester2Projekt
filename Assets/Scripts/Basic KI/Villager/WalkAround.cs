using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class WalkAround : Node
{
    private Transform _thisTransform;
    private NavMeshAgent _agent;
    private VillagerBT _thisVillagerBT;

    public WalkAround(Transform transform, NavMeshAgent agent, VillagerBT villagerBT)
    {
        _thisTransform = transform;
        _agent = agent;
        _thisVillagerBT = villagerBT;
    }

    public override ENodeState CalculateState()
    {
        object tmp = GetData("randomDirection");
        if (tmp is null)
            return state = ENodeState.FAILURE;

        Vector2 randomDirection = (Vector2)tmp;

        if (_agent.speed != VillagerBT.walkSpeed)
            _agent.speed = VillagerBT.walkSpeed;

        Vector3 movementDirection = new Vector3(randomDirection.x, _thisTransform.position.y, randomDirection.y);

        _agent.destination = _thisTransform.position + movementDirection;
        _thisVillagerBT.CurrentWalkTime += Time.deltaTime;

        if (WalkTimerDone())
            return ENodeState.FAILURE;

        return ENodeState.RUNNING;
    }

    private bool WalkTimerDone()
    {
        if (_thisVillagerBT.CurrentWalkTime >= _thisVillagerBT.MaxWalkTime)
        {
            return true;
        }

        return false;
    }
}