using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Material _waveMaterial;
    [SerializeField] private Texture2D _waveTexture;

    private float[][] _waveCurrent;
    private float[][] _wavePrevious;
    private float[][] _waveNext;

    private float _widthX = 10f;
    private float _heightY = 10f;

    [SerializeField] private float _densityX = 0.1f;
    [SerializeField] private float _colorMultiplier = 2f;
    [SerializeField] private float _pulseSpeed = 1f;
    [SerializeField] private float _pulseMagnitude = 1f;
    [SerializeField] private Vector2Int _pulsePosition = new Vector2Int(50,50);
    public float DensityY => _densityX;

    private int _resolutionX;
    private int _resolutionY;

    private float _CFL = 0.5f;
    private float _c = 1f;
    private float _deltaTime;
    private float _currentTime;


    void Start()
    {
        _resolutionX = Mathf.FloorToInt(_widthX / _densityX);
        _resolutionY = Mathf.FloorToInt(_heightY / DensityY);
        _waveTexture = new Texture2D(_resolutionX, _resolutionY, TextureFormat.RGBA32, false);

        // Create empty field
        _waveCurrent = new float[_resolutionX][];
        _wavePrevious = new float[_resolutionX][];
        _waveNext = new float[_resolutionX][];

        for (int i = 0; i < _resolutionX; i++)
        {
            _waveCurrent[i] = new float[_resolutionY];
            _wavePrevious[i] = new float[_resolutionY];
            _waveNext[i] = new float[_resolutionY];
        }

        _waveMaterial.SetTexture("_MainTex", _waveTexture);
        _waveMaterial.SetTexture("_Displacement", _waveTexture);
    }

    void Update()
    {
        WaveForming();
        ApplyMatrixToTexture(_waveCurrent, ref _waveTexture, _colorMultiplier);
    }

    private void WaveForming()
    {
        _deltaTime = _CFL * _densityX / _c;
        _currentTime += _densityX;

        for (int i = 0; i < _resolutionX; i++)
        {
            for (int j = 0; j < _resolutionY; j++)
            {
                _wavePrevious[i][j] = _waveCurrent[i][j];
                _waveCurrent[i][j] = _wavePrevious[i][j];
            }
        }


        _waveCurrent[_pulsePosition.x][_pulsePosition.y] = _deltaTime* _deltaTime*20 * _pulseMagnitude * Mathf.Sin(_currentTime * Mathf.Rad2Deg * _pulseSpeed);



        for (int i = 1; i < _resolutionX - 1; i++)// Ignore edges
        {
            for (int j = 1; j < _resolutionY - 1; j++)
            {
                float curr_ij = _waveCurrent[i][j];
                float curr_ip1j = _waveCurrent[i+1][j];
                float curr_im1j = _waveCurrent[i-1][j];
                float curr_ijp1 = _waveCurrent[i][j+1];
                float curr_ijm1 = _waveCurrent[i][j-1];

                float prev_ij = _wavePrevious[i][j];
                //float prev_ip1j = _wavePrevious[i + 1][j];
                //float prev_im1j = _wavePrevious[i - 1][j];
                //float prev_ijp1 = _wavePrevious[i][j + 1];
                //float prev_ijm1 = _wavePrevious[i][j - 1];

                _waveNext[i][j] = 2f * curr_ij - prev_ij + _CFL * _CFL * (curr_ijm1 + curr_ijp1 + curr_im1j + curr_ip1j - 4f * curr_ij);

            }
        }
    }

    private void ApplyMatrixToTexture(float[][] state, ref Texture2D texture, float multiplier)
    {
        for (int i = 0; i < _resolutionX; i++)
        {
            for (int j = 0; j < _resolutionY; j++)
            {
                float value = state[i][j] * multiplier;
                texture.SetPixel(i, j, new Color(value + 0.5f, value + 0.5f, value + 0.5f, 1f));
            }
        }
        texture.Apply();
    }
}
