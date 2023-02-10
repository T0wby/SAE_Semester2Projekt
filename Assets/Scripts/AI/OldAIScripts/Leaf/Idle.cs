using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class Idle : Node
{
    private NavMeshAgent _agent;
    private Animator _animator;

    public Idle(NavMeshAgent agent, Animator animator)
	{
		_agent = agent;
        _animator = animator;
	}

    public override ENodeState CalculateState()
	{
        //_agent.isStopped = true;
        //SetAnimationBool(_animator, "IsWalking", false);
        //SetAnimationBool(_animator, "IsAttacking", false);
        return ENodeState.RUNNING;
    }
}
