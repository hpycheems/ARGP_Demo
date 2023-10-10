using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingButton : UIEffectBase
{
    public Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
}
