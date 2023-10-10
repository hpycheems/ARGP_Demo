using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialog : Interactable
{

    public DialogManager dialogManager;
    public UIManager uiManager;
    
    private void Awake()
    {
        dialogManager = FindObjectOfType<DialogManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
        //关闭提示UI
        playerManager.interactableGameObject.SetActive(false);
        uiManager.CloseDefaultWindow();
        //开启对话
        dialogManager.ShowDialogRow();
    }
}
