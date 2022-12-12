using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Quad : MonoBehaviour
{
    List<Vector3> vertices;
    List<Vector2> uvs;
    List<int> triangles;

    // Start is called before the first frame update
    void Start()
    {
        vertices = new List<Vector3>();
        uvs = new List<Vector2>();
        triangles = new List<int>();

        vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f));
        vertices.Add(new Vector3(-0.5f,  0.5f, -0.5f));
        vertices.Add(new Vector3( 0.5f,  0.5f, -0.5f));
        vertices.Add(new Vector3( 0.5f, -0.5f, -0.5f));

        uvs.Add(new Vector2(0, 0.0f));
        uvs.Add(new Vector2(0, 1f));
        uvs.Add(new Vector2(1f, 1f));
        uvs.Add(new Vector2(1f, 0.0f));

        triangles.Add(0); triangles.Add(1); triangles.Add(3);
        triangles.Add(3); triangles.Add(1); triangles.Add(2);

        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
