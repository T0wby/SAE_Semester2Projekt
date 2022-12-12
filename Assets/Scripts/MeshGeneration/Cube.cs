using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum faces : byte
{
    front = 1,
    back = 2,
    left = 4,
    right = 8,
    top = 16,
    bottom = 32
}


[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Cube : MonoBehaviour
{
    //[SerializeField]
    public Vector2Int numTextures;
    public byte neighbours;

    List<Vector3> cubevertices;
    List<Vector3> vertices;
    List<Vector2> uvs;
    List<int> triangles;

    // Start is called before the first frame update
    public void Build(byte neighbours)
    {
        this.neighbours = neighbours;
        cubevertices = new List<Vector3>();
        vertices = new List<Vector3>();
        uvs = new List<Vector2>();
        triangles = new List<int>();

        cubevertices.Add(new Vector3(-0.5f, -0.5f, -0.5f)); //0 vul
        cubevertices.Add(new Vector3(-0.5f, 0.5f, -0.5f)); //1 vol
        cubevertices.Add(new Vector3(0.5f, 0.5f, -0.5f)); //2 vor
        cubevertices.Add(new Vector3(0.5f, -0.5f, -0.5f)); //3 vur
        cubevertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));  //4 hul
        cubevertices.Add(new Vector3(-0.5f, 0.5f, 0.5f));  //5 hol
        cubevertices.Add(new Vector3(0.5f, 0.5f, 0.5f));  //6 hor
        cubevertices.Add(new Vector3(0.5f, -0.5f, 0.5f));  //7 hur

        //front

        if ((neighbours & (int)faces.front) != (int)faces.front)
        {
            vertices.Add(cubevertices[0]);
            vertices.Add(cubevertices[1]);
            vertices.Add(cubevertices[2]);
            vertices.Add(cubevertices[3]);
            CalculateUVs(243);
            CalculateIndices();
        }

        //back
        if ((neighbours & (int)faces.back) != (int)faces.back)
        {
            vertices.Add(cubevertices[7]);
            vertices.Add(cubevertices[6]);
            vertices.Add(cubevertices[5]);
            vertices.Add(cubevertices[4]);
            CalculateUVs(243);
            CalculateIndices();
        }


        //left
        if ((neighbours & (int)faces.left) != (int)faces.left)
        {
            vertices.Add(cubevertices[4]);
            vertices.Add(cubevertices[5]);
            vertices.Add(cubevertices[1]);
            vertices.Add(cubevertices[0]);
            CalculateUVs(243);
            CalculateIndices();
        }

        //right
        if ((neighbours & (int)faces.right) != (int)faces.right)
        {
            vertices.Add(cubevertices[3]);
            vertices.Add(cubevertices[2]);
            vertices.Add(cubevertices[6]);
            vertices.Add(cubevertices[7]);
            CalculateUVs(243);
            CalculateIndices();
        }

        //top
        if ((neighbours & (int)faces.top) != (int)faces.top)
        {
            vertices.Add(cubevertices[1]);
            vertices.Add(cubevertices[5]);
            vertices.Add(cubevertices[6]);
            vertices.Add(cubevertices[2]);
            CalculateUVs(60);
            CalculateIndices();
        }

        //bottom
        if ((neighbours & (int)faces.bottom) != (int)faces.bottom)
        {
            vertices.Add(cubevertices[4]);
            vertices.Add(cubevertices[0]);
            vertices.Add(cubevertices[3]);
            vertices.Add(cubevertices[7]);
            CalculateUVs(242);
            CalculateIndices();
        }

        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();

        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void CalculateIndices()
    {
        var index = vertices.Count - 4;
        triangles.Add(index); triangles.Add(index+1); triangles.Add(index + 3);
        triangles.Add(index + 3); triangles.Add(index + 1); triangles.Add(index + 2);
    }

    private void CalculateUVs(int textureNumber = 15)
    {
        float sizeX = 1.0f / numTextures.x;
        float sizeY = 1.0f / numTextures.y;

        float startX = (textureNumber % numTextures.x) * sizeX;
        float startY = (textureNumber / numTextures.x) * sizeY;

        uvs.Add(new Vector2(startX, startY));
        uvs.Add(new Vector2(startX, startY + sizeY));
        uvs.Add(new Vector2(startX + sizeX, startY + sizeY));
        uvs.Add(new Vector2(startX + sizeX, startY));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
