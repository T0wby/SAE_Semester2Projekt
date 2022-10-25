using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

[RequireComponent(typeof(NavMeshAgent))]
public class OfficerBT : BehaviorTree.MyTree
{
    public Transform[] waypoints;
    public OfficerSettings settings;

    [SerializeField] private float _waypointRadius = 5f;

    protected override Node SetupTree()
    {
        _enemyLayerMask = 1 << 9;
        _agent = GetComponent<NavMeshAgent>();

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckForEnemyInAttackRange(transform, settings.InteractRange),
                new LeafAttack(transform, settings.AtkSpeed),
            }),
            new Sequence(new List<Node>
            {
                new CheckForEnemyInFOV(transform, settings.FovRange, settings.FovAngle, _enemyLayerMask),
                new GoToTarget(transform, _agent, settings)
            }),
            new PatrolWait(transform, waypoints, _agent, _waypointRadius, settings.WalkSpeed),
        });

        return root;
    }
}
