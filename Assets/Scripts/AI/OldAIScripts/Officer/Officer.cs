using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Officer : AEntity, IAttack
{
    [SerializeField] private OfficerSettings _settings;

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

    

    private void Awake()
    {
        _health = _settings.HP;
        _walkSpeed = _settings.WalkSpeed;
        _runSpeed = _settings.RunSpeed;
        _fovRange = _settings.FovRange;
        _fovAngle = _settings.FovAngle;
        OnHealthReduction.AddListener(CheckHealth);
    }

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
