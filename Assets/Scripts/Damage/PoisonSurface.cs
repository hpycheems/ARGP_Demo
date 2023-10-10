using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 有毒区域
/// </summary>
public class PoisonSurface : MonoBehaviour
{
    //中毒累积时间速度
    public float poisonBuildUpAmount;
    //在此区域中的character
    public List<CharacterEffectManager> characterStatsManagers;
    
    /// <summary>
    /// 进入此区域
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        CharacterEffectManager character = other.GetComponent<CharacterEffectManager>();
        if (character != null)
        {
            characterStatsManagers.Add(character);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        foreach (CharacterEffectManager character in characterStatsManagers)
        {
            if (character.isPoisoned)
                return;
            character.poisonBuildup += poisonBuildUpAmount * Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterEffectManager character = other.GetComponent<CharacterEffectManager>();
        if (character != null)
        {
            characterStatsManagers.Remove(character);
        }
    }
}