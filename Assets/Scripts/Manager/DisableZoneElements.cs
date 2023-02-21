using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisableZoneElements : MonoBehaviour
{
    private HideAI[] _hideAIGroup;
    private SimpleFlocking[] _flockinGroup;
    private GerstnerWave[] _gerstnerWave;
    private SnowAI[] _snowAIGroup;

    // Start is called before the first frame update
    void Start()
    {
        _hideAIGroup = FindObjectsOfType<HideAI>();
        _flockinGroup = FindObjectsOfType<SimpleFlocking>();
        _snowAIGroup = FindObjectsOfType<SnowAI>();
        _gerstnerWave = FindObjectsOfType<GerstnerWave>();
        ChangeHideAIStatus();
        ChangeSimpleFlockingStatus();
        ChangeSnowAIStatus();
    }

    public void ChangeHideAIStatus()
    {
        foreach (HideAI hideAi in _hideAIGroup)
        {
            hideAi.gameObject.SetActive(!hideAi.gameObject.activeSelf);
        }
    }
    public void ChangeSimpleFlockingStatus()
    {
        foreach (SimpleFlocking flock in _flockinGroup)
        {
            flock.gameObject.SetActive(!flock.gameObject.activeSelf);
        }

        foreach (GerstnerWave item in _gerstnerWave)
        {
            item.gameObject.SetActive(!item.gameObject.activeSelf);
        }
    }
    public void ChangeSnowAIStatus()
    {
        foreach (SnowAI ai in _snowAIGroup)
        {
            ai.gameObject.SetActive(!ai.gameObject.activeSelf);
        }
    }
}
