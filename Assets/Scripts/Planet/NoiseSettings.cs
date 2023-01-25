using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum EFilterType
    {
        Simple,
        Rigid
    }
    public EFilterType FilterType;

    public SimpleNoiseSettings simpleNoiseSettings;
    public RigidNoiseSettings rigidNoiseSettings;


    [System.Serializable]
    public class SimpleNoiseSettings
    {
        public float Strength = 1f;
        [Range(1, 8)] public int LayerCount;
        public float BaseRoughness = 1f;
        // The higher the roughness the greater the frequency will change for each Layer.
        public float Roughness = 2f;
        // Amplitude will be multiplied by Persistence with each layer to stop the initial form from changing to much.
        public float Persistence = 0.5f;
        public Vector3 NoiseCenter;
        public float GroundLevel;
    }

    [System.Serializable]
    public class RigidNoiseSettings : SimpleNoiseSettings
    {
        public float WeightMultiplier = 0.8f;
    }



}
