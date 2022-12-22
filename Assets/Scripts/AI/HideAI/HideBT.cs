using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(HideAI), typeof(NavMeshAgent), typeof(Animator))]
public class HideBT : MyTree
{
    public VillagerSettings settings;

    [SerializeField] private LayerMask _hideableLayers;
    [SerializeField] private TrackHideObject _hideObjects;
    private HideAI _hideAI;
    private Animator _animator;


    protected override Node SetupTree()
    {
        _enemyLayerMask = 1 << 9;
        _agent = GetComponent<NavMeshAgent>();
        _hideAI = GetComponent<HideAI>();
        _animator = GetComponent<Animator>();

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new LF_CheckAlertState(_hideAI),
                new Sequence(new List<Node>
                {
                    new LF_EnemyInRange(settings.HideRange, transform),
                    new Sequence(new List<Node>
                    {
                        new GetHidePosition(_agent, settings, _hideObjects, transform),
                        new SetHideDestination(_agent, settings, _animator)
                    }),
                })
            }),
            new CheckIfNotAtHidePoint(transform, _animator),
            new Sequence(new List<Node>
            {
                new CheckForEnemyInFOV(transform, settings.FovRange, settings.FovAngle, _enemyLayerMask),
                new LF_SetAlertState(_hideAI, true),
            }),
            new Sequence(new List<Node>
            {
                new ChooseDirection(_hideAI),
                new WalkAround(transform, _agent, settings.WalkSpeed, _animator, _hideAI)
            })
        });

        return root;
    }
}
