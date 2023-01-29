using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Rabbit : AAnimal
{

    #region Properties
    public override float Health
    {
        get => _health;
        set
        {
            _health = value;
            if (OnHealthChange != null)
                OnHealthChange.Invoke();
        }
    }

    public float Hunger
    {
        get => _hunger;
        set
        {
            _hunger = value;
            if (OnHungerChange != null)
                OnHungerChange.Invoke();
        }
    }
    public float Thirst
    {
        get => _thirst;
        set
        {
            _thirst = value;
            if (OnThirstChange != null)
                OnThirstChange.Invoke();
        }
    }
    
    public float ReproduceUrge
    {
        get => _reproduceUrge;
        set
        {
            _reproduceUrge = value;
            if (OnReproduceChange != null)
                OnReproduceChange.Invoke();
        }
    }

    #endregion

    #region Unity
    private void Awake()
    {
        StartingMethods();
    } 
    #endregion


    #region Methods

    #region EventMethods
    public override void CheckHealth()
    {
        _lifeBar.fillAmount = _health * 0.01f;

        if (_health <= 0)
        {
            Destroy();
        }
    }

    public override void CheckHunger()
    {
        if (_hunger < 0)
            _hunger = 0;

        _hungerBar.fillAmount = _hunger * 0.01f;

        if (_hunger < 20f && (_state == EAnimalStates.Move || _state == EAnimalStates.None))
        {
            State = EAnimalStates.Eat;
            return;
        }

        if (_hunger > _settings.MaxHunger)
            _hunger = _settings.MaxHunger;
    }

    public override void CheckThirst()
    {
        if (_thirst < 0)
            _thirst = 0;

        _thirstBar.fillAmount = _thirst * 0.01f;

        if (_thirst < 20f && (_state == EAnimalStates.Move || _state == EAnimalStates.None))
        {
            State = EAnimalStates.Drink;
            return;
        }

        if (_thirst > _settings.MaxThirst)
            _thirst = _settings.MaxThirst;
    }

    public override void CheckReproduceUrge()
    {
        _urgeBar.fillAmount = _reproduceUrge * 0.01f;
        if (_reproduceUrge >= _settings.MaxReproduceUrge && _state != EAnimalStates.Engaged && _state != EAnimalStates.ReproduceReady)
        {
            State = EAnimalStates.ReproduceReady;
            _reproduceUrge = _settings.MaxReproduceUrge;
            return;
        }
    }

    public override void CheckStateChange()
    {
        _stateText.text = $"{_state}";
    }
    #endregion

    /// <summary>
    /// Start all Coroutines on Awake
    /// </summary>
    private void StartingMethods()
    {
        OnHealthChange.AddListener(CheckHealth);
        OnHungerChange.AddListener(CheckHunger);
        OnThirstChange.AddListener(CheckThirst);
        OnReproduceChange.AddListener(CheckReproduceUrge);
        OnStateChange.AddListener(CheckReproduceUrge);
        _health = _settings.MaxHealth;
        _hunger = _settings.MaxHunger;
        _thirst = _settings.MaxThirst;
        _reproduceChance = _settings.ReproduceChance;
        StartCoroutine(ReduceValues());
    }

    public override void Destroy()
    {
        StopAllCoroutines();

        OnHealthChange.RemoveAllListeners();
        OnHungerChange.RemoveAllListeners();
        OnThirstChange.RemoveAllListeners();
        gameObject.SetActive(false);
    }

    public override void Reproduce()
    {
        Instantiate(_childPrefab, this.transform.position, Quaternion.identity, this.transform.parent);
        _reproduceUrge = 0f;
    }

    public override void Drink()
    {
        StartCoroutine(DrinkFull());
    }

    public override void Eat(Grass grass)
    {
        StartCoroutine(EatFull(grass));
    }
    #endregion

    #region Coroutines

    private IEnumerator ReduceValues()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            ReproduceUrge += 10f;
            Thirst -= 15f;
            Hunger -= 10f;
        }
    }

    private IEnumerator DrinkFull()
    {
        State = EAnimalStates.Drink;

        while (_thirst < _settings.MaxThirst)
        {
            yield return new WaitForSeconds(3f);
            Thirst += 10f;
        }

        State = EAnimalStates.None;
    }
    private IEnumerator EatFull(Grass grass)
    {
        State = EAnimalStates.Eat;
        grass.IsTaken = true;


        while (_hunger < _settings.MaxHunger)
        {
            yield return new WaitForSeconds(3f);
            Hunger += 10f;
        }

        Destroy(grass.gameObject);
        State = EAnimalStates.None;
    }
    #endregion

    #region Gizmo

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, _settings.SearchRange);
    }

    

    #endregion
}
