using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToDataPoint : Node
{
    #region Fields

    private Transform _thisTransform;
    private NavMeshAgent _agent;
    private object _destination;
    private string _dataKey;

    #endregion

    #region Constructor

    public GoToDataPoint(Transform transform, NavMeshAgent agent, string dataKey)
    {
        _thisTransform = transform;
        _agent = agent;
        _dataKey = dataKey;
    }

    #endregion

    #region Loop
    public override ENodeState CalculateState()
    {
        _destination = GetData(_dataKey);

        if (_destination != null && Vector3.Distance(_thisTransform.position, (Vector3)_destination) < 2f)
        {
            return ENodeState.FAILURE;
        }

        return ENodeState.SUCCESS;
    }
    #endregion
}
