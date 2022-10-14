using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foodpoint : MonoBehaviour
{
    [SerializeField] private float _foodAmount = 5f;
    private Villager _villager;
    private Coroutine _lastCoroutine;


    private void OnCollisionStay(Collision collision)
    {
        _villager = collision.gameObject.GetComponent<Villager>();

        if(_villager is not null)
        {
            if(_lastCoroutine is null)
                _lastCoroutine = StartCoroutine(StartFeeding());
        }
        else
        {
            if (_lastCoroutine is not null)
                StopCoroutine(StartFeeding());
        }
    }

    IEnumerator StartFeeding()
    {
        //TODO: Add max Hunger
        _villager.Hunger += _foodAmount;

        yield return null;
    }
}
