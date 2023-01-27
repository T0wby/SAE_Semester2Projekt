using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAISettings : ScriptableObject
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _maxHunger;
    [SerializeField] private float _maxThirst;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _searchRange;
    [SerializeField] private float _interactRange;
    [SerializeField] private float _maxReproduceUrge;
    [SerializeField, Range(0,50)] private float _reproduceChance;

    public float MaxHealth { get => _maxHealth; }
    public float MaxHunger { get => _maxHunger; }
    public float MaxThirst { get => _maxThirst; }
    public float WalkSpeed { get => _walkSpeed; }
    public float RunSpeed { get => _runSpeed; }
    public float SearchRange { get => _searchRange; }
    public float InteractRange { get => _interactRange; }
    public float ReproduceChance { get => _reproduceChance; }
    public float MaxReproduceUrge { get => _maxReproduceUrge; }
}
