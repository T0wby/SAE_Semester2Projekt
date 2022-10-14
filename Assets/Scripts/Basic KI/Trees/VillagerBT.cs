using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Villager))]
public class VillagerBT : RandomWalkTree
{
    public VillagerSettings settings;

    //Replace
    //public static float walkSpeed = 2f;
    public static float fleeSpeed = 4f;
    //public static float fovRange = 8f;
    public static float safeRange = 8f;
    //public static float viewAngle = 200f;

    [SerializeField] private Transform _foodPoint;
    private Villager _villager;
    private NavMeshAgent _agent;
    private int _enemyLayerMask = 1 << 9;

    protected override Node SetupTree()
    {
        _agent = GetComponent<NavMeshAgent>();
        _villager = GetComponent<Villager>();

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckForEnemyInFOV(transform, settings.FovRange, settings.FovAngle, _enemyLayerMask),
                new Flee(transform, _agent, this, settings)
            }),
            new Sequence(new List<Node>
            {
                new CheckIfHungry(_villager),
                new GoToPos(transform, _foodPoint, _agent, settings)
            }),
            new Sequence(new List<Node>
            {
                new ChooseDirection(this),
                new WalkAround(transform, _agent, settings.WalkSpeed, this)
            })
        });

        return root;
    }
}
