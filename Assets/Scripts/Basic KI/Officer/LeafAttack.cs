using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class LeafAttack : Node
{
    private Transform _currentTarget;
    private Transform _lastTarget;
    private Transform _thisTransform;
    private IMortal _enemy;
    private IAttack _thisAttack;
    private Animator _animator;

    private float _attackTime;
    private float _attackCounter = 0f;

    public LeafAttack(Transform transform, float attackSpeed, Animator animator)
    {
        _thisTransform = transform;
        _attackTime = attackSpeed;
        _thisAttack = transform.GetComponent<IAttack>();
        _animator = animator;
    }

    public override ENodeState CalculateState()
    {
        _currentTarget = (Transform)GetData("target");

        SetAnimationState(_animator, "IsWalking", false);
        

        if (_currentTarget != _lastTarget)
        {
            _enemy = _currentTarget.GetComponent<IMortal>();
            _lastTarget = _currentTarget;
        }

        _attackCounter += Time.deltaTime;
        if (_attackCounter >= _attackTime)
        {
            SetAnimationState(_animator, "IsAttacking", true);
            _thisAttack.Attack(_enemy);
            CheckEnemyHealth(_enemy);
            _attackCounter = 0f;
        }

        return state = ENodeState.RUNNING;
    }

    private void CheckEnemyHealth(IMortal enemy)
    {
        if (enemy.Health <= 0)
        {
            RemoveDeadEnemy(enemy);
        }
    }

    private void RemoveDeadEnemy(IMortal enemy)
    {
        DeleteData("target");
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
