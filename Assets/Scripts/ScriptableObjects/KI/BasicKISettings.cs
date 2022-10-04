using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicKISettings", menuName = "KI/BasicKISettings")]
public class BasicKISettings : ScriptableObject
{
    [SerializeField] private int _hp;
    [SerializeField] private float _fovRange;
    [SerializeField] private float _fovAngle;
    [SerializeField] private float _speed;
    [SerializeField] private float _runSpeed;

    public float FovRange => _fovRange;
    public float FovAngle => _fovAngle;
    public float Speed => _speed;
    public float RunSpeed => _runSpeed;
}
