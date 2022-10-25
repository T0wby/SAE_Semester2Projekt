using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBoidBT : BehaviorTree.MyTree
{
    public Transform[] waypoints;
    public BoidSettings settings;
    public BoidMovement boidMovement;

    protected override Node SetupTree()
    {
        _enemyLayerMask = 1 << 9;
        _agent = GetComponent<NavMeshAgent>();

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckForEnemyInAttackRange(transform, settings.InteractRange),
                new LeafAttack(transform, settings.AtkSpeed)
            }),
            new Sequence(new List<Node>
            {
                new CheckForEnemyInFOV(transform, settings.FovRange, settings.FovAngle, _enemyLayerMask),
                new GoToTarget(transform, _agent, settings)
            }),
            new Sequence(new List<Node>
            {
                new Patrol(transform, waypoints, _agent, settings.WalkSpeed),
                new FlockingEnemy(_agent, boidMovement, settings)
            }),
        });

        return root;
    }
}
