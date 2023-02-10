using System.Collections;
using System.Collections.Generic;
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

    public void GenerateAllPlanets(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            for (int i = 0; i < _allPlanets.Length; i++)
            {
                _allPlanets[i].GeneratePlanet();
            }
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
    }
    #endregion
}
