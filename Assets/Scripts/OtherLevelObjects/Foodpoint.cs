using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foodpoint : MonoBehaviour
{
    [SerializeField] private float _foodAmount = 5f;
    private Villager _villager; //Change to List and save all who entered and start feeding them.
    private Coroutine _lastCoroutine;


    //private void OnCollisionStay(Collision collision)
    //{
    //    _villager = collision.gameObject.GetComponent<Villager>();

    //    if(_villager is not null)
    //    {
    //        _villager.Hunger += _foodAmount;
    //        //if(_lastCoroutine is null)
    //        //    _lastCoroutine = StartCoroutine(StartFeeding());
    //    }
    //    else
    //    {
    //        //if (_lastCoroutine is not null)
    //        //    StopCoroutine(StartFeeding());
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        _villager = collision.gameObject.GetComponent<Villager>();

        if (_villager is not null)
            StartCoroutine(StartFeeding());
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }

    IEnumerator StartFeeding()
    {
        //TODO: Add max Hunger
        _villager.Hunger += _foodAmount;

        yield return null;
    }
}
