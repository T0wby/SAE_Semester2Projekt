using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckForEnemyInFOV : Node
{
    private int _enemyLayerMask;
    private Transform _thisTransform;
    private Collider[] _colliders = new Collider[30];
    private List<Collider> _inViewColliders = new List<Collider>();
    private float _range;
    private float _viewAngle;

    public CheckForEnemyInFOV(Transform transform, float range, float viewAngle, int enemyLayerMask)
    {
        _thisTransform = transform;
        _range = range;
        _viewAngle = viewAngle;
        _enemyLayerMask = enemyLayerMask;
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
        if (_colliders.Length > 0)
        {
            for (int i = 0; i < _colliders.Length; i++)
            {
                _colliders[i] = null;
            }
        }


        _colliders = Physics.OverlapSphere(_thisTransform.position, _range, _enemyLayerMask);

        if (_colliders.Length > 0)
        {
            foreach (Collider collider in _colliders)
            {
                if (Vector3.Angle(_thisTransform.forward, collider.transform.position - _thisTransform.position) < _viewAngle * 0.5f)
                {
                    if (!_inViewColliders.Contains(collider))
                        _inViewColliders.Add(collider);
                }
                else
                {
                    if (_inViewColliders.Contains(collider))
                        _inViewColliders.Remove(collider);
                }

            }

            if (_inViewColliders.Count > 0)
            {
                //Saving the Target in Root so that other Nodes can access it
                GetRoot(this).SetData("target", ClosestEnemy(_inViewColliders));
                return ENodeState.SUCCESS;
            }

            return ENodeState.FAILURE;

        }
        else
        {
            return ENodeState.FAILURE;
        }
    }

    private Transform ClosestEnemy(List<Collider> enemyColliders)
    {
        //Might consider sqrMagnitude
        float lowest = Vector3.Distance(enemyColliders[0].transform.position, _thisTransform.position);
        Collider closest = enemyColliders[0];

        for (int i = 0; i < enemyColliders.Count; i++)
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
