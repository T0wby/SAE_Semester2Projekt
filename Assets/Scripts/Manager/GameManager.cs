using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{

    #region Fields

    private PlanetGenerator[] _allPlanets = null;
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

    private void GetAllPlanets()
    {
        _allPlanets = FindObjectsOfType<PlanetGenerator>();
    }
    #endregion
}
