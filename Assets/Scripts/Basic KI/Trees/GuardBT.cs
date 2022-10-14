using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
public class GuardBT : BehaviorTree.Tree
{
    public Transform guardpoint;
    public GuardSettings settings;

    private NavMeshAgent _agent;
    private int _enemyLayerMask = 1 << 9;

    protected override Node SetupTree()
    {
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
                new GoToTarget(transform,_agent),
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