using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoidMovement : MonoBehaviour
{
    private List<BoidMovement> _neighbours = new List<BoidMovement>();
    private Vector3 _desiredVelocity = Vector3.zero;
    private Vector3 _currentVelocity;
    private Collider[] _colliders;
    [SerializeField] private BoidSettings _settings;
    private int _boidLayerMask = 1<<10;

    public Vector3 CurrentVelocity { get => _currentVelocity;}

    void Update()
    {
        CheckForOtherBoids();
        CombineMovement();

        Vector3 diff = _desiredVelocity - _currentVelocity;
        _currentVelocity = diff * Time.deltaTime;
        _currentVelocity = Vector3.ClampMagnitude(_currentVelocity, _settings.Speed);
        _currentVelocity *= Time.deltaTime;
        //Use currentVelocity in the Flocking Node
    }

    private void LateUpdate()
    {
        _desiredVelocity = Vector3.zero;
    }

    #region Movement
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Vector3 Alignment()
    {
        if (_neighbours.Count == 0) return Vector3.zero;

        Vector3 direction = Vector3.zero;

        for (int i = 0; i < _neighbours.Count; i++)
        {
            direction += _neighbours[i].CurrentVelocity;
        }

        direction /= _neighbours.Count;

        return direction.normalized * _settings.Speed * _settings.Alignment;
    }

    /// <summary>
    /// Boids move towards the center of their neighbour positions
    /// </summary>
    /// <returns></returns>
    private Vector3 Cohesion()
    {
        if (_neighbours.Count == 0) return Vector3.zero;

        Vector3 center = Vector3.zero;

        for (int i = 0; i < _neighbours.Count; i++)
        {
            center += _neighbours[i].transform.position;
        }

        center /= _neighbours.Count;
        //center -= transform.position;

        return center.normalized * _settings.Speed * _settings.Cohesion;
    }

    /// <summary>
    /// Boids move away from their neighbours
    /// </summary>
    /// <returns></returns>
    private Vector3 Seperation()
    {
        if (_neighbours.Count == 0) return Vector3.zero;

        Vector3 direction = Vector3.zero;
        Vector3 distance;

        for (int i = 0; i < _neighbours.Count; i++)
        {
            distance = _neighbours[i].transform.position - transform.position;
            direction += distance / distance.sqrMagnitude;
        }

        direction /= _neighbours.Count;

        return -direction.normalized * _settings.Speed * _settings.Seperation;
    }
    #endregion

    /// <summary>
    /// Checks if other boids are in the boids FOV
    /// </summary>
    private void CheckForOtherBoids()
    {
        _colliders = Physics.OverlapSphere(transform.position, _settings.FovRange, _boidLayerMask);
        BoidMovement boid;

        foreach (Collider collider in _colliders)
        {
            boid = collider.GetComponent<BoidMovement>();
            if (boid)
            {
                if (Vector3.Angle(transform.forward, boid.transform.position - transform.position) < _settings.FovAngle * 0.5f)
                {
                    _neighbours.Add(boid);
                }
            }
        }
    }

    private void CombineMovement()
    {
        _desiredVelocity += Alignment();
        _desiredVelocity += Cohesion();
        _desiredVelocity += Seperation();
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    BoidMovement boid = other.GetComponent<BoidMovement>();
    //    if (boid is null)
    //        return;

    //    if (Vector3.Angle(transform.forward, boid.transform.position - transform.position) < _settings.FovAngle * 0.5f)
    //    {
    //        _neighbours.Add(boid);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    BoidMovement boid = other.GetComponent<BoidMovement>();
    //    if (boid is null)
    //        return;

    //    if (_neighbours.Contains(boid))
    //    {
    //        _neighbours.Remove(boid);
    //    }
    //}
}
