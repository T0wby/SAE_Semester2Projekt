using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBoid : AEntity, IMortal, IAttack
{
    [SerializeField] private BoidSettings _settings;

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
    public float Damage { get => _settings.Damage; }
    public float AtkSpeed { get => _settings.AtkSpeed; }

    public void Attack(IMortal enemy)
    {
        throw new System.NotImplementedException();
    }

    public void CheckHealth()
    {
        throw new System.NotImplementedException();
    }

    public void Destroy()
    {
        throw new System.NotImplementedException();
    }
}
