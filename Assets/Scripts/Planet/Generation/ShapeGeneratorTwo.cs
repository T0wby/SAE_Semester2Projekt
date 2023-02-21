using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ShapeGeneratorTwo
{
    #region Fields
    private ShapeSettings _currentSettings;
    private INoiseFilter[] _noiseFilters;
    private MinMax _elevationMinMax;
    #endregion

    #region Properties
    public MinMax ElevationMinMax { get => _elevationMinMax; }
    #endregion

    /// <summary>
    /// Initializes all shape settings
    /// </summary>
    /// <param name="newSettings">ShapeSettings from the planet</param>
    public void UpdateSettings(ShapeSettings newSettings)
    {
        _currentSettings = newSettings;
        _noiseFilters = new INoiseFilter[_currentSettings.NoiseLayers.Length];
        for (int i = 0; i < _noiseFilters.Length; i++)
        {
            _noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(newSettings.NoiseLayers[i].NoiseSettings);
        }
        _elevationMinMax = new MinMax();
    }

    /// <summary>
    /// Transforms each vertice position from the original cube to one for a sphere
    /// </summary>
    /// <param name="_cubeVertPos">Vertice position</param>
    /// <param name="_useFancySphere">Use FancySphere formula</param>
    /// <returns>Returns new Position</returns>
    public Vector3 TransformCubeToSpherePos(Vector3 _cubeVertPos, bool _useFancySphere)
    {
        if (!_useFancySphere)
            return _cubeVertPos.normalized;

        float x2 = _cubeVertPos.x * _cubeVertPos.x;
        float y2 = _cubeVertPos.y * _cubeVertPos.y;
        float z2 = _cubeVertPos.z * _cubeVertPos.z;

        float xPrime = _cubeVertPos.x * Mathf.Sqrt(1 - (y2 + z2) / 2 + (y2 * z2) / 3);
        float yPrime = _cubeVertPos.y * Mathf.Sqrt(1 - (x2 + z2) / 2 + (x2 * z2) / 3);
        float zPrime = _cubeVertPos.z * Mathf.Sqrt(1 - (x2 + y2) / 2 + (x2 * y2) / 3);

        return new Vector3(xPrime, yPrime, zPrime);
    }

    /// <summary>
    /// Calculate point position depending on Noises and Planet radius
    /// </summary>
    /// <param name="_pointOnUnitSphere">Point to calculate</param>
    /// <returns>New Position for the point</returns>
    public Vector3 CalculatePointOnPlanet(Vector3 _pointOnUnitSphere)
    {
        // Value later added to the _pointOnUnitSphere to create higher parts in the mesh
        float elevation = 0f;
        float firstLayerElevation = 0f;

        if (_noiseFilters.Length > 0)
        {
            firstLayerElevation = _noiseFilters[0].Evaluate(_pointOnUnitSphere);

            if (_currentSettings.NoiseLayers[0].Enabled)
                elevation = firstLayerElevation;
        }

        //Start at idx 1 because first layer is already evaluated
        float mask = 0f;
        for (int i = 1; i < _noiseFilters.Length; i++)
        {
            if (_currentSettings.NoiseLayers[i].Enabled)
            {
                mask = _currentSettings.NoiseLayers[i].UseFirstLayerAsMask ? firstLayerElevation : 1f;

                elevation += _noiseFilters[i].Evaluate(_pointOnUnitSphere) * mask;
            }
        }

        elevation = _currentSettings.PlanetRadius * (1 + elevation);
        _elevationMinMax.AddValue(elevation);

        return _pointOnUnitSphere * elevation;
    }
}
