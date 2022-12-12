using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject _overheadCamera;
    [SerializeField] private GameObject _thirdPersonCamera;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            _overheadCamera.SetActive(false);
            _thirdPersonCamera.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            _overheadCamera.SetActive(true);
            _thirdPersonCamera.SetActive(false);
        }
    }
}
