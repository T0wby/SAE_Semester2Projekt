using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

[RequireComponent(typeof(NavMeshAgent), typeof(Rabbit))]
public class RabbitBT : BehaviorTree.MyTree
{
    [SerializeField] private AnimalAISettings _settings;
    private Rabbit _rabbit;

    protected override Node SetupTree()
    {
        _enemyLayerMask = 1 << 9;
        _agent = GetComponent<NavMeshAgent>();
        _rabbit = GetComponent<Rabbit>();

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new LF_CheckAnimalState(_rabbit, EAnimalStates.Engaged, true),
                new Sequence(new List<Node>
                {
                    new LF_GoToAnimalTarget(this.transform, _agent, _settings, "_reproduceTransform"),
                }),
            }),
            new Sequence(new List<Node>
            {
                new LF_CheckAnimalState(_rabbit, EAnimalStates.ReproRequest, true),
                new Sequence(new List<Node>
                {
                    new LF_CheckAnimalState(_rabbit, EAnimalStates.Engaged, false),
                    new LF_SetAnimalState(_rabbit, EAnimalStates.Engaged)
                }),
            }),
            new Sequence(new List<Node>
            {
                new LF_CheckAnimalState(_rabbit, EAnimalStates.ReproduceReady, true),
                new Sequence(new List<Node>
                {
                    // Target in Search radius ?
                        // got the same state ?
                        // set _animalTarget
                            // Change to ReproRequest on both and set _reproduceTransform
                }),
            }),
            new Sequence(new List<Node>
            {
                new LF_CheckAnimalState(_rabbit, EAnimalStates.Drink, true),
                new Sequence(new List<Node>
                {
                    // Target in Search radius ?
                }),
            }),
            new Sequence(new List<Node>
            {
                new LF_CheckAnimalState(_rabbit, EAnimalStates.Eat, true),
                new Sequence(new List<Node>
                {
                    // Target in Search radius ?
                }),
            }),
            // Move
        });

        return root;
    }
}
