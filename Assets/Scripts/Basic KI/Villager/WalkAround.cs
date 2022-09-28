using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class WalkAround : Node
{
    private Transform _thisTransform;
    private NavMeshAgent _agent;
    private RandomWalkTree _thisRandomWalkTree;
    private float _speed;

    public WalkAround(Transform transform, NavMeshAgent agent,float speed, RandomWalkTree randomBT)
    {
        _thisTransform = transform;
        _agent = agent;
        _speed = speed;
        _thisRandomWalkTree = randomBT;
    }

    public override ENodeState CalculateState()
    {
        object tmp = GetData("randomDirection");
        if (tmp is null)
            return state = ENodeState.FAILURE;

        Vector2 randomDirection = (Vector2)tmp;

        if (_agent.speed != _speed)
            _agent.speed = _speed;

        Vector3 movementDirection = new Vector3(randomDirection.x, _thisTransform.position.y, randomDirection.y);

        _agent.destination = _thisTransform.position + movementDirection;
        _thisRandomWalkTree.CurrentWalkTime += Time.deltaTime;

        if (WalkTimerDone())
            return ENodeState.FAILURE;

        return ENodeState.RUNNING;
    }

    private bool WalkTimerDone()
    {
        if (_thisRandomWalkTree.CurrentWalkTime >= _thisRandomWalkTree.MaxWalkTime)
        {
            return true;
        }

        return false;
    }
}
