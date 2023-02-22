using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateCanvas : MonoBehaviour
{
    #region Fields
    [SerializeField] private List<GameObject> _canvases;
    [SerializeField] string _targetTag = "Player"; 
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_targetTag))
        {
            foreach (GameObject canvas in _canvases)
            {
                canvas.SetActive(!canvas.activeSelf);
            }
        }
    }
}
