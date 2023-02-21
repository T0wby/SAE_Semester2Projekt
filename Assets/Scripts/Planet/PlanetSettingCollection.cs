using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSettingCollection : MonoBehaviour
{
    [SerializeField] private ShapeSettings[] _shapeSettings;
    [SerializeField] private List<ColourSettings> _colourSettings;

    public ShapeSettings GetRandomShapeSetting()
    {
        return _shapeSettings[Random.Range(0, _shapeSettings.Length)];
    }

    public ColourSettings GetRandomColourSetting()
    {
        if (_colourSettings.Count == 0)
            return null;
        int random = Random.Range(0, _colourSettings.Count);
        ColourSettings chosen = _colourSettings[random];
        _colourSettings.RemoveAt(random);
        return chosen;
    }
}
