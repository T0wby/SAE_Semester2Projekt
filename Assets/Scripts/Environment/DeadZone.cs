using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DeadZone : MonoBehaviour
{
    [SerializeField] private GameObject _startPoint;
    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = _startPoint.transform.position;
        }
    }
}
