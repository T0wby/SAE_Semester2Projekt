using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class LF_CheckIfAtHidePoint : Node
{
    #region Fields

    private Transform _thisTransform;
    private Animator _animator;
    private object _hideDestination;
    private float _distance;

    #endregion

    #region Constructor

    public LF_CheckIfAtHidePoint()
    {

    }

    /// <summary>
    /// Checks if the entity is near the hideDestination yet
    /// </summary>
    /// <param name="transform">Own transform</param>
    /// <param name="animator">Own animator</param>
    public LF_CheckIfAtHidePoint(Transform transform, Animator animator)
    {
        _thisTransform = transform;
        _animator = animator;
    }

    #endregion

    #region Loop
    public override ENodeState CalculateState()
    {
        _hideDestination = GetData("hideDestination");

        if (_hideDestination == null)
            return ENodeState.SUCCESS;


        _distance = ((Vector3)_hideDestination - _thisTransform.position).sqrMagnitude;
        if ((_distance * _distance) > 3f)
        {
            // Not near our hidepoint yet
            return ENodeState.FAILURE;
        }
        // Clearing the destination data since we arrived at it
        GetRoot(this).DeleteData("hideDestination");

        return ENodeState.SUCCESS;
    }
    #endregion
}
