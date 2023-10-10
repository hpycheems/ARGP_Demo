using System;
using UnityEngine;

public class EnemyAnimatorManager : CharacterAnimatorManager
{
    //Components
    private EnemyManager enemyManager;
    private EnemyStatsManager enemyStatsManager;
    private EnemyEffectManager enemyEffectManager;
    private EnemyWeaponSlotManager enemyWeaponSlotManager;
    
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        
        enemyManager = GetComponentInParent<EnemyManager>();
        enemyStatsManager = GetComponentInParent<EnemyStatsManager>();
        enemyEffectManager = GetComponentInParent<EnemyEffectManager>();
        enemyWeaponSlotManager = GetComponentInParent<EnemyWeaponSlotManager>();
    }

    /// <summary>
    /// 受到刺击伤害
    /// </summary>
    public override void TakeCriticalDamage()
    {
        enemyStatsManager.TakeDamageNoAnimation(enemyManager.pendingCriticalDamage,0);
        enemyManager.pendingCriticalDamage = 0;
    }

    /// <summary>
    /// 敌人死亡 回调， 为玩家添加灵魂
    /// </summary>
    public override void AwardSoulsOnDeath()
    {
        //GetComponents
        PlayerStatsManager playerStatsManager = FindObjectOfType<PlayerStatsManager>();
        SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();
        
        if (playerStatsManager != null)
        {
            playerStatsManager.AddSouls(enemyStatsManager.soulsAwardedOnDeath);
            soulCountBar.SetSoulCountText(playerStatsManager.soulCount);
        }
    }

    /// <summary>
    /// 动画 rootMotion
    /// </summary>
    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyManager.enemyRigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.enemyRigidbody.velocity = velocity;

        if (enemyManager.isRotatingWithRootMotion)
        {
            enemyManager.transform.rotation *= anim.deltaRotation;
        }
    }
    
    #region Animation Signal

    public void PlayWeaponTrial()
    {
        enemyEffectManager.PlayWeaponFX(false);
    }
    
    public void EnableIsParrying()
    {
        enemyManager.isParrying = true;
    }

    public void DisableIsParrying()
    {
        enemyManager.isParrying = false;
    }

    public void EnableCanBeRiposted()
    {
        enemyManager.canBeParried = true;
    }
    public void DisableCanBeRiposted()
    {
        enemyManager.canBeParried = false;
    }
    
    public void OpenDamageCollider()
    {
        enemyWeaponSlotManager.OpenDamageCollider();
    }

    public void CloseDamageCollider()
    {
        enemyWeaponSlotManager.CloseDamageCollider();
    }
    
    public void GrantWeaponAttackingPoiseBonus()
    {
        enemyWeaponSlotManager.GrantWeaponAttackingPoiseBonus();
    }

    public void ResetWeaponAttackingPoiseBonus()
    {
        enemyWeaponSlotManager.ResetWeaponAttackingPoiseBonus();
    }
    
    public void EnableIsParried()
    {
        enemyManager.isParried = true;
    }
    public void DisableIsParried()
    {
        enemyManager.isParried = false;
    }
    
    #endregion
}
