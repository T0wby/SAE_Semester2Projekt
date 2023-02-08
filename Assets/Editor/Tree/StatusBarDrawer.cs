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
    private Status _currentStatus;

    public StatusBarDrawer()
    {
        _currentStatus = Status.Idle;
    }

    public void SetCurrentStatus(Status status)
    {
        _currentStatus = status;
        DrawLabel();
    }

    public void DrawLabel(float currentWidth)
    {
        GUILayout.FlexibleSpace();
        GUILayout.Label(_currentStatus.ToString(),GUILayout.Width(currentWidth));
    }

    private void DrawLabel()
    {
        GUILayout.FlexibleSpace();
        GUILayout.Label(_currentStatus.ToString());
    }
}
