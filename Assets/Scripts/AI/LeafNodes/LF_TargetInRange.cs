using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LF_TargetInRange : Node
{
    #region Fields
    private Transform _thisTransform;
    private Transform _targetTransform;
    private string _dataSet;
    private float _range;
    #endregion

    #region Constructors
    public LF_TargetInRange()
    {

    }

    /// <summary>
    /// Checks if the target is in a certain range
    /// </summary>
    /// <param name="transform">Own transform</param>
    /// <param name="range">Range to be checked</param>
    /// <param name="dataSet">String used to store the target data</param>
    public LF_TargetInRange(Transform transform, float range, string dataSet)
    {
        _thisTransform = transform;
        _range = range;
        _dataSet = dataSet;
    }
    #endregion

    #region Method
    public override ENodeState CalculateState()
    {
        _targetTransform = (Transform)GetData(_dataSet);

        if (_targetTransform is null)
            return ENodeState.FAILURE;

        return CheckIfInRange(_thisTransform, _targetTransform, _range);
    }

    private ENodeState CheckIfInRange(Transform thisTransform, Transform targetTransform, float range)
    {
        return Vector3.SqrMagnitude(targetTransform.position - thisTransform.position) < range * range ? ENodeState.SUCCESS : ENodeState.FAILURE;
    }
    #endregion
}
