using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ColourSettings", menuName = "Planet/ColourSettings")]
public class ColourSettings : ScriptableObject
{
    public Material PlanetMaterial;
    public BiomeColourSettings biomeColourSettings;

    [System.Serializable]
    public class BiomeColourSettings
    {
        public Biome[] Biomes;
        public NoiseSettings Noise;
        public float NoiseOffset;
        public float NoiseStrength;
        
        [System.Serializable]
        public class Biome 
        {
            public Gradient gradient;
            public Color Tint;
            [Range(0, 1)] public float StartHeight;
            [Range(0, 1)] public float TintPercent;
        }
    }
}
