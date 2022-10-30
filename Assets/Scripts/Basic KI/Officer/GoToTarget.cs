using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class GoToTarget : Node
{
    private Transform _thisTransform;
    private NavMeshAgent _agent;
    private BasicKISettings _settings;
    private Animator _animator;

    public GoToTarget(Transform transform, NavMeshAgent agent, BasicKISettings settings, Animator animator)
    {
        _thisTransform = transform;
        _agent = agent;
        _settings = settings;
        _animator = animator;
    }

    public override ENodeState CalculateState()
    {
        Transform targetTransform = (Transform)GetData("target");

        SetAnimationState(_animator, "IsWalking", true);

        if (_agent.speed != _settings.RunSpeed)
            _agent.speed = _settings.RunSpeed;

        if (Vector3.Distance(_thisTransform.position, targetTransform.position) > 0.1f)
        {
            if (_agent.destination != targetTransform.position)
                _agent.destination = targetTransform.position;
        }
        else
            SetAnimationState(_animator, "IsWalking", false);

        return state = ENodeState.RUNNING;
    }

    /// <summary>
    /// Changes a bool value of an animator
    /// </summary>
    /// <param name="animator">Used animator</param>
    /// <param name="paramName">Exact name of the bool</param>
    /// <param name="state">bool value it should change to</param>
    private void SetAnimationState(Animator animator, string paramName, bool state)
    {
        if (animator.GetBool(paramName) != state)
        {
            animator.SetBool(paramName, state);
        }
    }
}
