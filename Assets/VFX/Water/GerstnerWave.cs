using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GerstnerWave : MonoBehaviour
{
    private struct VertexPosition
    {
        public Vector3 position;
    }

    [SerializeField] private ComputeShader _shader;
    [SerializeField] private float _waveAmplitude = 1f;
    [SerializeField] private float _waveWavelength = 10f;
    [SerializeField] private float _waveSteepness = 0.5f;
    [SerializeField] private Vector2 _waveDirection = Vector2.right;
    [SerializeField] private float _waveSpeed = 1f;
    private Mesh _mesh;
    private int _kernelHandle;
    private ComputeBuffer _vertexBuffer;
    private int _groupSizeX;
    private int _verticeCount;
    private Vector3[] _meshVertices;
    private VertexPosition[] _vertexPos;

    private void Start()
    {
        _kernelHandle = _shader.FindKernel("CSMain");
        _mesh = GetComponent<MeshFilter>().sharedMesh;
        _verticeCount = _mesh.vertices.Length;
        uint x;
        _shader.GetKernelThreadGroupSizes(_kernelHandle, out x, out _, out _);
        _groupSizeX = Mathf.CeilToInt((float)_verticeCount / (float)x);
        _vertexPos = new VertexPosition[_verticeCount];
        _meshVertices = _mesh.vertices;

        for (int i = 0; i < _verticeCount; i++)
        {
            _vertexPos[i].position = _meshVertices[i];
        }

        InitShader();
    }

    private void InitShader()
    {
        _vertexBuffer = new ComputeBuffer(1, sizeof(float) * 3);
        _vertexBuffer.SetData(_vertexPos);

        _shader.SetBuffer(_kernelHandle, "vertexBuffer", _vertexBuffer);
        _shader.SetFloat("waveAmplitude", _waveAmplitude);
        _shader.SetFloat("waveWavelength", _waveWavelength);
        _shader.SetFloat("waveSteepness", _waveSteepness);
        _shader.SetFloat("waveSpeed", _waveSpeed);
        _shader.SetVector("waveDirection", _waveDirection);

    }

    private void Update()
    {
        _shader.SetFloat("time", Time.time);

        _shader.Dispatch(_kernelHandle, _groupSizeX, 1, 1);

        _vertexBuffer.GetData(_vertexPos);

        for (int i = 0; i < _vertexPos.Length; i++)
        {
            _meshVertices[i] = _vertexPos[i].position;
        }
    }

    void OnDestroy()
    {
        if (_vertexBuffer != null)
        {
            _vertexBuffer.Dispose();
        }
    }
}
