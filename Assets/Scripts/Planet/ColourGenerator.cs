using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGenerator
{
    private ColourSettings _colourSettings;
	private Texture2D _texture;
	private const int _textureResolution = 50;

	public void UpdateSettings(ColourSettings colourSettings)
	{
		_colourSettings = colourSettings;
		if(_texture == null)
			_texture = new Texture2D(_textureResolution, 1);
    }

	/// <summary>
	/// Updates the Elevation Values for our Shader
	/// </summary>
	/// <param name="elevationMinMax"></param>
	public void UpdateElevation(MinMax elevationMinMax)
	{
		_colourSettings.PlanetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

	/// <summary>
	/// Updates our Texture depending on the Gradient
	/// </summary>
	public void UpdateColours()
	{
		Color[] colours = new Color[_textureResolution];

		for (int i = 0; i < _textureResolution; i++)
		{
			colours[i] = _colourSettings.gradient.Evaluate(i / (_textureResolution - 1f));
        }

		_texture.SetPixels(colours);
		_texture.Apply();
		_colourSettings.PlanetMaterial.SetTexture("_texture", _texture);
	}
}
