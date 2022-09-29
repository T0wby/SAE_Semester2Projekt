using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoidSettings", menuName ="KI/BoidSettings")]
public class BoidSettings : ScriptableObject
{
    [SerializeField] private float _alignment;
    [SerializeField] private float _cohesion;
    [SerializeField] private float _seperation;
    [SerializeField] private float _target;
    [SerializeField] private float _fovRange;
    [SerializeField] private float _fovAngle;
    [SerializeField] private float _speed;

    public float Alignment { get => _alignment;}
    public float Cohesion { get => _cohesion;}
    public float Seperation { get => _seperation;}
    public float Target { get => _target;}
    public float FovRange { get => _fovRange; }
    public float FovAngle { get => _fovAngle; }
    public float Speed { get => _speed; }
}
