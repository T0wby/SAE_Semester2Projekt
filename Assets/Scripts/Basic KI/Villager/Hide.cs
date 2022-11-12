using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;
using UnityEditor;
using Unity.VisualScripting;

public class Hide : Node
{
    private Transform _thisTransform;
    private NavMeshAgent _agent;
    private VillagerSettings _settings;
    private List<Collider> _colliders;
    private Transform _targetTransform;
    private TrackHideObject _trackHideObject;
    

    public Hide(Transform transform, NavMeshAgent agent, VillagerSettings settings, TrackHideObject trackHideObject)
    {
        _thisTransform = transform;
        _agent = agent;
        _settings = settings;
        _trackHideObject = trackHideObject;
    }

    public override ENodeState CalculateState()
    {
        object target = GetData("target");

        if(target != null)
            _targetTransform = (Transform)target;

        Debug.Log("Hide!!");

        if (Hiding(_targetTransform))
            return ENodeState.SUCCESS;

        return ENodeState.FAILURE;
    }

    private bool Hiding(Transform target)
    {
        while (true)
        {

            _colliders = _trackHideObject.Colliders;

            for (int i = 0; i < _colliders.Count; i++)
            {
                if (NavMesh.SamplePosition(_colliders[i].transform.position, out NavMeshHit hit, 100f, 1))
                {
                    if (!NavMesh.FindClosestEdge(hit.position, out hit, _agent.areaMask))
                    {
                        Debug.LogError("No closest Edge found!");
                    }

                    if (Vector3.Dot(hit.normal, (_targetTransform.position - hit.position).normalized) < _settings.HideSensitivity)
                    {
                        _agent.destination = hit.position;
                        return true;
                    }
                    else // if hit position is facing the player
                    {
                        if (NavMesh.SamplePosition(_colliders[i].transform.position - (_targetTransform.position - hit.position).normalized * 5, out NavMeshHit hittwo, 2f, _agent.areaMask))
                        {
                            if (!NavMesh.FindClosestEdge(hittwo.position, out hittwo, _agent.areaMask))
                            {
                                Debug.LogError("No closest Edge found the second!");
                            }

                            if (Vector3.Dot(hittwo.normal, (_targetTransform.position - hittwo.position).normalized) < _settings.HideSensitivity)
                            {
                                _agent.destination = hittwo.position;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }

}
