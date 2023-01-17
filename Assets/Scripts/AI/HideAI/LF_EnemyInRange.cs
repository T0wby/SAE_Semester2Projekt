using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class LF_EnemyInRange : Node
{
    private float _senseRange;
    private Transform _thisTransform;
    private Transform _targetTransform;

    public LF_EnemyInRange()
    {
        _senseRange = 0f;
        _thisTransform = null;
    }

    public LF_EnemyInRange(float senseRange, Transform thisTransform)
    {
        _senseRange = senseRange;
        _thisTransform = thisTransform;
    }

    public override ENodeState CalculateState()
    {
        _targetTransform = (Transform)GetData("target");

        if (_targetTransform is null)
            return ENodeState.FAILURE;

        return CheckIfInRange(_thisTransform, _targetTransform, _senseRange) ? ENodeState.SUCCESS : ENodeState.FAILURE;
    }

    private bool CheckIfInRange(Transform thisTransform, Transform targetTransform, float range)
    {
       return Vector3.SqrMagnitude(targetTransform.position - thisTransform.position) > range * range ?  false: true;
    }
}
