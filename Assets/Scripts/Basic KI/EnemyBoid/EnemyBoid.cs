using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBoid : AEntity, IAttack
{
    [SerializeField] private BoidSettings _settings;

    public UnityEvent OnHealthReduction;

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
    public float Damage { get => _settings.Damage; }
    public float AtkSpeed { get => _settings.AtkSpeed; }

    public void Attack(IMortal enemy)
    {
        enemy.Health -= Damage;
    }

    public override void CheckHealth()
    {
        if (_health <= 0)
        {
            Destroy();
        }
    }

    public override void Destroy()
    {
        gameObject.SetActive(false);
    }
}
