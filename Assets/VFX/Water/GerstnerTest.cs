using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerstnerTest : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float wavelength = 2f;
    public float speed = 1f;
    public Vector2 direction = new Vector2(1, 1);

    private Mesh mesh;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().sharedMesh;
    }

    void Update()
    {
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            vertex.y = CalculateGerstnerWaveHeight(vertex.x, vertex.z);
            vertices[i] = vertex;
        }
        mesh.vertices = vertices;
    }

    float CalculateGerstnerWaveHeight(float x, float z)
    {
        float steepness = 0.5f;
        float k = 2f * Mathf.PI / wavelength;
        float c = Mathf.Sqrt(9.8f / k);
        float q = k * (direction.x * x + direction.y * z) + speed * Time.time;
        float wx = amplitude * direction.x * Mathf.Cos(q);
        float wz = amplitude * direction.y * Mathf.Sin(q);
        float dy = steepness * k * (wx + wz);
        return dy;
    }
}
