using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class SetHideDestination : Node
{
    #region Fields

    private NavMeshAgent _agent;
    private VillagerSettings _settings;
    private Animator _animator;
    private object _hideDestination;

    #endregion

    #region Constructor
    public SetHideDestination(NavMeshAgent agent, VillagerSettings settings, Animator animator)
	{
        _agent = agent;
        _settings = settings;
        _animator = animator;
	}

    #endregion

    #region Loop
    public override ENodeState CalculateState()
    {
        _hideDestination = GetData("hideDestination");

        if (_hideDestination != null)
        {
            SetAgentDestination(_agent, (Vector3)_hideDestination);
            SetAgentSpeed();
            return ENodeState.SUCCESS;
        }

        return ENodeState.FAILURE;
    }
    #endregion

    #region Methods

    /// <summary>
    /// Sets a new agent destination
    /// </summary>
    /// <param name="navMeshAgent">Used agent</param>
    /// <param name="position">Position to set</param>
    private void SetAgentDestination(NavMeshAgent navMeshAgent, Vector3 position)
    {
        if (navMeshAgent.destination != position)
        {
            navMeshAgent.destination = position;
            SetAnimationBool(_animator, "IsWalking", true);
        }
    }

    /// <summary>
    /// Sets the speed of the Agent
    /// </summary>
    private void SetAgentSpeed()
    {
        if (_agent.speed != _settings.RunSpeed)
            _agent.speed = _settings.RunSpeed;
    }

    #endregion
}
