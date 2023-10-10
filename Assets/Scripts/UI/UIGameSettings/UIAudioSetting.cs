using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMusicVolumeSetting : UIEffectBase
{
    private UISettingsManager uiSettingsManager;
    
    public Slider volumeSlider;
    private void Awake()
    {
        uiSettingsManager = FindObjectOfType<UISettingsManager>();
            
        volumeSlider = GetComponentInChildren<Slider>();
        volumeSlider.onValueChanged.AddListener(OnValueChange);
    }

    private void OnValueChange(float arg0)
    {
        int volume = Mathf.RoundToInt(arg0 * 100);
        if (name == "SFXVolume")
        {
            uiSettingsManager.SetSFXVolume(volume);
        }
        else if (name == "MusicVolume")
        {
            uiSettingsManager.SetMusicVolume(volume);
        }
    }
}
