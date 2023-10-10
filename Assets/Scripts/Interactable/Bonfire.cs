using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : Interactable
{
    private UIManager uiManager;
    public GameObject effect;
    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
        
        GameManager.Instance.GamingSaveFile();
        
        //开启效果
        effect.SetActive(true);
        //播放相应的动画
        playerManager.playerAnimatorManager.PlayTargetAnimation("Sitting", true);
        //让 输入系统失效
        playerManager.inputHandler.DisableInput();
        
        //回复玩家的血量 魔法 体力等
        playerManager.ResetPlayerStats();
        playerManager.ResetPoisonState();
        //回复固定道具
        
        //处置玩家的状态 包括异常和 增益状态
        
        //显示UI
        uiManager.OpenBonFirePanel();
    }
}
