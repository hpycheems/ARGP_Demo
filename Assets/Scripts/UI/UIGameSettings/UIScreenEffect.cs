using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScreenEffect : UIEffectBase
{
    private UISettingsManager uiSettingsManager;

    private void Awake()
    {
        uiSettingsManager = FindObjectOfType<UISettingsManager>();
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            AddButtonListen(button);
        }
    }
    protected override void AddButtonListen(Button button)
    {
        if (button.name == "Right_Button")
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else if (button.name == "Left_Button")
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }
    void OnButtonClick()
    {
        AudioManager.Instance.OnButtonClickAudio();
        uiSettingsManager.data.isFullScreen = !uiSettingsManager.data.isFullScreen;
        uiSettingsManager.SetScreen();
    }
}
