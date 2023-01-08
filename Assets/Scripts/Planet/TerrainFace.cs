using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TerrainFace
{
    private ShapeGenerator _shapeGenerator;

    private Mesh _mesh;
    private int _currentResolution;
    private bool _useFancySphere;

    private Vector3 _localUpVector;
    private Vector3 _axisA;
    private Vector3 _axisB;
    private Vector3[] _verts;
    private int[] _tris;
    private Thread _thread;

    public TerrainFace(Mesh mesh, ShapeGenerator shapeGenerator, int resolution, Vector3 localUp, bool useFancySphere)
    {
        this._shapeGenerator = shapeGenerator;

        this._mesh = mesh;
        _currentResolution = resolution;
        this._useFancySphere = useFancySphere;

        _localUpVector = localUp;
        _axisA = new Vector3(_localUpVector.y, _localUpVector.z, _localUpVector.x);
        _axisB = Vector3.Cross(_axisA, _localUpVector);
    }

    public void GenerateMesh(bool useThreading)
    {
        Stopwatch st = new Stopwatch();
        st.Start();

        if (useThreading)
        {
            _thread = new Thread(CalculateMesh);
            _thread.Start();
        }
        else
            CalculateMesh();

        st.Stop();
        UnityEngine.Debug.Log($"GeneratePlanet took {st.ElapsedMilliseconds} ms to complete");
    }

    private void CalculateMesh()
    {

        //Stopwatch st = new Stopwatch();
        //st.Start();


        _verts = new Vector3[_currentResolution * _currentResolution];
        _tris = new int[(_currentResolution - 1) * (_currentResolution - 1) * 2 * 3];

        Vector3 rootPos = _localUpVector;
        Vector2 currPercent = Vector2.zero;
        int triIdx = 0;
        for (int y = 0, i = 0; y < _currentResolution; y++)
        {
            for (int x = 0; x < _currentResolution; x++, i++)
            {
                currPercent = new Vector2(x, y) / (_currentResolution - 1);
                currPercent.x -= 0.5f;
                currPercent.y -= 0.5f;

                Vector3 cubeVertPos = rootPos + _axisA * 2f * currPercent.x + _axisB * 2f * currPercent.y;
                Vector3 sphereVertPos = _shapeGenerator.TransformCubeToSpherePos(cubeVertPos, _useFancySphere);
                Vector3 planetVertPos = _shapeGenerator.CalculatePointOnPlanet(sphereVertPos);
                Vector3 transformedPos = _shapeGenerator.TransformPointWithOwnTransformMatrix(planetVertPos);

                _verts[i] = transformedPos;

                if (x < _currentResolution - 1 && y < _currentResolution - 1)
                {
                    _tris[triIdx++] = i;
                    _tris[triIdx++] = i + _currentResolution + 1;
                    _tris[triIdx++] = i + 1;

                    _tris[triIdx++] = i;
                    _tris[triIdx++] = i + _currentResolution;
                    _tris[triIdx++] = i + _currentResolution + 1;
                }
            }
        }


        //st.Stop();
        //UnityEngine.Debug.Log($"GeneratePlanet took {st.ElapsedMilliseconds} ms to complete");

    }

    public bool SetMeshValues()
    {
        if (_thread is null)
        {
            _mesh.Clear();
            _mesh.vertices = _verts;
            _mesh.triangles = _tris;
            _mesh.RecalculateNormals();
            return false;
        }
        else
        {
            bool isThreadAlive = _thread.IsAlive;
            if (isThreadAlive)
                return isThreadAlive;

            _mesh.Clear();
            _mesh.vertices = _verts;
            _mesh.triangles = _tris;
            _mesh.RecalculateNormals();
            return isThreadAlive;
        }
    }
}
