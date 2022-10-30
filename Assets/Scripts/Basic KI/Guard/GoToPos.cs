using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class GoToPos : Node
{
    private Transform _thisTransform;
    private Transform _targetTransform;
    private NavMeshAgent _agent;
    private BasicKISettings _settings;
    private Animator _animator;

    public GoToPos(Transform transform, Transform targetTransform, NavMeshAgent agent, BasicKISettings settings, Animator animator)
    {
        _thisTransform = transform;
        _targetTransform = targetTransform;
        _agent = agent;
        _settings = settings;
        _animator = animator;
    }

    public override ENodeState CalculateState()
    {
        if (_agent.speed != _settings.RunSpeed)
            _agent.speed = _settings.RunSpeed;

        if (Vector3.Distance(_thisTransform.position, _targetTransform.position) < 0.01f)
        {
            _thisTransform.position = _targetTransform.position;
            SetAnimationState(_animator, "IsWalking", false);
        }
        else
        {
            _agent.destination = _targetTransform.position;
            SetAnimationState(_animator, "IsWalking", true);
        }

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
