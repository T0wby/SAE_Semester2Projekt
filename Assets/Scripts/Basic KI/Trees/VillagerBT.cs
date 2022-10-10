using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class VillagerBT : RandomWalkTree
{
    public VillagerSettings settings;

    public static float walkSpeed = 2f;
    public static float fleeSpeed = 4f;
    public static float fovRange = 8f;
    public static float safeRange = 8f;
    public static float viewAngle = 200f;
    private NavMeshAgent _agent;
    private int _enemyLayerMask = 1 << 9;

    protected override Node SetupTree()
    {
        _agent = GetComponent<NavMeshAgent>();

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckForEnemyInFOV(transform, fovRange, viewAngle, _enemyLayerMask),
                new Flee(transform, _agent, this),
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
