using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class LF_CheckForEnemyInFOV : Node
{
    #region Fields
    private int _enemyLayerMask;
    private Transform _thisTransform;
    private Collider[] _colliders = new Collider[30];
    private List<Collider> _inViewColliders = new List<Collider>();
    private float _range;
    private float _viewAngle;
    private float _sqrDistLowest;
    private float _sqrDistCurrent;
    #endregion

    #region Constructors
    public LF_CheckForEnemyInFOV()
    {

    }

    /// <summary>
    /// Check if an enemy is in your field of view
    /// </summary>
    /// <param name="transform">Own transform</param>
    /// <param name="range">View range</param>
    /// <param name="viewAngle">View Angle</param>
    /// <param name="enemyLayerMask">Layermask number of the enemy type</param>
    public LF_CheckForEnemyInFOV(Transform transform, float range, float viewAngle, int enemyLayerMask)
    {
        _thisTransform = transform;
        _range = range;
        _viewAngle = viewAngle;
        _enemyLayerMask = enemyLayerMask;
    }
    #endregion

    #region Methods
    public override ENodeState CalculateState()
    {
        object target = GetData("target");


        if (target is not null)
            DeleteData("target");

        return CheckforEnemy();

    }

    /// <summary>
    /// Adds all enemys, that are in the viewAngle to a list and sets the target to the closest entity
    /// </summary>
    /// <returns>If we found a target or not</returns>
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

    /// <summary>
    /// Returns the transform of the closest Enemy
    /// </summary>
    /// <param name="enemyColliders">List to check in</param>
    /// <returns>closest Enemy transform</returns>
    private Transform ClosestEnemy(List<Collider> enemyColliders)
    {
        _sqrDistLowest = (enemyColliders[0].transform.position - _thisTransform.position).sqrMagnitude;
        Collider closest = enemyColliders[0];

        for (int i = 0; i < enemyColliders.Count; i++)
        {
            _sqrDistCurrent = (enemyColliders[i].transform.position - _thisTransform.position).sqrMagnitude;

            if (_sqrDistLowest > _sqrDistCurrent)
            {
                closest = enemyColliders[i];
                _sqrDistLowest = _sqrDistCurrent;
            }
        }
        return closest.transform;
    } 
    #endregion
}
