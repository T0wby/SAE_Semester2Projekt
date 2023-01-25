using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ColourSettings", menuName = "Planet/ColourSettings")]
public class ColourSettings : ScriptableObject
{
    public Gradient gradient;
    public Material PlanetMaterial;
}
