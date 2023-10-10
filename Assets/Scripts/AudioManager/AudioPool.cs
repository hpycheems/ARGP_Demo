using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPool : MonoBehaviour
{
    
    #region Singleton

    private AudioPool() {}
    private static AudioPool instance;
    public static AudioPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioPool>();
                if (instance == null)
                {
                    GameObject newObj = new GameObject("AudioPool");
                    instance = newObj.AddComponent<AudioPool>();
                }
            }
            return instance;
        }
    }

    #endregion
    
    private AudioFactory factory;
    
    public int sfxAudioWarmUp = 5;
    public int musicAudioWarmUp = 1;

    private List<GameObject> musicAudioCuePool = new List<GameObject>();
    private List<GameObject> sfxAudioCuePool = new List<GameObject>();
    
    private void Awake()
    {
        factory = GetComponent<AudioFactory>();
    }

    private void Start()
    {
        for (int i = 0; i < sfxAudioWarmUp; i++)
        {
            AddSFXAudioCue(true);
        }

        for (int i = 0; i < musicAudioWarmUp; i++)
        {
            AddSFXAudioCue(false);
        }
    }

    public GameObject AddSFXAudioCue(bool isSFX)
    {
        GameObject obj = null;
        if (isSFX)
        {
            obj = factory.InstantiatePrefab(true);
            obj.transform.parent = transform;
            sfxAudioCuePool.Add(obj);
        }
        else
        {
            obj = factory.InstantiatePrefab(false);
            obj.transform.parent = transform;
            musicAudioCuePool.Add(obj);
        }

        return obj;
    }

    public GameObject GetSFXAudioCue()
    {
        for (int i = 0; i < sfxAudioCuePool.Count; i++)
        {
            if (sfxAudioCuePool[i].activeSelf == false)
            {
                GameObject obj = sfxAudioCuePool[i];
                sfxAudioCuePool.RemoveAt(i);
                return obj;
            }
        }
        return null;
    }

    public GameObject GetMusicAudioCue()
    {
        for (int i = 0; i < musicAudioCuePool.Count; i++)
        {
            if (musicAudioCuePool[i].activeSelf == false)
            {
                GameObject obj = musicAudioCuePool[i];
                obj.SetActive(true);
                musicAudioCuePool.RemoveAt(i);
                return obj;
            }
        }
        return null;
    }

    public void PushAudio(GameObject obj,bool isSfx)
    {
        obj.SetActive(false);
        if (isSfx)
        {
            sfxAudioCuePool.Add(obj);
        }
        else
        {
            musicAudioCuePool.Add(obj);
        }
    }
}
