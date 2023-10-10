using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音频 管理器 用于播放音频
/// </summary>
public class AudioManager : MonoBehaviour
{
    #region Singleton

    private AudioManager() {}
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    GameObject newObj = new GameObject("AudioManager");
                    instance = newObj.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }

    #endregion
    [Header("音频工厂")]
    public AudioFactory factory;
    [Header("音乐和音效大小")]
    public int musicVolume;
    public int sfxVolume;
    [Header("各种音乐片段")]
    public AudioClip onButtonEnterClip;
    public AudioClip onButtonClickClip;
    
    public AudioClip newGamePanelAudioClip;
    private AudioSource backGroundMusic;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        factory = GetComponent<AudioFactory>();
        GameManager.Instance.OnMusicVolumeChange += (x) => { musicVolume = x; };
        GameManager.Instance.OnSFXVolumeChange += (x) => { sfxVolume = x; };
    }

    /// <summary>
    /// 用于播放音乐
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="isSFX"></param>
    /// <param name="parent"></param>
    public void PlayAudio(AudioClip clip, bool isSFX,Transform parent = null)
    {
        AudioSource audioSource = null;
        if (isSFX)
        {
            GameObject obj = AudioPool.Instance.GetSFXAudioCue();
            if (obj == null)
            {
                obj = factory.InstantiatePrefab(isSFX);
                obj.transform.parent = transform;
            }
            audioSource = obj.GetComponent<AudioSource>();
            audioSource.volume = sfxVolume / 100.0f;
        }
        else
        {
            if (backGroundMusic == null)
            {
                backGroundMusic = AudioPool.Instance.GetMusicAudioCue().GetComponent<AudioSource>();
            }

            backGroundMusic.transform.parent = transform;
            backGroundMusic.volume = musicVolume / 100.0f;
            backGroundMusic.clip = clip;
            backGroundMusic.loop = true;
            backGroundMusic.playOnAwake = true;
            backGroundMusic.Play();
            return;
        }

        if (audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.gameObject.SetActive(true);
            audioSource.Play();    
        }
    }

    /// <summary>
    /// 停止播放背景音乐
    /// </summary>
    public void StopBackgroundMusic()
    {
        if (backGroundMusic != null)
        {
            backGroundMusic.Stop();
            backGroundMusic.clip = null;
        }
    }

    /// <summary>
    /// 按钮按下的音效
    /// </summary>
    public void OnButtonClickAudio()
    {
        AudioClip clip = onButtonClickClip;
        PlayAudio(clip, true);
    }
}
