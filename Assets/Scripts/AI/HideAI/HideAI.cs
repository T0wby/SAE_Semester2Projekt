using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HideAI : AEntity
{
    #region Fields
    [SerializeField] private float _maxWalkTime;
    private float _currentWalkTime = 50f;
    private bool _hasSeenEnemy = false;

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

    public float MaxWalkTime
    {
        get { return _maxWalkTime; }
        set { _maxWalkTime = value; }
    }
    public float CurrentWalkTime
    {
        get { return _currentWalkTime; }
        set { _currentWalkTime = value; }
    }

    public bool HasSeenEnemy
    {
        get { return _hasSeenEnemy; }
        set { _hasSeenEnemy = value; }
    }

    #endregion

    #region Methods
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
