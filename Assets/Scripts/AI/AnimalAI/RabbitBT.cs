using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

[RequireComponent(typeof(NavMeshAgent), typeof(Rabbit))]
public class RabbitBT : BehaviorTree.MyTree
{
    [SerializeField] private AnimalAISettings _settings;
    [SerializeField] private AnimalSearchArea _animalSearchArea;
    private Rabbit _rabbit;

    public Node Root => _root;

    protected override Node SetupTree()
    {
        _agent = GetComponent<NavMeshAgent>();
        _rabbit = GetComponent<Rabbit>();

        _root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new LF_CheckAnimalState(_rabbit, EAnimalStates.Engaged, true),
                new Sequence(new List<Node>
                {
                    new LF_GoToAnimalTarget(this.transform, _agent, _settings, "_reproduceTransform"),
                    new Sequence(new List<Node>
                    {
                        new LF_TargetInRange(this.transform, _settings, "_reproduceTransform"),
                        new LF_StartActivity(_rabbit, EAnimalStates.Engaged)
                    }),
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
                    new LF_TargetInRadius(this.transform, _rabbit, _animalSearchArea, ETargetTypes.Animal),
                    new LF_SetAnimalState(_rabbit, EAnimalStates.Engaged),
                }),
            }),
            new Sequence(new List<Node>
            {
                new LF_CheckAnimalState(_rabbit, EAnimalStates.Drink, true),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                    new LF_TargetInRadius(this.transform, _rabbit, _animalSearchArea, ETargetTypes.Water),
                    new LF_GoToAnimalTarget(this.transform, _agent, _settings, "_waterTarget"),
                    new LF_TargetInRange(this.transform, _settings, "_waterTarget"),
                    new LF_StartActivity(_rabbit, EAnimalStates.Drink)
                    }),
                }),
                new LF_MoveAround(this.transform, _agent, _settings, _rabbit)
            }),
            new Sequence(new List<Node>
            {
                new LF_CheckAnimalState(_rabbit, EAnimalStates.Eat, true),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new LF_TargetInRadius(this.transform, _rabbit, _animalSearchArea, ETargetTypes.Grass),
                        new LF_GoToAnimalTarget(this.transform, _agent, _settings, "_eatTargetTransform"),
                        new LF_TargetInRange(this.transform, _settings, "_eatTargetTransform"),
                        new LF_StartActivity(_rabbit, EAnimalStates.Eat),
                    }),
                    new LF_MoveAround(this.transform, _agent, _settings, _rabbit)
                }),
            }),
            new LF_MoveAround(this.transform, _agent, _settings, _rabbit)
        });

        return _root;
    }
}
