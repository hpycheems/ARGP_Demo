using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    /// <summary>
    /// 初始化血条
    /// </summary>
    /// <param name="maxHealth"></param>
    public void SetMaxStamina(int maxStamina)
    {
        slider.maxValue = maxStamina;
        slider.value = maxStamina;
    }

    /// <summary>
    /// 设置当前血条
    /// </summary>
    /// <param name="currentHealth"></param>
    public void SetCurrentStamina(float currentStamina)
    {
        slider.value = currentStamina;
    }
}
