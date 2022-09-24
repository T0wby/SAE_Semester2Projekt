using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckForEnemyInFOV : Node
{
    private static int _enemyLayerMask = 1 << 9;
    private Transform _thisTransform;
    private Collider[] _colliders;

    public CheckForEnemyInFOV(Transform transform)
    {
        _thisTransform = transform; 
    }

    public override ENodeState CalculateState()
    {
        object target = GetData("target");
        if (target is null)
        {
            _colliders = Physics.OverlapSphere(_thisTransform.position, OfficerBT.fovRange, _enemyLayerMask);

            if (_colliders.Length > 0)
            {
                //Saving the Target in Root so that other Nodes can access it
                Parent.Parent.SetData("target", _colliders[0].transform);
                return state = ENodeState.SUCCESS;
            }
            else
            {
                return state = ENodeState.FAILURE;
            }
        }

        return state = ENodeState.SUCCESS;
    }
}
