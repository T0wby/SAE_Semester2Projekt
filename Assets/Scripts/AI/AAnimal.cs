using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class AAnimal : MonoBehaviour, IMortal
{
    [Header("Settings")]
    [SerializeField] protected AnimalAISettings _settings;
    [SerializeField] protected GameObject _childPrefab;

    [Header("UI")]
    [SerializeField] protected TMP_Text _stateText;
    [SerializeField] protected Image _lifeBar;
    [SerializeField] protected Image _hungerBar;
    [SerializeField] protected Image _thirstBar;
    [SerializeField] protected Image _urgeBar;

    protected float _health;
    protected float _hunger;
    protected float _thirst;
    protected float _reproduceUrge = 0f;
    protected float _reproduceChance;
    protected EAnimalStates _state = EAnimalStates.None;

    public abstract float Health { get; set; }

    public EAnimalStates State
    {
        get => _state;
        set
        {
            _state = value;
            if (OnStateChange != null)
                OnStateChange.Invoke();
        }
    }
    public float ReproduceChance { get => _reproduceChance; }

    public UnityEvent OnHealthChange;
    public UnityEvent OnHungerChange;
    public UnityEvent OnThirstChange;
    public UnityEvent OnReproduceChange;
    public UnityEvent OnStateChange;

    private void Awake()
    {
        
    }

    public abstract void CheckHealth();
    public abstract void CheckHunger();
    public abstract void CheckThirst();
    public abstract void CheckReproduceUrge();
    public abstract void CheckStateChange();
    public abstract void Destroy();
    public abstract void Reproduce();
    public abstract void Drink();
    public abstract void Eat(Grass grass);
}
