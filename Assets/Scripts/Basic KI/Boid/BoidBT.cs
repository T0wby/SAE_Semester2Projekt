using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class BoidBT : BehaviorTree.Tree
{
    public Transform[] waypoints;
    public BoidSettings settings;
    public BoidMovement boidMovement;

    public static float speed = 2f;
    public static float fovRange = 6f;
    public static float attackRange = 2f;
    public static float viewAngle = 200f;

    private NavMeshAgent _agent;
    private int _enemyLayerMask = 1 << 10;

    protected override Node SetupTree()
    {
        _agent = GetComponent<NavMeshAgent>();

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckForEnemyInAttackRange(transform, attackRange),
                new LeafAttack(transform),
            }),
            new Sequence(new List<Node>
            {
                new CheckForEnemyInFOV(transform, fovRange, viewAngle, _enemyLayerMask),
                new GoToTarget(transform, _agent),
            }),
            new Flocking(transform, waypoints, _agent, boidMovement),
        });

        return root;
    }
}
