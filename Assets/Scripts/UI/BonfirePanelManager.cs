using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonfirePanelManager : MonoBehaviour
{
    private UIManager uiManager;
    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        Button[] buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            AddButtonListener(buttons[i]);
        }
    }

    void AddButtonListener(Button button)
    {
        if (button.name == "Exit_Button")
        {
            button.onClick.AddListener(() =>
            {
                //关闭窗口
                uiManager.CloseBonFirePanel();
                //让输入系统生效
                PlayerManager playerManager = FindObjectOfType<PlayerManager>();
                playerManager.inputHandler.EnableInput();
                //播放起身动画
                playerManager.playerAnimatorManager.PlayTargetAnimation("Stand", true);
            });
        }
        else if(button.name == "LevelUp_Button")
        {
            button.onClick.AddListener(() =>
            {
                
            });
        }
    }
}
