using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foodpoint : MonoBehaviour
{
    [SerializeField] private float _foodAmount = 5f;
    [SerializeField] private float _feedingInterval = 1f;
    private List<Villager>  _villagerList;
    private Villager  _villager;
    private Coroutine _lastCoroutine;

    private void Awake()
    {
        _villagerList = new List<Villager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _villager = other.gameObject.GetComponent<Villager>();

        if (_villager == null)
            return;

        _villagerList.Add(_villager);

        if (_villagerList.Count > 0 && _lastCoroutine is null)
            _lastCoroutine = StartCoroutine(StartFeeding());
    }

    private void OnTriggerExit(Collider other)
    {
        _villagerList.Remove(other.gameObject.GetComponent<Villager>());

        if (_villagerList.Count < 1)
        {
            StopCoroutine(StartFeeding());
            _lastCoroutine = null;
        }
    }

    IEnumerator StartFeeding()
    {
        //TODO: Add max Hunger

        if (_villagerList.Count > 0)
        {
            for (int i = 0; i < _villagerList.Count; i++)
            {
                _villagerList[i].Hunger += _foodAmount;
            }
        }
        

        yield return new WaitForSeconds(_feedingInterval);
    }
}
