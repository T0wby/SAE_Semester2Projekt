using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckForEnemyInFOV : Node
{
    private static int _enemyLayerMask = 1 << 9;
    private Transform _thisTransform;
    private Collider[] _colliders;
    private float _range;

    public CheckForEnemyInFOV(Transform transform, float range)
    {
        _thisTransform = transform;
        _range = range;
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
        _colliders = Physics.OverlapSphere(_thisTransform.position, _range, _enemyLayerMask);

        if (_colliders.Length > 0)
        {
            //Saving the Target in Root so that other Nodes can access it
            Node tmp = GetRoot(this);
            tmp.SetData("target", ClosestEnemy(_colliders));
            return ENodeState.SUCCESS;
        }
        else
        {
            return ENodeState.FAILURE;
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
