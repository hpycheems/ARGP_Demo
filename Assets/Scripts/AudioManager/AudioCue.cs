using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音频
/// </summary>
public class AudioCue : MonoBehaviour
{
     WaitForSeconds wate = new WaitForSeconds(0.5f);
    private void OnEnable()
    {
        StartCoroutine(DelayRecovery());
    }

    IEnumerator DelayRecovery()
    {
        yield return wate;
        //回收到对象池
        AudioPool.Instance.PushAudio(gameObject,gameObject.name == "SFXAudioSource");
    }
}
