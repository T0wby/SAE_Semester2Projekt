using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ColourSettings", menuName = "Planet/ColourSettings")]
public class ColourSettings : ScriptableObject
{
    public Color PlanetColour;
    public Material PlanetMaterial;
}
