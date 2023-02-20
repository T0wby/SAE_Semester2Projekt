using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFlocking : MonoBehaviour
{
    public struct Boid
    {
        public Vector3 position;
        public Vector3 direction;
        
        public Boid(Vector3 pos)
        {
            position = pos;
            direction = Vector3.zero;
        }
    }

    [SerializeField] private ComputeShader _shader;

    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private float _boidSpeed = 1f;
    [SerializeField] private float _neighbourDistance = 1f;
    [SerializeField] private float _boidSpeedVariation = 1f;
    [SerializeField] private GameObject _boidPrefab;
    [SerializeField] private int _boidsCount;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private Transform _target;

    private int _kernelHandle;
    private ComputeBuffer _boidsBuffer;
    private Boid[] _boidsArray;
    private GameObject[] _boids;
    private int _groupSizeX;
    private int _numOfBoids;

    void Start()
    {
        _kernelHandle = _shader.FindKernel("CSMain");

        uint x;
        _shader.GetKernelThreadGroupSizes(_kernelHandle, out x, out _, out _);
        _groupSizeX = Mathf.CeilToInt((float)_boidsCount / (float)x);
        _numOfBoids = _groupSizeX * (int)x;

        InitBoids();
        InitShader();
    }

    private void InitBoids()
    {
        _boids = new GameObject[_numOfBoids];
        _boidsArray = new Boid[_numOfBoids];

        for (int i = 0; i < _numOfBoids; i++)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * _spawnRadius;
            _boidsArray[i] = new Boid(pos);
            _boids[i] = Instantiate(_boidPrefab, pos, Quaternion.identity);
            _boidsArray[i].direction = _boids[i].transform.forward;
        }
    }

    private void InitShader()
    {
        _boidsBuffer = new ComputeBuffer(_numOfBoids, 6 * sizeof(float));
        _boidsBuffer.SetData(_boidsArray);

        _shader.SetBuffer(_kernelHandle, "boidsBuffer", _boidsBuffer);
        _shader.SetFloat("rotationSpeed", _rotationSpeed);
        _shader.SetFloat("boidSpeed", _boidSpeed);
        _shader.SetFloat("boidSpeedVariation", _boidSpeedVariation);
        _shader.SetVector("flockPosition", _target.transform.position);
        _shader.SetFloat("neighbourDistance", _neighbourDistance);
        _shader.SetInt("boidsCount", _boidsCount);
    }

    void Update()
    {
        _shader.SetFloat("time", Time.time);
        _shader.SetFloat("deltaTime", Time.deltaTime);

        _shader.Dispatch(_kernelHandle, _groupSizeX, 1, 1);

        _boidsBuffer.GetData(_boidsArray);

        for (int i = 0; i < _boidsArray.Length; i++)
        {
            _boids[i].transform.localPosition = _boidsArray[i].position;

            if (!_boidsArray[i].direction.Equals(Vector3.zero))
            {
                _boids[i].transform.rotation = Quaternion.LookRotation(_boidsArray[i].direction);
            }

        }
    }

    void OnDestroy()
    {
        if (_boidsBuffer != null)
        {
            _boidsBuffer.Dispose();
        }
    }
}

