using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

[RequireComponent(typeof(NavMeshAgent))]
public class OfficerBT : BehaviorTree.Tree
{
    public Transform[] waypoints;

    public static float speed = 2f;
    public static float fovRange = 6f;
    public static float attackRange = 2f;

    private NavMeshAgent _agent;
    public NavMeshAgent Agent { get { return _agent; } }

    protected override Node SetupTree()
    {
        _agent = GetComponent<NavMeshAgent>();

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckForEnemyInAttackRange(transform),
                new LeafAttack(transform),
            }),
            new Sequence(new List<Node>
            {
                new CheckForEnemyInFOV(transform),
                new GoToTarget(transform),
            }),
            new Patrol(this.transform, waypoints),
        });

        return root;
    }
}
