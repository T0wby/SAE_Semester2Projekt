using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class BoidMinionBT : BehaviorTree.MyTree
{
    public BoidSettings settings;
    public BoidMovement boidMovement;

    protected override Node SetupTree()
    {
        _enemyLayerMask = 1 << 10;
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
                            new GoToTarget(transform, _agent, settings)
                        }),
                        new Avoid(transform, _agent, settings, boidMovement)
                    })
            }),
             new MinionFlocking(_agent, boidMovement, settings)
        });

        return root;
    }
}
