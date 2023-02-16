using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LF_TargetInRange : Node
{
    private AnimalAISettings _settings;
    private Transform _thisTransform;
    private Transform _targetTransform;
    private string _dataSet;

    #region Constructors
    public LF_TargetInRange()
    {

    }

    public LF_TargetInRange(Transform transform, AnimalAISettings settings, string dataSet)
    {
        _thisTransform = transform;
        _settings = settings;
        _dataSet = dataSet;
    }
    #endregion

    #region Method
    public override ENodeState CalculateState()
    {
        _targetTransform = (Transform)GetData(_dataSet);
        if (Vector3.Distance(_thisTransform.position, _targetTransform.position) < _settings.InteractRange)
        {
            return ENodeState.SUCCESS;
        }

        return ENodeState.FAILURE;
    }
    #endregion
}
