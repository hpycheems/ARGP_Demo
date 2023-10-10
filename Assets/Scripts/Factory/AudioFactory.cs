using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFactory : MonoBehaviour
{
    public GameObject sfxPrefab;
    public GameObject musicPrefab;
    
    public GameObject InstantiatePrefab(bool isSFX)
    {
        GameObject obj = null;
        if (isSFX)
        {
            obj = Instantiate(sfxPrefab);
            obj.name = "SFXAudioSource";
        }
        else
        {
            obj = Instantiate(musicPrefab);
            obj.name = "MusicAudioSource";
        }
        
        if(obj!=null)
            obj.SetActive(false);
        
        return obj;
    }
}
