using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAI : MonoBehaviour
{
    [SerializeField] private float _maxWalkTime;
    private float _currentWalkTime;

    public float MaxWalkTime
    {
        get { return _maxWalkTime; }
        set { _maxWalkTime = value; }
    }
    public float CurrentWalkTime
    {
        get { return _currentWalkTime; }
        set { _currentWalkTime = value; }
    }
}
