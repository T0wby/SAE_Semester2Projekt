using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoidSettings", menuName ="KI/BoidSettings")]
public class BoidSettings : BasicKISettings
{
    [SerializeField] private float _alignment;
    [SerializeField] private float _cohesion;
    [SerializeField] private float _seperation;
    [SerializeField] private float _target;
    [SerializeField] private float _damage;
    [SerializeField] private float _atkSpeed;

    public float Alignment => _alignment;
    public float Cohesion => _cohesion;
    public float Seperation => _seperation;
    public float Target => _target;
    public float Damage => _damage;
    public float AtkSpeed => _atkSpeed;
}
