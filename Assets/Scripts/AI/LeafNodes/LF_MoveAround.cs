using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LF_MoveAround : Node
{
    #region Fields
    private Transform _thisTransform;
    private NavMeshAgent _agent;
    private float _searchRange;
    private AAnimal _animal;
    private Vector2 _destination;
    private float _distance; 
    #endregion

    #region Constructors
    public LF_MoveAround()
    {
        
    }

    /// <summary>
    /// Moves the agent to a random point inside a circle
    /// </summary>
    /// <param name="thisTransform">Own transform</param>
    /// <param name="agent"Own NavMeshAgent></param>
    /// <param name="searchRange">radius of the circle</param>
    /// <param name="animal">Own AAnimal reference</param>
    public LF_MoveAround(Transform thisTransform, NavMeshAgent agent, float searchRange, AAnimal animal)
    {
        _thisTransform = thisTransform;
        _agent = agent;
        _searchRange = searchRange;
        _animal = animal;
    }
    #endregion

    #region Method
    public override ENodeState CalculateState()
    {
        SetRandomDestination(_agent, _thisTransform, _searchRange, _animal.RandomMove);
        return ENodeState.RUNNING;
    }

    private void SetRandomDestination(NavMeshAgent agent, Transform thisTransform, float range, bool allowedToMove)
    {
        _distance = (thisTransform.position - agent.destination).sqrMagnitude;
        if ((_distance * _distance) < 2f && allowedToMove)
        {
            _destination = Random.insideUnitCircle * range;
            agent.SetDestination(new Vector3(thisTransform.position.x + _destination.x, thisTransform.position.y, thisTransform.position.z + _destination.y));
        }
    }

    #endregion
}
