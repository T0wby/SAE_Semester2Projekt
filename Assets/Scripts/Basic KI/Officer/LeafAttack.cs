using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class LeafAttack : Node
{
    private Transform _lastTarget;
    private Enemy enemy;

    private float _attackTime = 1f;
    private float _attackCounter = 0f;

    public LeafAttack(Transform transform)
    {

    }

    public override ENodeState CalculateState()
    {
        Transform target = (Transform)GetData("target");

        if (target != _lastTarget)
        {
            enemy = target.GetComponent<Enemy>();
            _lastTarget = target;
        }

        _attackCounter += Time.deltaTime;
        if (_attackCounter >= _attackTime)
        {
            Debug.Log("Attack!");
            _attackCounter = 0f;
        }

        return state = ENodeState.RUNNING;
    }
}
