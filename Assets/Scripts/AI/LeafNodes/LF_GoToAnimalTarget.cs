using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LF_GoToAnimalTarget : Node
{
    #region Fields
    private NavMeshAgent _agent;
    private AnimalAISettings _settings;
    private Transform _thisTransform;
    private Transform _targetTransform;
    private string _dataSet; 
    #endregion

    #region Constructors
    public LF_GoToAnimalTarget()
    {
        
    }

    /// <summary>
    /// Move the agent to a target
    /// </summary>
    /// <param name="transform">Own transform</param>
    /// <param name="agent">Own NavMeshAgent</param>
    /// <param name="settings">Own AnimalAISettings</param>
    /// <param name="dataSet">String for the target transform</param>
    public LF_GoToAnimalTarget(Transform transform, NavMeshAgent agent, AnimalAISettings settings, string dataSet)
    {
        _thisTransform = transform;
        _agent = agent;
        _settings = settings;
        _dataSet = dataSet;
    }
    #endregion

    #region Method
    public override ENodeState CalculateState()
    {
        _targetTransform = (Transform)GetData(_dataSet);
        if (_agent.speed != _settings.RunSpeed)
            _agent.speed = _settings.RunSpeed;

        if (Vector3.SqrMagnitude(_thisTransform.position - _targetTransform.position) > (_settings.InteractRange * _settings.InteractRange))
        {
            if (_agent.destination != _targetTransform.position)
                _agent.destination = _targetTransform.position;
        }

        return ENodeState.RUNNING;
    }
    #endregion
}
