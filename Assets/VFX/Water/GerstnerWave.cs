using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GerstnerWave : MonoBehaviour
{
    #region Fields
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
    #endregion

    private void Start()
    {
        _kernelHandle = _shader.FindKernel("CSMain");
        _mesh = GetComponent<MeshFilter>().sharedMesh;
        _verticeCount = _mesh.vertices.Length;
        uint x;
        _shader.GetKernelThreadGroupSizes(_kernelHandle, out x, out _, out _);
        _groupSizeX = Mathf.CeilToInt((float)_verticeCount / (float)x);
        _meshVertices = _mesh.vertices;

        InitShader();
    }

    /// <summary>
    /// Initialize all shader properties and the buffer
    /// </summary>
    private void InitShader()
    {
        _vertexBuffer = new ComputeBuffer(_verticeCount, sizeof(float) * 3);
        _vertexBuffer.SetData(_meshVertices);

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

        _vertexBuffer.GetData(_meshVertices);

        _mesh.vertices = _meshVertices;
    }

    void OnDestroy()
    {
        if (_vertexBuffer != null)
        {
            _vertexBuffer.Dispose();
        }
    }
}
