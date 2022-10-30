using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class GuardBT : BehaviorTree.MyTree
{
    public Transform guardpoint;
    public GuardSettings settings;
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
                new LeafAttack(transform, settings.AtkSpeed, _animator),
            }),
            new Sequence(new List<Node>
            {
                new CheckForEnemyInFOV(transform, settings.FovRange, settings.FovAngle, _enemyLayerMask),
                new GoToTarget(transform, _agent, settings, _animator)
            }),
            new Sequence(new List<Node>
            {
                new CheckIfNotAtGuardPos(transform, guardpoint, _animator),
                new GoToPos(transform, guardpoint, _agent, settings, _animator)
            })
        });

        return root;
    }
}