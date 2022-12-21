using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortToPoint : MonoBehaviour
{
    [SerializeField] Transform _targetPoint;
    [SerializeField] string _targetTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_targetTag))
        {
            other.gameObject.transform.position = _targetPoint.position;
        }
    }
}
