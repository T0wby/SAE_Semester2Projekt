using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneSettings", menuName = "Settings/SceneSettings")]
public class SceneSettings : ScriptableObject
{
    public bool EnableDayAndNightCycle;
    public float DayLengthInSeconds;
    public float DayInitialRatio;
    public float LightRefreshRate;
}
