using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject _menu;

    protected override void Awake()
    {
        IsInAllScenes = true;
        base.Awake();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
    }

    public void OpenCloseMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _menu.SetActive(!_menu.activeSelf);

            if (_menu.activeSelf)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
