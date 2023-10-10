using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家 进入受伤 区域
/// </summary>
public class DamagePlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerStatsManager statsManager = other.GetComponent<PlayerStatsManager>();
        if (statsManager != null)
        {
            //statsManager.TakeDamage(25,0);
        }
    }
}
