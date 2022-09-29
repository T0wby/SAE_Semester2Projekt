using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBT : RandomWalkTree
{
    public static float walkSpeed = 2f;
    public static float fleeSpeed = 4f;
    public static float fovRange = 8f;
    public static float safeRange = 8f;
    public static float attackRange = 2f;
    public static float viewAngle = 200f;
    private NavMeshAgent _agent;
    private int _enemyLayerMask = 1 << 10;


    protected override Node SetupTree()
    {
        _agent = GetComponent<NavMeshAgent>();

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckForEnemyInAttackRange(transform, attackRange),
                new LeafAttack(transform),
            }),
            new Sequence(new List<Node>
            {
                new CheckForEnemyInFOV(transform, fovRange, viewAngle, _enemyLayerMask),
                new Sequence(new List<Node>
                {
                    //TODO: CheckForVillager
                    new GoToTarget(transform, _agent)
                })
            }),
            new Sequence(new List<Node>
            {
                new ChooseDirection(this),
                new WalkAround(transform, _agent, walkSpeed , this)
            })
        });

        return root;
    }
}
