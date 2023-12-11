using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class GameManager : Singleton<GameManager>
{
    #region Fields
    [SerializeField] private GameObject _sandstorm;
    [SerializeField] private VisualEffect _sandground;
    [SerializeField] private VolumeProfile _sandV;
    private PlanetGenerator[] _allPlanets = null;
    private List<PlanetGenerator> _noneGeneratedPlanets = null;
    private UnityEngine.Rendering.Universal.Vignette _vignette;
    private PlanetSettingCollection _collection;
    #endregion

    #region Unity
    protected override void Awake()
    {
        IsInAllScenes = true;
        base.Awake();
        GetAllPlanets();
        _collection = FindObjectOfType<PlanetSettingCollection>();
    }
    #endregion

    #region Methods

    /// <summary>
    /// Changes the Multithreading of all planet objects in the game
    /// </summary>
    /// <param name="value">value to change to</param>
    public void ChangeMultiThreading(bool value)
    {
        for (int i = 0; i < _allPlanets.Length; i++)
        {
            _allPlanets[i].UseMultiThreading = value;
        }
    } 

    /// <summary>
    /// Generate the meshes of one of the existing planets
    /// </summary>
    /// <param name="context"></param>
    public void GenerateRandomPlanet(InputAction.CallbackContext context)
    {
        /** Generate random planet positions, but used fixed settings **/
        if (context.started && _noneGeneratedPlanets.Count > 0)
        {
            int random = Random.Range(0, _noneGeneratedPlanets.Count - 1);

            _noneGeneratedPlanets[random].GeneratePlanet();
            _noneGeneratedPlanets.RemoveAt(random);
        }

        /** Completely randomize planets **/
        //if (context.started && _noneGeneratedPlanets.Count > 0)
        //{
        //    int random = Random.Range(0, _noneGeneratedPlanets.Count - 1);
        //    ShapeSettings shapeSettings = _collection.GetRandomShapeSetting();
        //    ColourSettings colourSettings = _collection.GetRandomColourSetting();


        //    _noneGeneratedPlanets[random].ShapeSettings = shapeSettings;
        //    _noneGeneratedPlanets[random].ColourSettings = colourSettings;
        //    _noneGeneratedPlanets[random].GeneratePlanet();
        //    _noneGeneratedPlanets.RemoveAt(random);
        //}
    }

    /// <summary>
    /// Turns the sandstorm vfx on and off
    /// </summary>
    /// <param name="context"></param>
    public void ToggleSandstorm(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _sandstorm.SetActive(!_sandstorm.activeSelf);
            if (!_sandV.TryGet(out _vignette))
                return;

            if (_sandstorm.activeSelf)
            {
                _vignette.intensity.Override(0.7f);
                _sandground.SetInt("SpawnRate", 80);
            }
            else
            {
                _vignette.intensity.Override(0.0f);
                _sandground.SetInt("SpawnRate", 0);
            }
        }
    }

    /// <summary>
    /// Get a reference to all Planets
    /// </summary>
    private void GetAllPlanets()
    {
        _allPlanets = FindObjectsOfType<PlanetGenerator>();
        _noneGeneratedPlanets = _allPlanets.ToList();
    }
    #endregion
}
