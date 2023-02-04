using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    private Vector3 position;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        //position = cam.WorldToScreenPoint(target.position + offset);

        //if (transform.position != position)
        //    transform.position = position;

        transform.LookAt(cam.transform);
        transform.Rotate(0, 180, 0);
    }
}
