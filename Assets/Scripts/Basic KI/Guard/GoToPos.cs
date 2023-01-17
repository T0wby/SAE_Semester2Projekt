using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class GoToPos : Node
{
    #region Fields
    private Transform _thisTransform;
    private Transform _targetTransform;
    private NavMeshAgent _agent;
    private BasicKISettings _settings;
    private Animator _animator;
    #endregion

    #region Constructor
    public GoToPos()
    {

    }

    public GoToPos(Transform transform, Transform targetTransform, NavMeshAgent agent, BasicKISettings settings, Animator animator)
    {
        _thisTransform = transform;
        _targetTransform = targetTransform;
        _agent = agent;
        _settings = settings;
        _animator = animator;
    } 
    #endregion

    public override ENodeState CalculateState()
    {
        if (_agent.speed != _settings.RunSpeed)
            _agent.speed = _settings.RunSpeed;

        if (Vector3.Distance(_thisTransform.position, _targetTransform.position) < 0.01f)
        {
            _thisTransform.position = _targetTransform.position;
            SetAnimationBool(_animator, "IsWalking", false);
        }
        else
        {
            _agent.destination = _targetTransform.position;
            SetAnimationBool(_animator, "IsWalking", true);
        }

        return state = ENodeState.RUNNING;
    }
}
