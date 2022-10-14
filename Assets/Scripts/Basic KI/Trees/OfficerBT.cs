using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

[RequireComponent(typeof(NavMeshAgent))]
public class OfficerBT : BehaviorTree.Tree
{
    public Transform[] waypoints;
    public OfficerSettings settings;
    public static float targetRadius = 5f;

    private NavMeshAgent _agent;
    private int _enemyLayerMask = 1 << 9;

    protected override Node SetupTree()
    {
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
            new PatrolWait(transform, waypoints, _agent, targetRadius, settings.WalkSpeed),
        });

        return root;
    }
}
