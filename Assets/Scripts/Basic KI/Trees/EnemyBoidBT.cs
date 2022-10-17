using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class EnemyBoidBT : BehaviorTree.Tree
{
    public BoidSettings settings;
    public BoidMovement boidMovement;

    private NavMeshAgent _agent;
    private int _enemyLayerMask = 1 << 9;

    protected override Node SetupTree()
    {
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
                            new GoToTarget(transform, _agent, settings)
                        }),
                        new Avoid(transform, _agent, settings, boidMovement)
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
