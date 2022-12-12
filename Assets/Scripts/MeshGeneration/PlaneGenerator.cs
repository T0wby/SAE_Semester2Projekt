using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class PlaneGenerator : MonoBehaviour
{
    private Mesh _mesh;
    private MeshFilter _filter;
    private MeshRenderer _renderer;

    [SerializeField] private Material _meshMat;
    [SerializeField, Range(2,256)] private int _resolution;
    [SerializeField] private float _meshScale;

    private void Awake()
    {
        _filter= GetComponent<MeshFilter>();
        _renderer= GetComponent<MeshRenderer>();

        _mesh = new Mesh();
        _mesh.name = "SnowPlane";
        _filter.sharedMesh = _mesh;

        _renderer.sharedMaterial= _meshMat;
    }

    private void Start()
    {
        GeneratePlane(_resolution);
    }

    private void GeneratePlane(int resolution)
    {
        Vector3[] verts = new Vector3[resolution * resolution];
        int[] tris = new int[(resolution - 1) * (resolution - 1) * 2 * 3];

        Vector2 currPercent = Vector2.zero;
        int triIndex = 0;

        for (int y = 0, i = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++, i++)
            {
                currPercent = new Vector2(x , y) / (resolution - 1);
                currPercent.x -= 0.5f;
                currPercent.y -= 0.5f;
                Vector3 baseVertPos = Vector3.right * _meshScale * currPercent.x + Vector3.forward * _meshScale * currPercent.y;

                verts[i] = baseVertPos;

                if (x < resolution -1 && y < resolution - 1)
                {
                    tris[triIndex++] = i;
                    tris[triIndex++] = i + resolution + 1;
                    tris[triIndex++] = i + 1;

                    tris[triIndex++] = i;
                    tris[triIndex++] = i + resolution;
                    tris[triIndex++] = i + resolution + 1;
                }
            }
        }

        _mesh.Clear();
        _mesh.vertices = verts;
        _mesh.triangles = tris;
        _mesh.RecalculateNormals();
    }
}
