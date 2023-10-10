using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIResolutionEffect : UIEffectBase
{
    public GameObject notInteract;
    
    private UISettingsManager uiSettingsManager;
    private CanvasGroup canvasGroup;
    private void Awake()
    {
        uiSettingsManager = FindObjectOfType<UISettingsManager>();
        canvasGroup = GetComponent<CanvasGroup>();
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach (var button in buttons)
        {
            AddButtonListen(button);
        }
    }

    private void Update()
    {
        if (!canvasGroup.interactable)
        {
            notInteract.SetActive(true);
        }
        else
        {
            notInteract.SetActive(false);
        }
    }

    protected override void AddButtonListen(Button button)
    {
        if (button.name == "Right_Button")
        {
            button.onClick.AddListener(OnRightClick);
        }
        else if (button.name == "Left_Button")
        {
            button.onClick.AddListener(OnLeftClick);
        }
    }
    void OnLeftClick()
    {
        AudioManager.Instance.OnButtonClickAudio();
        int resolutionIndex = uiSettingsManager.data.resolutionIndex;
        GameManager manager = GameManager.Instance;
        resolutionIndex = (resolutionIndex + manager.resolutions.Length - 1) % manager.resolutions.Length;
        uiSettingsManager.SetResolution(resolutionIndex);
    }
    void OnRightClick()
    {
        AudioManager.Instance.OnButtonClickAudio();
        int resolutionIndex = uiSettingsManager.data.resolutionIndex;
        GameManager manager = GameManager.Instance;
        resolutionIndex = (resolutionIndex + 1) % manager.resolutions.Length;
        uiSettingsManager.SetResolution(resolutionIndex);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        AudioClip clip = AudioManager.Instance.onButtonEnterClip;
        AudioManager.Instance.PlayAudio(clip, true, null);
        if (canvasGroup.interactable)
        {
            effectObject.SetActive(true);
        }   
    }
}
