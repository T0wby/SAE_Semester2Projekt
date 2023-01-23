using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGenerator
{
    private ColourSettings _colourSettings;

	public ColourGenerator(ColourSettings colourSettings)
	{
		_colourSettings = colourSettings;
	}

	public void UpdateElevation(MinMax elevationMinMax)
	{
		_colourSettings.PlanetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));

    }
}
