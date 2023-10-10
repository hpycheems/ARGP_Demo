using UnityEngine;
using UnityEngine.UI;


public class FocusPointBar : MonoBehaviour
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
    public void SetMaxFocusPoints(int maxFocus)
    {
        slider.maxValue = maxFocus;
        slider.value = maxFocus;
    }

    /// <summary>
    /// 设置当前血条
    /// </summary>
    /// <param name="currentHealth"></param>
    public void SetCurrentFocusPoints(float currentFocus)
    {
        slider.value = currentFocus;
    }
}
