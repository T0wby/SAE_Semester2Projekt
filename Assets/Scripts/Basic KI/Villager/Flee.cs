using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine.AI;
using UnityEngine;

public class Flee : Node
{
    private Transform _thisTransform;
    private NavMeshAgent _agent;
    private RandomWalkTree _thisRandomWalkTree;

    #region Constructors
    public Flee(Transform transform, NavMeshAgent agent)
    {
        _thisTransform = transform;
        _agent = agent;
    }

    public Flee(Transform transform, NavMeshAgent agent, RandomWalkTree randomWalkTree)
    {
        _thisTransform = transform;
        _agent = agent;
        _thisRandomWalkTree = randomWalkTree;
    }
    #endregion

    public override ENodeState CalculateState()
    {
        if (_agent.speed != VillagerBT.fleeSpeed)
            _agent.speed = VillagerBT.fleeSpeed;

        Transform targetTransform = (Transform)GetData("target");

        if (Vector3.Distance(_thisTransform.position, targetTransform.position) > VillagerBT.safeRange)
            return ENodeState.FAILURE;

        ResetRandomDirection();

        _agent.destination = _thisTransform.position + (_thisTransform.position - targetTransform.position).normalized;

        return ENodeState.RUNNING;
    }

    private void ResetRandomDirection()
    {
        Node root = GetRoot(this);
        object tmp = root.GetData("randomDirection");
        if (tmp != null && _thisRandomWalkTree)
        {
            DeleteData("randomDirection");
            _thisRandomWalkTree.CurrentWalkTime = 0f;
        }
    }
}
