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
        
        if (target is not null)
            DeleteData("target");

        return CheckforEnemy();

    }

    private ENodeState CheckforEnemy()
    {
        _colliders = Physics.OverlapSphere(_thisTransform.position, OfficerBT.fovRange, _enemyLayerMask);

        if (_colliders.Length > 0)
        {
            //Saving the Target in Root so that other Nodes can access it
            Parent.Parent.SetData("target", ClosestEnemy(_colliders));
            return state = ENodeState.SUCCESS;
        }
        else
        {
            return state = ENodeState.FAILURE;
        }
    }

    private Transform ClosestEnemy(Collider[] enemyColliders)
    {
        float lowest = Vector3.Distance(enemyColliders[0].transform.position, _thisTransform.position);
        Collider closest = enemyColliders[0];

        for (int i = 0; i < enemyColliders.Length; i++)
        {
            float next = Vector3.Distance(enemyColliders[i].transform.position, _thisTransform.position);

            if (lowest > next)
            {
                closest = enemyColliders[i];
                lowest = next;
            }
        }
        return closest.transform;
    }
}
