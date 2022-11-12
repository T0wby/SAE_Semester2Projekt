using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Villager))]
public class VillagerBT : RandomWalkTree
{
    public VillagerSettings settings;

    [SerializeField] private Transform _foodPoint;
    [SerializeField] private LayerMask _hideableLayers;
    [SerializeField] private TrackHideObject _hideObjects;
    private Villager _villager;
    private Animator _animator;

    protected override Node SetupTree()
    {
        _enemyLayerMask = 1 << 9;
        _agent = GetComponent<NavMeshAgent>();
        _villager = GetComponent<Villager>();
        _animator = GetComponent<Animator>();

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckForEnemyInFOV(transform, settings.FovRange, settings.FovAngle, _enemyLayerMask),
                //new Flee(transform, _agent, this, settings, _animator)
                new Hide(transform, _agent, settings, _hideObjects)
            }),
            new Sequence(new List<Node>
            {
                new CheckIfHungry(_villager),
                new GoToPos(transform, _foodPoint, _agent, settings, _animator)
            }),
            //new Sequence(new List<Node>
            //{
            //    new ChooseDirection(this),
            //    new WalkAround(transform, _agent, settings.WalkSpeed, this, _animator)
            //})
        });

        return root;
    }
}
