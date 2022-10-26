using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class Hide : Node
{
    private Transform _thisTransform;
    private NavMeshAgent _agent;
    private VillagerSettings _settings;
    private LayerMask _hideableLayers;
    private Collider[] _colliders = new Collider[20];
    private Transform _targetTransform;


    public Hide(Transform transform, NavMeshAgent agent, VillagerSettings settings, LayerMask hideableLayers)
    {
        _thisTransform = transform;
        _agent = agent;
        _settings = settings;
        _hideableLayers = hideableLayers;
    }

    public override ENodeState CalculateState()
    {
        object target = GetData("target");

        if(target != null)
            _targetTransform = (Transform)target;

        return ENodeState.RUNNING;
    }

    private bool Hiding(Transform target)
    {
        while (true)
        {
            _colliders = Physics.OverlapSphere(_thisTransform.position, _settings.FovRange, _hideableLayers);

            for (int i = 0; i < _colliders.Length; i++)
            {
                if (NavMesh.SamplePosition(_colliders[i].transform.position, out NavMeshHit hit, 2f, _agent.areaMask))
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
