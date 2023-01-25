using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    private Noise _noise = new Noise();
    private NoiseSettings.SimpleNoiseSettings _settings;


    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
    {
        this._settings = settings;
    }


    /// <summary>
    /// Generates value, typically in range [0, 1]
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = _settings.BaseRoughness;
        float amplitude = 1f;

        for (int i = 0; i < _settings.LayerCount; i++)
        {
            float v = _noise.Evaluate(point * frequency + _settings.NoiseCenter);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= _settings.Roughness;
            amplitude *= _settings.Persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - _settings.GroundLevel);
        return noiseValue * _settings.Strength;
    }
}
