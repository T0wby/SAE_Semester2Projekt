using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class BoidBT : BehaviorTree.Tree
{
    public BoidSettings settings;
    public BoidMovement boidMovement;

    //public static float speed = 2f;
    //public static float fovRange = 6f;
    //public static float attackRange = 1.5f;
    //public static float viewAngle = 60f;

    private NavMeshAgent _agent;
    private int _enemyLayerMask = 1 << 10;

    protected override Node SetupTree()
    {
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
                new Selector(new List<Node>
                    {
                        new Sequence(new List<Node>
                        {
                            new EnemyIsCertainType<VillagerBT>(),
                            new GoToTarget(transform, _agent)
                        }),
                        new Avoid(transform, _agent, settings)
                    })
            }),
            new Sequence(new List<Node>
            {
                new CheckIfAtTargetPos(transform, settings),
                new Flocking(transform, _agent, boidMovement, settings)
            }),
            new Idle(_agent)
        });

        return root;
    }
}