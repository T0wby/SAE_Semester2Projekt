using System.Threading;
using UnityEngine;

public class TerrainSide
{
    #region Fields
    private ShapeGeneratorTwo _shapeGenerator;
    private Mesh _mesh;
    private int _resolution;
    private bool _useFancySphere = true;
    private Vector3 _localUp;
    private Vector3 _axisA;
    private Vector3 _axisB;
    private Thread _thread;
    private Vector3[] _vertices;
    private int[] _triangles;
    #endregion

    #region Constructor
    public TerrainSide(ShapeGeneratorTwo shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        _shapeGenerator = shapeGenerator;
        _mesh = mesh;
        _resolution = resolution;
        _localUp = localUp;
        _axisA = new Vector3(_localUp.y, _localUp.z, _localUp.x);
        _axisB = Vector3.Cross(localUp, _axisA);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Generates the mesh of the terrainside
    /// </summary>
    /// <param name="useThreading">Determines if we use multithreading for the calculation or not</param>
    /// <param name="useFancySphere">Determines if we use the FancySphere or not</param>
    public void GenerateMesh(bool useThreading, bool useFancySphere)
    {
        _useFancySphere = useFancySphere;

        if (useThreading)
        {
            // Create a new thread for the calculation of vertices and triangles
            _thread = new Thread(ConstructMesh);
            _thread.Start();
        }
        else
            ConstructMesh();

    }

    /// <summary>
    /// Calculating each verice and triangle of the TerrainSide
    /// </summary>
    public void ConstructMesh()
    {
        // Number of vertices per mesh
        _vertices = new Vector3[_resolution * _resolution];
        // Number of triangles per mesh
        _triangles = new int[(_resolution - 1) * (_resolution - 1) * 6];

        int triIdx = 0;
        for (int y = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++)
            {
                int i = x + y * _resolution;
                // Getting the percentage of completion of the mesh
                // (_resolution - 1) because we do not loop over the last _resolution index
                Vector2 percent = new Vector2(x, y) / (_resolution - 1);

                Vector3 pointOnUnitCube = _localUp + (percent.x - 0.5f) * 2 * _axisA + (percent.y - 0.5f) * 2 * _axisB;
                Vector3 pointOnUnitSphere = _shapeGenerator.TransformCubeToSpherePos(pointOnUnitCube, _useFancySphere);
                _vertices[i] = _shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);

                // Creating triangles
                if (x < _resolution - 1 && y < _resolution - 1)
                {
                    _triangles[triIdx++] = i;
                    _triangles[triIdx++] = i + _resolution + 1;
                    _triangles[triIdx++] = i + _resolution;

                    _triangles[triIdx++] = i;
                    _triangles[triIdx++] = i + 1;
                    _triangles[triIdx++] = i + _resolution + 1;
                }
            }
        }
    }

    /// <summary>
    /// Set the values depending on the thread status
    /// Done extra cause we access unity objects, which are not thread safe
    /// </summary>
    /// <returns>Returns false if the calculation is not done yet or true if it is</returns>
    public bool SetMeshValues()
    {
        Vector2[] uv = _mesh.uv;

        if (_thread is null)
        {
            _mesh.Clear();
            _mesh.vertices = _vertices;
            _mesh.triangles = _triangles;
            _mesh.RecalculateNormals();
            _mesh.uv = uv;
            return false;
        }
        else
        {
            bool isThreadAlive = _thread.IsAlive;
            if (isThreadAlive)
                return isThreadAlive;

            _mesh.Clear();
            _mesh.vertices = _vertices;
            _mesh.triangles = _triangles;
            _mesh.RecalculateNormals();
            _mesh.uv = uv;
            return isThreadAlive;
        }
    }

    /// <summary>
    /// Update UV values for the planet
    /// </summary>
    /// <param name="colourGenerator">ColourGenerator of the planet</param>
    public void UpdateUV(ColourGenerator colourGenerator)
    {
        Vector2[] uv = new Vector2[_resolution * _resolution];

        for (int y = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++)
            {
                int i = x + y * _resolution;
                // Getting the percentage of completion
                // (_resolution - 1) because we do not loop over the last _resolution index
                Vector2 percent = new Vector2(x, y) / (_resolution - 1);

                Vector3 pointOnUnitCube = _localUp + (percent.x - 0.5f) * 2 * _axisA + (percent.y - 0.5f) * 2 * _axisB;
                Vector3 pointOnUnitSphere = _shapeGenerator.TransformCubeToSpherePos(pointOnUnitCube, _useFancySphere);

                uv[i] = new Vector2(colourGenerator.BiomePercentageFromPoint(pointOnUnitSphere), 0);
            }
        }
        if (_mesh.uv.Length == uv.Length)
        {
            _mesh.uv = uv;
        }
    } 
    #endregion
}