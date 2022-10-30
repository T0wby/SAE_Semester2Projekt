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
    private VillagerSettings _settings;
    private Animator _animator;

    #region Constructors
    public Flee(Transform transform, NavMeshAgent agent, Animator animator)
    {
        _thisTransform = transform;
        _agent = agent;
        _animator = animator;
    }
    
    public Flee(Transform transform, NavMeshAgent agent, RandomWalkTree randomWalkTree, VillagerSettings vilSettings, Animator animator)
    {
        _thisTransform = transform;
        _agent = agent;
        _thisRandomWalkTree = randomWalkTree;
        _settings = vilSettings;
        _animator = animator;
    }
    #endregion

    public override ENodeState CalculateState()
    {
        if (_agent.speed != _settings.RunSpeed)
            _agent.speed = _settings.RunSpeed;

        Transform targetTransform = (Transform)GetData("target");
        SetAnimationState(_animator, "IsWalking", true);
        if (Vector3.Distance(_thisTransform.position, targetTransform.position) > _settings.SafeRange)
        {
            return ENodeState.FAILURE;
        }
            

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
