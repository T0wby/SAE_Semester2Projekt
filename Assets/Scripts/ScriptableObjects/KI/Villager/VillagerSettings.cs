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
    [Header("Movement")]
    [SerializeField] private float _fleeSpeed;
    [SerializeField] private float _safeRange;
    [Range(-1f, 1f)]
    [SerializeField] private float _hideSensitivity = 0;

    public float Hunger => _hunger;
    public float HungerReductionIntervall => _hungerReductionIntervall;
    public float MaxHungerReduction => _maxHungerReduction;
    public float MinHungerReduction => _minHungerReduction;
    public float HealthReduction => _healthReduction;
    public float FleeSpeed => _fleeSpeed;
    public float SafeRange => _safeRange;
    public float HideSensitivity => _hideSensitivity;
}
