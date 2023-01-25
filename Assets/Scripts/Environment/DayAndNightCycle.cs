using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNightCycle : MonoBehaviour
{
    [SerializeField] private SceneSettings _sceneSettings;
    [SerializeField] private Transform _lightTransform;
    [SerializeField] private GameObject[] _lights;

    private float _lightRefreshRate;
    private float _rotationAngleStep;
    private Vector3 _rotationAxis;


    private void Awake()
    {
        if (_sceneSettings.EnableDayAndNightCycle)
        {
            _lightTransform.rotation = Quaternion.Euler(_sceneSettings.DayInitialRatio * 360f, -30f, 0f);
            _lightRefreshRate = _sceneSettings.LightRefreshRate;
            _rotationAxis = _lightTransform.right;
            _rotationAngleStep = 360f * _lightRefreshRate / _sceneSettings.DayLengthInSeconds;
        }
        else
        {
            for (int i = 0; i < _lights.Length; i++)
            {
                _lights[i].SetActive(false);
            }
        }
    }

    private void Start()
    {
        if (_sceneSettings.EnableDayAndNightCycle)
            StartCoroutine(UpdateLight());
    }


    private IEnumerator UpdateLight()
    {
        while (true)
        {
            _lightTransform.Rotate(_rotationAxis, _rotationAngleStep, Space.World);
            yield return new WaitForSeconds(_lightRefreshRate);
        }
    }
}
