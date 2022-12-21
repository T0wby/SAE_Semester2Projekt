using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuardSettings", menuName = "KI/GuardSettings")]
public class GuardSettings : BasicKISettings
{
    [SerializeField] private float _damage;
    [SerializeField] private float _atkSpeed;

    public float Damage => _damage;
    public float AtkSpeed => _atkSpeed;
}
