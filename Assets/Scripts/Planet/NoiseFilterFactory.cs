using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseFilterFactory
{
    public static INoiseFilter CreateNoiseFilter(NoiseSettings noiseSettings)
    {
        switch (noiseSettings.FilterType)
        {
            case NoiseSettings.EFilterType.Simple:
                return new SimpleNoiseFilter(noiseSettings.simpleNoiseSettings);
            case NoiseSettings.EFilterType.Rigid:
                return new RigidNoiseFilter(noiseSettings.rigidNoiseSettings);
            default:
                return null;
        }
    }
}
