using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class FlockingEnemy : Node
{
    private NavMeshAgent _agent;
    private BoidMovement _boidMovement;
    private BoidSettings _settings;

    public FlockingEnemy(NavMeshAgent agent, BoidMovement boidMovement, BoidSettings settings)
    {
        _agent = agent;
        _boidMovement = boidMovement;
        _settings = settings;
    }

    public override ENodeState CalculateState()
    {
        if (_agent.speed != _settings.WalkSpeed)
            _agent.speed = _settings.WalkSpeed;

        _agent.Move(_boidMovement.CurrentVelocity);
        return state = ENodeState.RUNNING;
    }
}

