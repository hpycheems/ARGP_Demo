using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewGameManager : MonoBehaviour
{
    private MainMenuUIManager mainMenuUIManager;

    public GameObject NickNameInput_Tip;
    
    public Button leftButton;
    public Button rightButton;
    public TMP_Text sex_Text;
    public TMP_InputField nicName;

    public Button closeButton;
    public Button startButton;
    
    private bool sex = true;

    private void Awake()
    {
        mainMenuUIManager = GetComponentInParent<MainMenuUIManager>();
    }

    private void Start()
    {
        leftButton.onClick.AddListener(OnClick);
        rightButton.onClick.AddListener(OnClick);
        closeButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.StopBackgroundMusic();
            AudioManager.Instance.OnButtonClickAudio();
            mainMenuUIManager.CloseNewGamePanel();
        });
        startButton.onClick.AddListener(() =>
        {
            if (!CheckNickName()) return;
            //把音乐停掉
            AudioManager.Instance.StopBackgroundMusic();
            //加载场景
            SceneLoadingManager.Instance.ASyncLoadingScene(SceneType.NewGameInitScene);
        });
    }
    
    void OnClick()
    {
        
        AudioManager.Instance.OnButtonClickAudio();
        sex = !sex;
        if (sex)
        {
            GameManager.Instance.ChangeDisplayModel(true);
            sex_Text.text = "男";
            GameManager.Instance.playerSex = true;
        }
        else
        {
            GameManager.Instance.ChangeDisplayModel(false);
            sex_Text.text = "女";
            GameManager.Instance.playerSex = false;
        }
    }
    
    //名称输入检测
    bool CheckNickName()
    {
        if (string.IsNullOrEmpty(nicName.text))
        {
            NickNameInput_Tip.SetActive(true);
            return false;
        }

        return true;
    }
}
