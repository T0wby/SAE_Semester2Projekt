using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGenerator
{
	#region Fields
	private ColourSettings _colourSettings;
	private Texture2D _texture;
	private const int _textureResolution = 50;
	private int _biomeCount;
	private INoiseFilter _biomeNoiseFilter;
	#endregion

	#region Methods
	
	/// <summary>
	/// Set settings, create new texture and filter if needed
	/// </summary>
	/// <param name="colourSettings">Coloursettings of the planet</param>
	public void UpdateSettings(ColourSettings colourSettings)
	{
		_colourSettings = colourSettings;
		_biomeCount = _colourSettings.biomeColourSettings.Biomes.Length;

		if (_texture == null || _texture.height != _biomeCount)
			_texture = new Texture2D(_textureResolution, _biomeCount);

		_biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(_colourSettings.biomeColourSettings.Noise);
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
	/// First Biome returns 0, last Biome 1
	/// </summary>
	/// <param name="point">point to calculate from</param>
	/// <returns>Returns value between 0 and 1</returns>
	public float BiomePercentageFromPoint(Vector3 point)
	{
		float heightPercent = (point.y + 1f) * 0.5f;

		// Using Noise to blurr otherwise obviouse lines between each biome
		heightPercent += (_biomeNoiseFilter.Evaluate(point) - _colourSettings.biomeColourSettings.NoiseOffset) * _colourSettings.biomeColourSettings.NoiseStrength;

		float biomeIndex = 0;

		for (int i = 0; i < _biomeCount; i++)
		{
			if (_colourSettings.biomeColourSettings.Biomes[i].StartHeight < heightPercent)
			{
				biomeIndex = i;
			}
			else
			{
				break;
			}
		}

		// Prevent 0 division
		return biomeIndex / Mathf.Max(1, _biomeCount - 1);
	}

	/// <summary>
	/// Updates our Texture depending on the Gradient
	/// </summary>
	public void UpdateColours()
	{
		Color[] colours = new Color[_texture.width * _texture.height];

		int idx = 0;
		foreach (var biome in _colourSettings.biomeColourSettings.Biomes)
		{
			for (int i = 0; i < _textureResolution; i++)
			{
				Color gradientColor = biome.gradient.Evaluate(i / (_textureResolution - 1f));
				Color tintColor = biome.Tint;
				colours[idx] = gradientColor * (1 - biome.TintPercent) + tintColor * biome.TintPercent;
				idx++;
			}
		}

		_texture.SetPixels(colours);
		_texture.Apply();
		_colourSettings.PlanetMaterial.SetTexture("_texture", _texture);
	} 
	#endregion
}
