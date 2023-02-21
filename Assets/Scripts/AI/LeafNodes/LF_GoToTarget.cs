using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class LF_GoToTarget : Node
{
    #region Fields
    private Transform _thisTransform;
    private NavMeshAgent _agent;
    private BasicKISettings _settings;
    private Animator _animator;
    #endregion

    #region Constructors
    public LF_GoToTarget()
    {

    }

    /// <summary>
    /// Move the agent to a target
    /// </summary>
    /// <param name="transform">Own transform</param>
    /// <param name="agent">Own NavMeshAgent</param>
    /// <param name="settings">KI settings</param>
    /// <param name="animator">Own animator</param>
    public LF_GoToTarget(Transform transform, NavMeshAgent agent, BasicKISettings settings, Animator animator)
    {
        _thisTransform = transform;
        _agent = agent;
        _settings = settings;
        _animator = animator;
    } 
    #endregion

    public override ENodeState CalculateState()
    {
        Transform targetTransform = (Transform)GetData("target");


        if (_agent.speed != _settings.RunSpeed)
            _agent.speed = _settings.RunSpeed;

        if (Vector3.SqrMagnitude(_thisTransform.position - targetTransform.position) > (0.7f * 0.7f))
        {
            if (_agent.destination != targetTransform.position)
                _agent.destination = targetTransform.position;
        }

        return state = ENodeState.RUNNING;
    }
}
