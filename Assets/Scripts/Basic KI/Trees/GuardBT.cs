using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
public class GuardBT : BehaviorTree.MyTree
{
    public Transform guardpoint;
    public GuardSettings settings;

    protected override Node SetupTree()
    {
        _enemyLayerMask = 1 << 9;
        _agent = GetComponent<NavMeshAgent>();

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckForEnemyInAttackRange(transform, settings.InteractRange),
                new LeafAttack(transform, settings.AtkSpeed),
            }),
            new Sequence(new List<Node>
            {
                new CheckForEnemyInFOV(transform, settings.FovRange, settings.FovAngle, _enemyLayerMask),
                new GoToTarget(transform, _agent, settings)
            }),
            new Sequence(new List<Node>
            {
                new CheckIfNotAtGuardPos(transform, guardpoint),
                new GoToPos(transform, guardpoint, _agent, settings)
            })
        });

        return root;
    }
}