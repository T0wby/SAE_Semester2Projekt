using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LF_MoveAround : Node
{
    private Transform _thisTransform;
    private NavMeshAgent _agent;
    private AnimalAISettings _settings;

    #region Constructors
    public LF_MoveAround()
    {
        
    }

    public LF_MoveAround(Transform thisTransform, NavMeshAgent agent, AnimalAISettings settings)
    {
        _thisTransform = thisTransform;
        _agent = agent;
        _settings = settings;
    }
    #endregion

    #region Method
    public override ENodeState CalculateState()
    {
        SetRandomDestination(_agent, _thisTransform, _settings.SearchRange);
        return ENodeState.RUNNING;
    }

    private void SetRandomDestination(NavMeshAgent agent, Transform thisTransform, float range)
    {
        if (agent.destination != thisTransform.position)
        {
            agent.SetDestination(new Vector3(thisTransform.position.x + Random.Range(-range, range), thisTransform.position.y, thisTransform.position.z + Random.Range(-range, range)));
        }
    }

    #endregion
}
