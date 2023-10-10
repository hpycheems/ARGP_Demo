using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏开始主界面 用于加载游戏、开启新游戏等按钮的初始化
/// </summary>
public class MainMenuOperationManager : MonoBehaviour
{
    [Header("Button-Name")]
    public string quitButtonName;
    public string startNewGameButtonName;
    public string continueButtonName;
    public string settingsButtonName;
    public string loadGameButtonName;

    private MainMenuUIManager mainMenuUIManager;
    private void Awake()
    {
        mainMenuUIManager = GetComponentInParent<MainMenuUIManager>();
        Button[] buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            AddButtonListen(buttons[i]);
        }
    }

    void AddButtonListen(Button button)
    {
        if (button.name == quitButtonName)
        {
            button.onClick.AddListener(() =>
            {
                AudioManager.Instance.OnButtonClickAudio();
                GameManager.Instance.QuitGame();
            });
        }
        else if (button.name == settingsButtonName)
        {
            button.onClick.AddListener(() =>
            {
                AudioManager.Instance.OnButtonClickAudio();
                mainMenuUIManager.OpenSettingsPanel();
            });
        }
        else if (button.name == startNewGameButtonName)
        {
            button.onClick.AddListener(() =>
            {
                AudioManager.Instance.OnButtonClickAudio();
                StartCoroutine(GameManager.Instance.MaskFade(0, 1, () =>
                {
                    mainMenuUIManager.OpenNewGamePanel();
                }));
            });
        }
        else if (button.name == loadGameButtonName)
        {
            button.onClick.AddListener(() =>
            {
                AudioManager.Instance.OnButtonClickAudio();
                StartCoroutine(GameManager.Instance.MaskFade(0, 1, () =>
                {
                    mainMenuUIManager.OpenFilePanel();
                }));
            });
        }
        else if (button.name == continueButtonName)
        {
            button.onClick.AddListener(() =>
            {
                if(GameManager.Instance.haveContinueData)
                    GameManager.Instance.LoadContinueGameData();
            });
        }
    }
}
