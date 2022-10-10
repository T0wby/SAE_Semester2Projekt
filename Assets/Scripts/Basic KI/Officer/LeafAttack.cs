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

    private float _attackTime;
    private float _attackCounter = 0f;

    public LeafAttack(Transform transform, float attackSpeed)
    {
        _thisTransform = transform;
        _attackTime = attackSpeed;
        _thisAttack = transform.GetComponent<IAttack>();
    }

    public override ENodeState CalculateState()
    {
        _currentTarget = (Transform)GetData("target");

        if (_currentTarget != _lastTarget)
        {
            _enemy = _currentTarget.GetComponent<IMortal>();
            _lastTarget = _currentTarget;
        }

        _attackCounter += Time.deltaTime;
        if (_attackCounter >= _attackTime)
        {
            Debug.Log(_thisTransform.gameObject.name + " Attacks!");
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
}
