﻿using UnityEngine;
using UnityEngine.UI;


public class PoisonBuildUpBar : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = 100;
        slider.value = 0;
        gameObject.SetActive(false);
    }
    
    public void SetPoisonBuildUp(int currentPoisonBuildUp)
    {
        slider.value = currentPoisonBuildUp;
    }
}
