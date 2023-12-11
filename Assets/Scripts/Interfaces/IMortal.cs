using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMortal
{
    public float Health { get; set; }
    void Destroy();
    void CheckHealth();

}
