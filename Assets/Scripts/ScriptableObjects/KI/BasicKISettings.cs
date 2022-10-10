using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicKISettings : ScriptableObject
{
    [SerializeField] private int _hp;
    [SerializeField] private float _fovRange;
    [SerializeField] private float _fovAngle;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _interactRange;

    public int HP => _hp;
    public float FovRange => _fovRange;
    public float FovAngle => _fovAngle;
    public float WalkSpeed => _walkSpeed;
    public float RunSpeed => _runSpeed;
    public float InteractRange => _interactRange;
}
