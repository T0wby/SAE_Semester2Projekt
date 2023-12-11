using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    #region Fields
    private Camera cam;
    #endregion

    private void Start()
    {
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        transform.LookAt(cam.transform);
        transform.Rotate(0, 180, 0);
    }
}
