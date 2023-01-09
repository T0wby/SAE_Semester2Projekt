using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    [SerializeField] protected static bool _isInAllScenes;

    public static T Instance
    {
        get
        {
            if (_instance is null) Initialize();
            else
            {
                var name = typeof(T).Name;
                Debug.LogWarning($"Another instance of {name} is already running. Instance is {_instance.gameObject.name}.");
            }
            return _instance;
        }
    }

    public bool IsInAllScenes
    {
        get { return _isInAllScenes; }
        set { _isInAllScenes = value; }
    }

    private static void Initialize()
    {
        _instance = (T)FindObjectOfType(typeof(T));
        if (_instance is null)
        {
            var gameObject = new GameObject();
            gameObject.name = typeof(T).Name;
            _instance = gameObject.AddComponent<T>();
        }
    }

    private void RemoveDuplicates()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (_isInAllScenes) DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
    }

    protected virtual void Awake() => RemoveDuplicates();
}
