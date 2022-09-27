using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class GuardBT : BehaviorTree.Tree
{
    public Transform guardpoint;

    public static float speed = 2f;
    public static float fovRange = 6f;
    public static float attackRange = 2f;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckForEnemyInAttackRange(transform),
                new LeafAttack(transform),
            }),
            new Sequence(new List<Node>
            {
                new CheckForEnemyInFOV(transform),
                //new GoToTarget(transform),
                //TODO: NavMesh to Guard prefab
            }),
            new Sequence(new List<Node>
            {
                new CheckIfNotAtGuardPos(transform, guardpoint),
                new GoToGuardPos(transform, guardpoint),
            })
            //new Patrol(this.transform, waypoints),
        });

        return root;
    }
}