using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class VillagerBT : RandomWalkTree
{
    public static float walkSpeed = 2f;
    public static float fleeSpeed = 4f;
    public static float fovRange = 8f;
    public static float safeRange = 8f;
    private NavMeshAgent _agent;

    protected override Node SetupTree()
    {
        _agent = GetComponent<NavMeshAgent>();

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckForEnemyInFOV(transform, fovRange),
                new Flee(transform, _agent),
            }),
            new Sequence(new List<Node>
            {
                new ChooseDirection(this),
                new WalkAround(transform, _agent, walkSpeed, this),
            })
        });

        return root;
    }
}
