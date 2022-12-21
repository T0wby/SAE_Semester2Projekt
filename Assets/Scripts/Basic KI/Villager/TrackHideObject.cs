using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TrackHideObject : MonoBehaviour
{
    #region Attributes
    [SerializeField] private VillagerSettings _settings;
    private List<Collider> _colliders;
    private SphereCollider _sphereCollider;
    #endregion

    #region Properties
    public List<Collider> Colliders { get => _colliders; }
    #endregion


    #region Unity

    private void Awake()
    {
        _colliders = new List<Collider>();
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = _settings.HideRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _settings.HideLayerInt)
            _colliders.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _colliders.Remove(other);
    }

    #endregion
}
