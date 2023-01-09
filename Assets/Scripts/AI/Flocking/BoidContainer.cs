using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Boid), typeof(BoidMovement))]
public class BoidContainer : MonoBehaviour
{
    private Boid _boid;
    private BoidMovement _boidMovement;
    private Vector3 _centerDistance;

    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _containerForce = 1f;
    [SerializeField] private GameObject _center;

    private void Awake()
    {
        _boid= GetComponent<Boid>();
        _boidMovement = GetComponent<BoidMovement>();
    }

    private void Update()
    {
        _centerDistance = _center.transform.position - transform.position;
        if (_centerDistance.sqrMagnitude > (_radius * _radius))
        {
            _boidMovement.CurrentVelocity += _centerDistance.normalized * _containerForce * Time.deltaTime;
        }
    }
}
