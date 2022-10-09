using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IAttack 
{
    public float Damage { get;}
    public float AtkSpeed { get;}
    public void Attack(IMortal enemy);
}
