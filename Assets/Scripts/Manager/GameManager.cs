using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : Singleton<GameManager>
{

    #region Fields
    [SerializeField] private GameObject _sandstorm;
    [SerializeField] private VolumeProfile _sandV;
    private PlanetGenerator[] _allPlanets = null;
    private List<PlanetGenerator> _noneGeneratedPlanets = null;
    private UnityEngine.Rendering.Universal.Vignette _vignette;
    #endregion


    #region Unity
    protected override void Awake()
    {
        IsInAllScenes = true;
        base.Awake();
        GetAllPlanets();
    }
    #endregion

    #region Methods
    public void ChangeMultiThreading(bool value)
    {
        for (int i = 0; i < _allPlanets.Length; i++)
        {
            _allPlanets[i].UseMultiThreading = value;
        }
    } 

    public void GenerateRandomPlanet(InputAction.CallbackContext context)
    {
        if (context.started && _noneGeneratedPlanets.Count > 0)
        {
            int random = Random.Range(0, _noneGeneratedPlanets.Count - 1);

            _noneGeneratedPlanets[random].GeneratePlanet();
            _noneGeneratedPlanets.RemoveAt(random);
        }
    }
    public void ToggleSandstorm(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _sandstorm.SetActive(!_sandstorm.activeSelf);
            if (!_sandV.TryGet(out _vignette))
                return;

            if (_sandstorm.activeSelf)
                _vignette.intensity.Override(0.7f);
            else
                _vignette.intensity.Override(0.0f);
        }
    }

    private void GetAllPlanets()
    {
        _allPlanets = FindObjectsOfType<PlanetGenerator>();
        _noneGeneratedPlanets = _allPlanets.ToList();
    }
    #endregion
}
