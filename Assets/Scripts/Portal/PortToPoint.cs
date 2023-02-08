using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortToPoint : MonoBehaviour
{
    [SerializeField] Transform _targetPoint;
    [SerializeField] string _targetTag = "Player";

    public UnityEvent OnEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_targetTag))
        {
            other.gameObject.transform.position = _targetPoint.position;
            if(OnEnter != null)
                OnEnter.Invoke();
        }
    }
}
