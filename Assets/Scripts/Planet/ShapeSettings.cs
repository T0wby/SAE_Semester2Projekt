using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ShapeSettings", menuName = "Planet/ShapeSettings")]
public class ShapeSettings : ScriptableObject
{
    public float PlanetRadius = 1f;
    public bool UseFancySphere;

    public SNoiseLayer[] NoiseLayers;

    [System.Serializable]
    public struct SNoiseLayer
    {
        public bool Enabled;
        public bool UseFirstLayerAsMask;

        public NoiseSettings NoiseSettings;
    }
}
