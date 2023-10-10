using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFileData : UIEffectBase
{
    GameFileData data;
    private Button button;
    public Image icon;
    public TMP_Text time;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            //把存档传递到GameManager
            if(data != null)
                GameManager.Instance.LoadGameSaveFile(data);
        });
    }

    public void UpdateDisplay(Sprite icon, GameFileData data = null)
    {
        if (data != null)
        {
            this.data = data;
            this.time.text = data.time;
        }
        else
        {
            this.time.text = "";
        }
        
        if (icon != null)
        {
            this.icon.sprite = icon;
            this.icon.gameObject.SetActive(true);
        }
        else
        {
            this.icon.sprite = null;
            this.icon.gameObject.SetActive(false);
        }
    }
}
