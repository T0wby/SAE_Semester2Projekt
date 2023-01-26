using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RabbitMale : AAnimal
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
    #endregion

    public UnityEvent OnHealthReduction;

    #region Unity
    private void Awake()
    {
        OnHealthReduction.AddListener(CheckHealth);
    }
    #endregion

    public override void CheckHealth()
    {
        if (_health <= 0)
        {
            Destroy();
        }
    }

    public override void Destroy()
    {
        OnHealthReduction.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
