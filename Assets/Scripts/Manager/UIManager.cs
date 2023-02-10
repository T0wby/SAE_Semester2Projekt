using Player_Towby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject _menu;
    private PlayerController _playerController;

    public bool IsInMenu { get; set; }

    protected override void Awake()
    {
        IsInAllScenes = true;
        base.Awake();
        _playerController = FindObjectOfType<PlayerController>();
    }

    public void PauseGame()
    {
        IsInMenu = true;
        Cursor.visible = true;
        Time.timeScale = 0f; 
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        IsInMenu = false;
        Cursor.visible = false;
        _playerController.EnableMovement();
    }

    public void OpenCloseMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _menu.SetActive(!_menu.activeSelf);

            if (_menu.activeSelf)
            {
                PauseGame();
                _playerController.DisableMovement();
            }
            else
            {
                ResumeGame();
                _playerController.EnableMovement();
            }
                
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
