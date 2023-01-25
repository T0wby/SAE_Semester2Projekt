using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ShapeGenerator
{
    //private ShapeSettings _currentSettings;
    //private SimpleNoiseFilter[] _noiseFilters;
    //private MinMax _elevationMinMax;

    //Vector3 _position;
    //Vector3 _rotation;
    //Vector3 _scale;

    //public MinMax ElevationMinMax { get => _elevationMinMax; }

    //public void UpdateShapeSettings(ShapeSettings newSettings, Vector3 position, Vector3 rotation, Vector3 scale)
    //{
    //    _currentSettings = newSettings;

    //    this._position = position;
    //    this._rotation = rotation;
    //    this._scale = scale;

    //    _noiseFilters = new SimpleNoiseFilter[newSettings.NoiseLayers.Length];
    //    for (int i = 0; i < _noiseFilters.Length; i++)
    //    {
    //        _noiseFilters[i] = new SimpleNoiseFilter(newSettings.NoiseLayers[i].NoiseSettings);
    //    }
    //    _elevationMinMax = new MinMax();
    //}

    //public Vector3 TransformCubeToSpherePos(Vector3 _cubeVertPos, bool _useFancySphere)
    //{
    //    if (!_useFancySphere)
    //        return _cubeVertPos.normalized;

    //    float x2 = _cubeVertPos.x * _cubeVertPos.x;
    //    float y2 = _cubeVertPos.y * _cubeVertPos.y;
    //    float z2 = _cubeVertPos.z * _cubeVertPos.z;

    //    float xPrime = _cubeVertPos.x * Mathf.Sqrt(1 - (y2 + z2) / 2 + (y2 * z2) / 3);
    //    float yPrime = _cubeVertPos.y * Mathf.Sqrt(1 - (x2 + z2) / 2 + (x2 * z2) / 3);
    //    float zPrime = _cubeVertPos.z * Mathf.Sqrt(1 - (x2 + y2) / 2 + (x2 * y2) / 3);

    //    return new Vector3(xPrime, yPrime, zPrime);
    //}

    //public Vector3 CalculatePointOnPlanet(Vector3 _spherePos)
    //{
    //    Vector3 planetPos = _spherePos;

    //    float elevation = 0f;
    //    float firstLayerElevation = 0f;

    //    if (_noiseFilters.Length > 0)
    //    {
    //        firstLayerElevation = _noiseFilters[0].Evaluate(_spherePos);

    //        if (_currentSettings.NoiseLayers[0].Enabled)
    //            elevation = firstLayerElevation;
    //    }

    //    //Start at idx 1 because first layer is already evaluated
    //    float mask = 0f;
    //    for (int i = 1; i < _noiseFilters.Length; i++)
    //    {
    //        if (_currentSettings.NoiseLayers[i].Enabled)
    //        {
    //            mask = _currentSettings.NoiseLayers[i].UseFirstLayerAsMask ? firstLayerElevation : 1f;

    //            elevation += _noiseFilters[i].Evaluate(_spherePos) * mask;
    //        }
    //    }

    //    elevation = _currentSettings.PlanetRadius * (1 + elevation);
    //    _elevationMinMax.AddValue(elevation);

    //    return _spherePos * elevation;
    //}

    //public Vector3 TransformPointWithOwnTransformMatrix(Vector3 _basePos)
    //{
    //    Matrix4x4 transformMatrix = GetCurrTransformMatrix();

    //    Vector4 basePos4 = new Vector4(_basePos.x, _basePos.y, _basePos.z, 1);
    //    basePos4 = transformMatrix * basePos4;

    //    return new Vector3(basePos4.x, basePos4.y, basePos4.z);
    //}

    //private Matrix4x4 GetCurrTransformMatrix()
    //{
    //    #region Broken Threading
    //    //float xRotRad = rotation.x * Mathf.Deg2Rad;
    //    //Matrix4x4 rotMatX = new Matrix4x4();
    //    //Thread tOne = new Thread(() =>
    //    //{
    //    //    rotMatX.SetRow(0, new Vector4(1, 0, 0, 0));
    //    //    rotMatX.SetRow(1, new Vector4(0, Mathf.Cos(xRotRad), -Mathf.Sin(xRotRad), 0));
    //    //    rotMatX.SetRow(2, new Vector4(0, Mathf.Sin(xRotRad), Mathf.Cos(xRotRad), 0));
    //    //    rotMatX.SetRow(3, new Vector4(0, 0, 0, 1));
    //    //});


    //    //float yRotRad = rotation.y * Mathf.Deg2Rad;
    //    //Matrix4x4 rotMatY = new Matrix4x4();
    //    //Thread tTwo = new Thread(() =>
    //    //{
    //    //    rotMatY.SetRow(0, new Vector4(Mathf.Cos(yRotRad), 0, Mathf.Sin(yRotRad), 0));
    //    //    rotMatY.SetRow(1, new Vector4(0, 1, 0, 0));
    //    //    rotMatY.SetRow(2, new Vector4(-Mathf.Sin(yRotRad), 0, Mathf.Cos(yRotRad), 0));
    //    //    rotMatY.SetRow(3, new Vector4(0, 0, 0, 1));
    //    //});

    //    //float zRotRad = rotation.z * Mathf.Deg2Rad;
    //    //Matrix4x4 rotMatZ = new Matrix4x4();
    //    //Thread tThree = new Thread(() =>
    //    //{
    //    //    rotMatZ.SetRow(0, new Vector4(Mathf.Cos(zRotRad), -Mathf.Sin(zRotRad), 0, 0));
    //    //    rotMatZ.SetRow(1, new Vector4(Mathf.Sin(zRotRad), Mathf.Cos(zRotRad), 0, 0));
    //    //    rotMatZ.SetRow(2, new Vector4(0, 0, 1, 0));
    //    //    rotMatZ.SetRow(3, new Vector4(0, 0, 0, 1));
    //    //});

    //    //tOne.Start();
    //    //tTwo.Start();
    //    //tThree.Start();

    //    //tOne.Join();
    //    //tTwo.Join();
    //    //tThree.Join();
    //    #endregion


    //    float xRotRad = _rotation.x * Mathf.Deg2Rad;
    //    Matrix4x4 rotMatX = new Matrix4x4();
    //    rotMatX.SetRow(0, new Vector4(1, 0, 0, 0));
    //    rotMatX.SetRow(1, new Vector4(0, Mathf.Cos(xRotRad), -Mathf.Sin(xRotRad), 0));
    //    rotMatX.SetRow(2, new Vector4(0, Mathf.Sin(xRotRad), Mathf.Cos(xRotRad), 0));
    //    rotMatX.SetRow(3, new Vector4(0, 0, 0, 1));


    //    float yRotRad = _rotation.y * Mathf.Deg2Rad;
    //    Matrix4x4 rotMatY = new Matrix4x4();
    //    rotMatY.SetRow(0, new Vector4(Mathf.Cos(yRotRad), 0, Mathf.Sin(yRotRad), 0));
    //    rotMatY.SetRow(1, new Vector4(0, 1, 0, 0));
    //    rotMatY.SetRow(2, new Vector4(-Mathf.Sin(yRotRad), 0, Mathf.Cos(yRotRad), 0));
    //    rotMatY.SetRow(3, new Vector4(0, 0, 0, 1));

    //    float zRotRad = _rotation.z * Mathf.Deg2Rad;
    //    Matrix4x4 rotMatZ = new Matrix4x4();
    //    rotMatZ.SetRow(0, new Vector4(Mathf.Cos(zRotRad), -Mathf.Sin(zRotRad), 0, 0));
    //    rotMatZ.SetRow(1, new Vector4(Mathf.Sin(zRotRad), Mathf.Cos(zRotRad), 0, 0));
    //    rotMatZ.SetRow(2, new Vector4(0, 0, 1, 0));
    //    rotMatZ.SetRow(3, new Vector4(0, 0, 0, 1));


    //    //Base Matrix mit Rotation
    //    Matrix4x4 transformMatrix = rotMatX * rotMatY * rotMatZ;

    //    //Skalierung
    //    transformMatrix.m00 *= _scale.x;
    //    transformMatrix.m11 *= _scale.y;
    //    transformMatrix.m22 *= _scale.z;

    //    //Translation
    //    transformMatrix.m03 = _position.x;
    //    transformMatrix.m13 = _position.y;
    //    transformMatrix.m23 = _position.z;

    //    return transformMatrix;
    //}
}
