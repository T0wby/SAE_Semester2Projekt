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
}
