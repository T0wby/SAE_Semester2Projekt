using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnowAI : AEntity, IAttack
{
    #region Fields
    [SerializeField] private OfficerSettings _settings;

    public UnityEvent OnHealthReduction; 
    #endregion

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
    public float Damage { get => _settings.Damage; }
    public float AtkSpeed { get => _settings.AtkSpeed; } 
    #endregion



    private void Awake()
    {
        _health = _settings.HP;
        _walkSpeed = _settings.WalkSpeed;
        _runSpeed = _settings.RunSpeed;
        _fovRange = _settings.FovRange;
        _fovAngle = _settings.FovAngle;
        OnHealthReduction.AddListener(CheckHealth);
    }

    #region Methods
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
    #endregion
}
