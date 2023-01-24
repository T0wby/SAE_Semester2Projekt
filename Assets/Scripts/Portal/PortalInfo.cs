using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PortalInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text _portalName;

    private void Awake()
    {
        _portalName.text = gameObject.name;
    }
}
