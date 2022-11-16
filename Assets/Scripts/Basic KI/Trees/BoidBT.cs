using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class BoidBT : BehaviorTree.MyTree
{
    public BoidSettings settings;
    public BoidMovement boidMovement;
    private Animator _animator;

    protected override Node SetupTree()
    {
        _enemyLayerMask = 1 << 10;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckForEnemyInAttackRange(transform, settings.InteractRange, _animator),
                new LeafAttack(transform, settings.AtkSpeed, _animator)
            }),
            new Sequence(new List<Node>
            {
                new CheckForEnemyInFOV(transform, settings.FovRange, settings.FovAngle, _enemyLayerMask),
                new Selector(new List<Node>
                    {
                        new Sequence(new List<Node>
                        {
                            new EnemyIsCertainType<VillagerBT>(),
                            new GoToTarget(transform, _agent, settings, _animator)
                        }),
                        new Avoid(transform, _agent, settings, boidMovement)
                    })
            }),
            new Sequence(new List<Node>
            {
                new CheckIfAtMousePos(transform, settings, _agent),
                new Flocking(transform, _agent, boidMovement, settings)
            }),
            new Idle(_agent)
        });

        return root;
    }
}
