using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{

    #region Fields
    [SerializeField] private bool _useMultiThreading = true;

    private MyPlanetGenerator[] _allPlanets = null;
    #endregion

    #region Properties
    public bool UseMultiThreading
    {
        get { return _useMultiThreading; }
        set { _useMultiThreading = value; }
    }
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
        Debug.Log(value);
        Debug.Log($"_useMultiThreading: {_useMultiThreading}");

        _useMultiThreading = value;
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
        _allPlanets = FindObjectsOfType<MyPlanetGenerator>();
    }
    #endregion
}
