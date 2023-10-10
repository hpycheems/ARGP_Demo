using UnityEngine;
using UnityEngine.UI;


public class PoisonAmountBar : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = 100;
        slider.value = 100;
        gameObject.SetActive(false);
    }
    
    public void SetPoisonAmount(int currentPoisonAmount)
    {
        slider.value = currentPoisonAmount;
    }
}
