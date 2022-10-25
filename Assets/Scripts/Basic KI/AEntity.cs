using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEntity : MonoBehaviour, IMortal
{
    protected float _health;
    protected float _walkSpeed;
    protected float _runSpeed;
    protected float _fovRange;
    protected float _fovAngle;

    public virtual float Health { get; set; }

    public virtual void CheckHealth()
    {
    }

    public virtual void Destroy()
    {
    }
}
