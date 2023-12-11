using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class SnowBT : BehaviorTree.MyTree
{
    public Transform[] waypoints;
    public OfficerSettings settings;
    private Animator _animator;

    [SerializeField] private float _waypointRadius = 5f;

    protected override Node SetupTree()
    {
        _enemyLayerMask = 1 << 9;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new LF_CheckForEnemyInFOV(transform, settings.FovRange, settings.FovAngle, _enemyLayerMask),
                new LF_GoToTarget(transform, _agent, settings, _animator)
            }),
            new LF_PatrolWait(transform, waypoints, _agent, _waypointRadius, settings.WalkSpeed, _animator),
        });

        return root;
    }
}
