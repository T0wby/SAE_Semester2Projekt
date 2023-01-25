using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    private bool _isTaken = false;

    public bool IsTaken { get => _isTaken; set => _isTaken = value; }
}
