using System;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SoulCountBar : MonoBehaviour
{
    public TMP_Text soulCountText;

    public void SetSoulCountText(int soulCount)
    {
        soulCountText.text = soulCount.ToString();
    }
}