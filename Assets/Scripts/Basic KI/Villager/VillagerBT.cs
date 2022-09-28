using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class VillagerBT : BehaviorTree.Tree
{
    public static float walkSpeed = 2f;
    public static float fleeSpeed = 4f;
    public static float fovRange = 8f;
    public static float safeRange = 8f;
    private NavMeshAgent _agent;
    private float _currentWalkTime = 0f;
    private float _maxWalkTime;

    public float CurrentWalkTime { get { return _currentWalkTime; } set { _currentWalkTime = value; } }
    public float MaxWalkTime { get { return _maxWalkTime; } set { _maxWalkTime = value; } }

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
                new WalkAround(transform, _agent, this),
            })
        });

        return root;
    }
}
