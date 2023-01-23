using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMax
{
    private float _min;
    private float _max;

    public float Min { get => _min;}
    public float Max { get => _max;}

    public MinMax()
    {
        _min = float.MaxValue; 
        _max = float.MinValue;
    }

    public void AddValue(float value)
    {
        if (value > _max)
        {
            _max = value;
        }
        if (value < _min)
        {
            _min = value;
        }
    }
}
