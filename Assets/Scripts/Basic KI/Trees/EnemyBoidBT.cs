using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class EnemyBoidBT : BehaviorTree.MyTree
{
    public Transform[] waypoints;
    public BoidSettings settings;
    public BoidMovement boidMovement;
    private Animator _animator;

    protected override Node SetupTree()
    {
        _enemyLayerMask = 1 << 9;
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
                new GoToTarget(transform, _agent, settings, _animator)
            }),
            new Sequence(new List<Node>
            {
                new Patrol(transform, waypoints, _agent),
                new MinionFlocking(_agent, boidMovement, settings)
            }),
        });

        return root;
    }
}
