using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EAnimalStates
{
    None,
    Move,
    Eat,
    Drink,
    ReproduceReady,
    ReproRequest,
    Engaged
}

public abstract class AAnimal : MonoBehaviour, IMortal
{
    [SerializeField] protected AnimalAISettings _settings;
    [SerializeField] protected GameObject _childPrefab;
    protected float _health;
    protected float _hunger;
    protected float _thirst;
    protected float _reproduceUrge = 0f;
    private float _reproduceChance;
    protected EAnimalStates _state = EAnimalStates.None;

    public abstract float Health { get; set; }
    public EAnimalStates State { get => _state; set => _state = value; }
    public float ReproduceChance { get => _reproduceChance; }

    public UnityEvent OnHealthReduction;
    public UnityEvent OnHungerReduction;
    public UnityEvent OnThirstReduction;
    public UnityEvent OnReproduceAddition;

    private void Awake()
    {
        OnHealthReduction.AddListener(CheckHealth);
        OnHungerReduction.AddListener(CheckHunger);
        OnThirstReduction.AddListener(CheckThirst);
        OnReproduceAddition.AddListener(CheckReproduceUrge);
        _health = _settings.MaxHealth;
        _hunger = _settings.MaxHunger;
        _thirst = _settings.MaxThirst;
        _reproduceChance = _settings.ReproduceChance;
    }

    public abstract void CheckHealth();
    public abstract void CheckHunger();
    public abstract void CheckThirst();
    public abstract void CheckReproduceUrge();
    public abstract void Destroy();
    public abstract void Reproduce();
    public abstract void Drink();
    public abstract void Eat();
}
