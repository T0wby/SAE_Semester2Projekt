using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class GoToGuardPos : Node
{
    private Transform _thisTransform;
    private Transform _guardpoint;

    public GoToGuardPos(Transform transform, Transform guardpoint)
    {
        _thisTransform = transform;
        _guardpoint = guardpoint;
    }

    public override ENodeState CalculateState()
    {
        if (Vector3.Distance(_thisTransform.position, _guardpoint.position) < 0.01f)
        {
            _thisTransform.position = _guardpoint.position;
        }
        else
        {
            _thisTransform.position = Vector3.MoveTowards(_thisTransform.position, _guardpoint.position, GuardBT.speed * Time.deltaTime);
            _thisTransform.LookAt(_guardpoint.position);
            return state = ENodeState.RUNNING;
        }

        return state = ENodeState.FAILURE;
    }
}
