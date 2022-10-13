using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Villager : AEntity, IMortal
{
    [SerializeField] private VillagerSettings _settings;
    [Header("Hunger")]
    private float _hunger;
    private float _hungerReductionIntervall;
    private float _maxHungerReduction;
    private float _minHungerReduction;
    [Header("Health")]
    private float _healthReduction;


    public UnityEvent OnHealthReduction;

    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            if (OnHealthReduction != null)
                OnHealthReduction.Invoke();
        }
    }

    private void Awake()
    {
        _health = _settings.HP;
        _walkSpeed = _settings.WalkSpeed;
        _runSpeed = _settings.RunSpeed;
        _fovRange = _settings.FovRange;
        _fovAngle = _settings.FovAngle;
        _hunger = _settings.Hunger;
        // Check for no Value below 0
        _hungerReductionIntervall = _settings.HungerReductionIntervall;
        // Check for no Value below 0 and if max is bigger min
        _maxHungerReduction = _settings.MaxHungerReduction;
        _minHungerReduction = _settings.MinHungerReduction;
        //Check for no Value below 0
        _healthReduction = _settings.HealthReduction;

        OnHealthReduction.AddListener(CheckHealth);
    }

    private void Start()
    {
        StartCoroutine(StartHunger());
    }

    public void CheckHealth()
    {
        if (_health <= 0)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        //gameObject.GetComponent<Rigidbody>().freezeRotation = false;
        //Component.Destroy(gameObject.GetComponent<VillagerBT>());
        //gameObject.tag = null;
        //Component.Destroy(this);
        gameObject.SetActive(false);
    }

    private IEnumerator StartHunger()
    {
        while (true)
        {
            if (CheckIfHungry())
            {
                Health -= _healthReduction;
            }else
                _hunger -= Random.Range(_minHungerReduction, _maxHungerReduction);

            //Debug.Log("hunger: " + _hunger);
            //Debug.Log("health: " + _health);

            yield return new WaitForSeconds(_hungerReductionIntervall);
        }
    }

    private bool CheckIfHungry()
    {
        return _hunger <=0 ? true : false;
    }
}
