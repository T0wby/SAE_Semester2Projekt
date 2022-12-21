using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OfficerSettings", menuName = "KI/OfficerSettings")]
public class OfficerSettings : BasicKISettings
{
    [SerializeField] private float _damage;
    [SerializeField] private float _atkSpeed;

    public float Damage => _damage;
    public float AtkSpeed => _atkSpeed;
}
