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
            if (OnHealthReduction != null)
                OnHealthReduction.Invoke();
        }
    }

    public float Hunger
    {
        get => _hunger;
        set
        {
            _hunger = value;
            if (OnHungerReduction != null)
                OnHungerReduction.Invoke();
        }
    }
    public float Thirst
    {
        get => _thirst;
        set
        {
            _thirst = value;
            if (OnThirstReduction != null)
                OnThirstReduction.Invoke();
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
        if (_health <= 0)
        {
            Destroy();
        }
    }

    public override void CheckHunger()
    {
        if (_hunger < 0)
            _hunger = 0;

        if (_hunger < 20f && (_state == EAnimalStates.Move || _state == EAnimalStates.None))
        {
            _state = EAnimalStates.Eat;
            return;
        }

        if (_hunger > _settings.MaxHunger)
            _hunger = _settings.MaxHunger;
    }

    public override void CheckThirst()
    {
        if (_thirst < 0)
            _thirst = 0;

        if (_thirst < 20f && (_state == EAnimalStates.Move || _state == EAnimalStates.None))
        {
            _state = EAnimalStates.Drink;
            return;
        }

        if (_thirst > _settings.MaxThirst)
            _thirst = _settings.MaxThirst;
    }

    public override void CheckReproduceUrge()
    {
        if (_reproduceUrge >= _settings.MaxReproduceUrge && _state != EAnimalStates.Engaged)
        {
            _state = EAnimalStates.ReproduceReady;
            _reproduceUrge = _settings.MaxReproduceUrge;
            return;
        }
    }
    #endregion

    /// <summary>
    /// Start all Coroutines on Awake
    /// </summary>
    private void StartingMethods()
    {
        StartCoroutine(ReduceValues());
    }

    public override void Destroy()
    {
        StopAllCoroutines();

        OnHealthReduction.RemoveAllListeners();
        OnHungerReduction.RemoveAllListeners();
        OnThirstReduction.RemoveAllListeners();
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

    public override void Eat()
    {
        StartCoroutine(EatFull());
    }
    #endregion

    #region Coroutines

    private IEnumerator ReduceValues()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            _reproduceUrge += 10f;
            _thirst -= 15f;
            _hunger -= 10f;
        }
    }

    private IEnumerator DrinkFull()
    {
        _state = EAnimalStates.Drink;

        while (_thirst < _settings.MaxThirst)
        {
            yield return new WaitForSeconds(3f);
            _thirst += 10f;
        }

        _state = EAnimalStates.None;
    }
    private IEnumerator EatFull()
    {
        _state = EAnimalStates.Eat;

        while (_hunger < _settings.MaxHunger)
        {
            yield return new WaitForSeconds(3f);
            _hunger += 10f;
        }

        _state = EAnimalStates.None;
    }
    #endregion
}
