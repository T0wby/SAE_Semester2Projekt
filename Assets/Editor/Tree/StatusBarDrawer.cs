using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status
{
    Idle,
    Adding,
    Removing
}

public class StatusBarDrawer
{
    #region Fields
    private Status _currentStatus;
    #endregion

    #region Constructor
    public StatusBarDrawer()
    {
        _currentStatus = Status.Idle;
    }
    #endregion

    #region Methods
    public void SetCurrentStatus(Status status)
    {
        _currentStatus = status;
        DrawLabel();
    }

    public void DrawLabel(float currentWidth)
    {
        GUILayout.FlexibleSpace();
        GUILayout.Label(_currentStatus.ToString(), GUILayout.Width(currentWidth));
    }

    private void DrawLabel()
    {
        GUILayout.FlexibleSpace();
        GUILayout.Label(_currentStatus.ToString());
    } 
    #endregion
}
