using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class WalkAround : Node
{
    #region Fields
    private Transform _thisTransform;
    private NavMeshAgent _agent;
    private float _speed;
    private HideAI _hideAI;
    private Animator _animator;
    #endregion

    #region Constructor
    public WalkAround()
    {

    }

    public WalkAround(Transform transform, NavMeshAgent agent, float speed, Animator animator, HideAI hideAI)
    {
        _thisTransform = transform;
        _agent = agent;
        _speed = speed;
        _animator = animator;
        _hideAI = hideAI;
    }
    #endregion

    #region Methods
    public override ENodeState CalculateState()
    {
        object tmp = GetData("randomDirection");
        if (tmp is null)
            return state = ENodeState.FAILURE;

        Vector2 randomDirection = (Vector2)tmp;

        if (_agent.speed != _speed)
            _agent.speed = _speed;

        SetAnimationBool(_animator, "IsWalking", true);

        Vector3 movementDirection = new Vector3(randomDirection.x, _thisTransform.position.y, randomDirection.y);

        _agent.destination = _thisTransform.position + movementDirection;
        _hideAI.CurrentWalkTime += Time.deltaTime;

        if (WalkTimerDone())
            return ENodeState.FAILURE;

        return ENodeState.RUNNING;
    }

    private bool WalkTimerDone()
    {
        if (_hideAI.CurrentWalkTime >= _hideAI.MaxWalkTime)
        {
            return true;
        }

        return false;
    } 
    #endregion
}