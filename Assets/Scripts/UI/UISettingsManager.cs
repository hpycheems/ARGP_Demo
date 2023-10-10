using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 设置页面管理
/// </summary>
public class UISettingsManager : MonoBehaviour
{
    private MainMenuUIManager mainMenuUIManager;
    [Header("控制分辨率按钮是否可使用")]
    public CanvasGroup resolutionCanvasGroup;
    [Header("界面按钮")]
    public UISettingButton save_Button;
    public UISettingButton default_Button;
    [Header("音频滑动条")]
    public Slider musicSlider;
    public Slider sfxSlider;
    [Header("显示的文本")]
    public TMP_Text resolutionText;
    public TMP_Text fullScreenText;
    
    public SettingData data;

    private void Awake()
    {
        mainMenuUIManager = FindObjectOfType<MainMenuUIManager>();
    }
    private void Start()
    {
        default_Button.button.onClick.AddListener(OnDefaultClick);
        save_Button.button.onClick.AddListener(OnSaveClick);
    }
    
    /// <summary>
    /// 加载数据从保存的文件中加载
    /// </summary>
    public void LoadSettingDataFormGameManager()
    {
        UpdateData(GameManager.Instance.settingData);
    }
    
    /// <summary>
    /// 设置分辨率
    /// </summary>
    /// <param name="resolutionIndex"></param>
    public void SetResolution(int resolutionIndex)
    {
        switch (resolutionIndex)
        {
            case 0:
                resolutionText.text = GameManager.Instance.resolutions[0];
                data.resolutionIndex = 0;
                break;
            case 1:
                resolutionText.text = GameManager.Instance.resolutions[1];
                data.resolutionIndex = 1;
                break;
            case 2:
                resolutionText.text = GameManager.Instance.resolutions[2];
                data.resolutionIndex = 2;
                break;
        }
    }
    /// <summary>
    /// 设置是否使用全屏
    /// </summary>
    public void SetScreen()
    {
        if (data.isFullScreen)
        {
            fullScreenText.text = "全屏";
        }
        else
        {
            fullScreenText.text = "窗口化";
        }

        if (data.isFullScreen)
        {
            SetResolution(2);
        }
        resolutionCanvasGroup.interactable = !data.isFullScreen;
    }
    /// <summary>
    /// 设置音乐
    /// </summary>
    /// <param name="volume"></param>
    public void SetMusicVolume(int volume)
    {
        float value = volume / 100.0f;
        musicSlider.value = value;

        AudioManager.Instance.musicVolume = volume;
        
        data.musicVolume = volume;
    }
    /// <summary>
    /// 设置音效
    /// </summary>
    /// <param name="volume"></param>
    public void SetSFXVolume(int volume)
    {
        float value = volume / 100.0f;
        sfxSlider.value = value;

        AudioManager.Instance.sfxVolume = volume;
        
        data.sfxVolume = volume;
    }
    
    /// <summary>
    /// 保存当前设置的数据
    /// </summary>
    void OnSaveClick()
    {
        AudioManager.Instance.OnButtonClickAudio();
        save_Button.effectObject.SetActive(false);
        
        GameManager.Instance.UpdateSettingData(data);
        mainMenuUIManager.CloseSettingsPanel();
    }
    /// <summary>
    /// 设置成默认值
    /// </summary>
    void OnDefaultClick()
    {
        AudioManager.Instance.OnButtonClickAudio();
        default_Button.effectObject.SetActive(false);
        
        SettingData data = new SettingData();
        data.resolutionIndex = SystemDefine.defaultResolutionIndex;
        data.musicVolume = SystemDefine.defaultmusicVolume;
        data.sfxVolume = SystemDefine.defaultsfxVolume;
        data.isFullScreen = SystemDefine.defaultIsFullScreen;
        UpdateData(data);
    }
    
    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    private void UpdateData(SettingData data)
    {
        this.data.resolutionIndex = data.resolutionIndex;
        this.data.musicVolume = data.musicVolume;
        this.data.sfxVolume = data.sfxVolume;
        this.data.isFullScreen = data.isFullScreen;
        DisplayUI(data);
    }
    /// <summary>
    /// 通过数据更新UI
    /// </summary>
    /// <param name="data"></param>
    private void DisplayUI(SettingData data)
    {
        SetResolution(data.resolutionIndex);
        SetScreen();
        SetMusicVolume(data.musicVolume);
        SetSFXVolume(data.sfxVolume);
    }
}
