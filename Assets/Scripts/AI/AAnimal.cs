using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AAnimal : MonoBehaviour, IMortal
{
    protected float _health;
    protected float _hunger;
    protected float _thirst;
    protected float _walkSpeed;
    protected float _searchRange;
    protected float _interactRange;
    protected float _reproduceUrge;
    public abstract float Health { get; set; }

    public abstract void CheckHealth();
    public abstract void Destroy();
}
