using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;
using UnityEditor;
using Unity.VisualScripting;

public class LF_GetHidePosition : Node
{
    #region Fields

    private NavMeshAgent _agent;
    private HideAISettings _settings;
    private List<Collider> _colliders;
    private Transform _thisTransform;
    private Transform _targetTransform;
    private TrackHideObject _trackHideObject;
    private object _target;

    #endregion

    #region Constructors
    public LF_GetHidePosition()
    {

    }

    /// <summary>
    /// Searches for a hide point
    /// </summary>
    /// <param name="agent">Agent that is hiding</param>
    /// <param name="settings">Settings of the hiding agent</param>
    /// <param name="trackHideObject">Reference to the tracked objects that the agents can hide behind</param>
    /// <param name="thisTransform">Own transform</param>
    public LF_GetHidePosition(NavMeshAgent agent, HideAISettings settings, TrackHideObject trackHideObject, Transform thisTransform)
    {
        _agent = agent;
        _settings = settings;
        _trackHideObject = trackHideObject;
        _thisTransform = thisTransform;
    } 
    #endregion

    public override ENodeState CalculateState()
    {
        _target = GetData("target");

        if (_target is not null)
            _targetTransform = (Transform)_target;

        if (Hiding(_targetTransform))
            return ENodeState.SUCCESS;

        return ENodeState.FAILURE;
    }

    /// <summary>
    /// Calculates where to hide from the target
    /// </summary>
    /// <param name="target">Transform of the target</param>
    /// <returns>If a position was found or not</returns>
    private bool Hiding(Transform target)
    {
        _colliders = _trackHideObject.Colliders;

        for (int i = 0; i < _colliders.Count; i++)
        {
            if (NavMesh.SamplePosition(_colliders[i].transform.position, out NavMeshHit hit, 100f, 1))
            {
                Node root = GetRoot(this);

                if (!NavMesh.FindClosestEdge(hit.position, out hit, _agent.areaMask))
                {
                    Debug.LogError("No closest Edge found!");
                }

                // Check if the hit position is on the side of the player or not
                if (Vector3.Dot(hit.normal, (target.position - hit.position).normalized) < _settings.HideSensitivity)
                {
                    root.SetData("hideDestination", hit.position);
                    return true;
                }
                else // if hit position is facing the player
                {
                    if (NavMesh.SamplePosition(_colliders[i].transform.position - (target.position - hit.position).normalized * 5, out NavMeshHit hittwo, 2f, _agent.areaMask))
                    {
                        if (!NavMesh.FindClosestEdge(hittwo.position, out hittwo, _agent.areaMask))
                        {
                            Debug.LogError("No closest Edge found the second!");
                        }

                        if (Vector3.Dot(hittwo.normal, (target.position - hittwo.position).normalized) < _settings.HideSensitivity)
                        {
                            root.SetData("hideDestination", hittwo.position);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}
