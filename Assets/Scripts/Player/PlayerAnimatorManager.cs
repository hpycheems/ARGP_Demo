using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家AnimatorManager
/// </summary>
public class PlayerAnimatorManager : CharacterAnimatorManager
{
    //Components
    private PlayerEffectsManager playerEffectsManager;
    private PlayerWeaponSlotManager playerWeaponSlotManager;
    private PlayerLocomotionManager playerLocomotionManager;
    private PlayerStatsManager playerStatsManager;
    private PlayerCombatManager playerCombatManager;
    private PlayerManager playerManager;
    
    //Input Signal
    private int vertical;
    private int horizontal;
    
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        playerLocomotionManager = GetComponentInParent<PlayerLocomotionManager>();
        playerWeaponSlotManager = GetComponentInParent<PlayerWeaponSlotManager>();
        playerEffectsManager = GetComponentInParent<PlayerEffectsManager>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerStatsManager = GetComponentInParent<PlayerStatsManager>();
        playerCombatManager = GetComponentInParent<PlayerCombatManager>();
    }

    public void Initialize()
    {
        anim = GetComponentInChildren<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    /// <summary>
    /// 更新Horizontal Vetical
    /// </summary>
    /// <param name="verticalMovement"></param>
    /// <param name="horizontalMovement"></param>
    /// <param name="isSprinting"></param>
    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
        #region vertical
        float v = 0;
        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            v = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            v = 1f;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            v = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            v = -1f;
        }
        else
        {
            v = 0;
        }

        #endregion

        #region Horizontal
        float h = 0;
        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            h = 0.5f;
        }
        else if (horizontalMovement > 0.55f)
        {
            h = 1f;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            h = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            h = -1f;
        }
        else
        {
            h = 0;
        }

        #endregion

        if (isSprinting)
        {
            v = 2;
            h = horizontalMovement;
        }
        anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    /// <summary>
    /// 收到刺击伤害 回调
    /// </summary>
    public override void TakeCriticalDamage()
    {
        playerStatsManager.TakeDamageNoAnimation(playerManager.pendingCriticalDamage,0);
        playerManager.pendingCriticalDamage = 0;
    }

    /// <summary>
    /// Unity Animator 根运动
    /// </summary>
    private void OnAnimatorMove()
    {
        if(playerManager.isInteracting == false) 
            return;

        float delta = Time.deltaTime;
        playerLocomotionManager.rigidbody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        playerLocomotionManager.rigidbody.velocity = velocity;
    }
    
    #region Animation Signal

    public void EnableIsParrying()
    {
        playerManager.isParrying = true;
    }

    public void DisableIsParrying()
    {
        playerManager.isParrying = false;
    }

    public void EnableCanBeRiposted()
    {
        playerManager.canBeParried = true;
    }
    public void DisableCanBeRiposted()
    {
        playerManager.canBeParried = false;
    }

    public void OpenDamageCollider()
    {
        playerWeaponSlotManager.OpenDamageCollider();
    }

    public void CloseDamageCollider()
    {
        playerWeaponSlotManager.CloseDamageCollider();
    }
    /// <summary>
    /// 攻击消耗体力 轻
    /// </summary>
    public override void DrainStaminaLightAttack()
    {
        playerWeaponSlotManager.DrainStaminaLightAttack();
    }

    /// <summary>
    /// 攻击消耗体力 重
    /// </summary>
    public override void DrainStaminaHeavyAttack()
    {
        playerWeaponSlotManager.DrainStaminaHeavyAttack();
    }
    
    /// <summary>
    /// 动画事件回调 
    /// </summary>
    public void HealPlayerFromEffect()
    {
        playerEffectsManager.HealPlayerFromEffect();
    }

    public void SuccessfullyThrowFireBomb()
    {
        playerWeaponSlotManager.SuccessfullyThrowFireBomb();
    }

    public void SuccessfullyCastSpell()
    {
        playerCombatManager.SuccessfullyCastSpell();
    }

    public void DisableSetPosition()
    {
        playerLocomotionManager.DisableSetTransformPosition();
    }

    public void GrantWeaponAttackingPoiseBonus()
    {
        playerWeaponSlotManager.GrantWeaponAttackingPoiseBonus();
    }

    public void ResetWeaponAttackingPoiseBonus()
    {
        playerWeaponSlotManager.ResetWeaponAttackingPoiseBonus();
    }

    public void EnableIsParried()
    {
        playerManager.isParried = true;
    }
    public void DisableIsParried()
    {
        playerManager.isParried = false;
    }
    
    #endregion
}
