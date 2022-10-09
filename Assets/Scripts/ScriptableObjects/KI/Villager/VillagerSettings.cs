using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow;

[CreateAssetMenu(fileName = "VillagerSettings", menuName = "KI/VillagerSettings")]
public class VillagerSettings : BasicKISettings
{
    [SerializeField] private float _hunger;

    public float Hunger => _hunger;

}
