using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow;

[CreateAssetMenu(fileName = "VillagerSettings", menuName = "KI/VillagerSettings")]
public class VillagerSettings : BasicKISettings
{
    [Header("Hunger")]
    [SerializeField] private float _hunger;
    [SerializeField] private float _hungerReductionIntervall;
    [SerializeField] private float _maxHungerReduction;
    [SerializeField] private float _minHungerReduction;
    [Header("Health")]
    [SerializeField] private float _healthReduction;

    public float Hunger => _hunger;
    public float HungerReductionIntervall => _hungerReductionIntervall;
    public float MaxHungerReduction => _maxHungerReduction;
    public float MinHungerReduction => _minHungerReduction;
    public float HealthReduction => _healthReduction;

    //public float MinHungerReduction
    //{
    //    get { return _minHungerReduction; }
    //    set { _minHungerReduction = value;
    //        if (_minHungerReduction < 0f)
    //            _minHungerReduction = 0f;
    //        }
    //}

    //public float HealthReduction
    //{
    //    get { return _healthReduction; }
    //    set
    //    {
    //        _healthReduction = value;
    //        if (_healthReduction < 0f)
    //            _healthReduction = 0f;
    //    }
    //}

}
