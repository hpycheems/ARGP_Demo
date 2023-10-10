using System;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
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
    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    /// <summary>
    /// 设置当前血条
    /// </summary>
    /// <param name="currentHealth"></param>
    public void SetCurrentHealth(int currentHealth)
    {
        slider.value = currentHealth;
    }
}
