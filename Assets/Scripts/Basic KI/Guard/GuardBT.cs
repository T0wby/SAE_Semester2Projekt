using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
public class GuardBT : BehaviorTree.Tree
{
    public Transform guardpoint;

    public static float speed = 2f;
    public static float fovRange = 6f;
    public static float attackRange = 2f;
    private NavMeshAgent _agent;
    private int _enemyLayerMask = 1 << 9;

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
                new CheckForEnemyInFOV(transform, fovRange, _enemyLayerMask),
                new GoToTarget(transform,_agent),
            }),
            new Sequence(new List<Node>
            {
                new CheckIfNotAtGuardPos(transform, guardpoint),
                new GoToGuardPos(transform, guardpoint, _agent),
            })
            //new Patrol(this.transform, waypoints),
        });

        return root;
    }
}