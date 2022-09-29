using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class ChooseDirection : Node 
{
    private RandomWalkTree _thisRandomWalkTree;

    public ChooseDirection(RandomWalkTree randomWalkTree)
	{
        _thisRandomWalkTree = randomWalkTree;
    }

    public override ENodeState CalculateState()
	{
        CheckForNewDirection();

        return ENodeState.SUCCESS;
	}

    private void CheckForNewDirection()
    {
        if (_thisRandomWalkTree.CurrentWalkTime >= _thisRandomWalkTree.MaxWalkTime)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            Node root = GetRoot(this);
            root.SetData("randomDirection", randomDirection);

            _thisRandomWalkTree.CurrentWalkTime = 0f;
            _thisRandomWalkTree.MaxWalkTime = Random.Range(5f, 10f);
        }
    }
}
