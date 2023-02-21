using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : INoiseFilter
{
    #region Fields
    private Noise _noise = new Noise();
    private NoiseSettings.RigidNoiseSettings _settings;
    #endregion


    #region Constructor
    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings)
    {
        this._settings = settings;
    } 
    #endregion


    /// <summary>
    /// Generates value, typically in range [0, 1] and is inversed
    /// </summary>
    /// <param name="point">point to calculate noise value for</param>
    /// <returns>Value of the noise</returns>
    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = _settings.BaseRoughness;
        float amplitude = 1f;
        // Later Layers will have a higher weight and provide more details for this Filter
        float weight = 1f;

        // Calculate values for each Layer
        for (int i = 0; i < _settings.LayerCount; i++)
        {
            float v = 1 - Mathf.Abs(_noise.Evaluate(point * frequency + _settings.NoiseCenter));
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * _settings.WeightMultiplier);
            noiseValue += v * amplitude;
            frequency *= _settings.Roughness;
            amplitude *= _settings.Persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - _settings.GroundLevel);
        return noiseValue * _settings.Strength;
    }
}
