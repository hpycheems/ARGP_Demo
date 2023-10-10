using System;
using UnityEngine;

[Serializable]
public class SettingData
{
    [Header("视图")]
    public int resolutionIndex;
    public bool isFullScreen;

    [Header("音频")]
    public int musicVolume;
    public int sfxVolume;
}
