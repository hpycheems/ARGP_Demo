using System;
using UnityEngine;

/// <summary>
/// 玩家武器槽管理器
/// </summary>
public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
{
    //Components
    public QuickSlotUI quickSlotUI;
    private InputHandler inputHandler;
    private CameraHandler cameraHandler;
    private PlayerManager playerManager;
    private PlayerStatsManager playerStatsManager;
    private PlayerEffectsManager playerEffectsManager;
    private PlayerInventoryManager playerInventoryManager;
    private PlayerAnimatorManager playerAnimatorManager;
    
    protected override void Awake()
    {
        base.Awake();
        quickSlotUI = FindObjectOfType<QuickSlotUI>();
        
        //获取 组件
        playerStatsManager = GetComponent<PlayerStatsManager>();
        inputHandler = GetComponent<InputHandler>();
        playerManager = GetComponent<PlayerManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        playerInventoryManager =  GetComponent<PlayerInventoryManager>();
        
        playerAnimatorManager =  GetComponentInChildren<PlayerAnimatorManager>();
    }
    private void Start()
    {
        cameraHandler = CameraHandler.singleton;
    }
    public void UpdateQuickSlotUI(Item item)
    {
        quickSlotUI.UpdateCurrentConsumableIcon(item as ConsumableItem);
    }

    /// <summary>
    /// 加载武器到武器槽
    /// </summary>
    /// <param name="weaponItem"></param>
    /// <param name="isLeft"></param>
    public override void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (weaponItem != null)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                if(!weaponItem.isUnarmed)
                    LoadLeftWeaponDamageCollider();
                //更新 Quick Slot
                quickSlotUI.UpdateWeaponQuickSlotsUI(weaponItem, isLeft);
                characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, 0.2f,true);
            }
            else
            {
                if (characterManager.isTwoHandingWeapon)
                {
                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    leftHandSlot.UnloadWeaponAndDestroy();
                    characterAnimatorManager.PlayTargetAnimation("TH_Idle_01", false, 0.2f,true);
                }
                else
                {
                    backSlot.UnloadWeaponAndDestroy();
                    characterAnimatorManager.PlayTargetAnimation("Both_Arm_Empty", false, 0.2f,true);
                }
                
                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                if(!weaponItem.isUnarmed)
                    LoadRightWeaponDamageCollider();
                quickSlotUI.UpdateWeaponQuickSlotsUI(weaponItem, isLeft);
                characterAnimatorManager.anim.runtimeAnimatorController = weaponItem.weaponController;
            }
        }
        else
        {
            weaponItem = unarmedWeapon;
            if (isLeft)
            {
                characterAnimatorManager.anim.CrossFade("Left_Arm_Empty", 0.2f);
                characterInventoryManager.leftWeapon = weaponItem;
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                //LoadLeftWeaponDamageCollider();
                quickSlotUI.UpdateWeaponQuickSlotsUI(weaponItem, true);
                characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, 0.2f,true);
            }
            else
            {
                characterAnimatorManager.anim.CrossFade("Right_Arm_Empty", 0.2f);
                characterInventoryManager.rightWeapon = weaponItem;
                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                //LoadRightWeaponDamageCollider();
                quickSlotUI.UpdateWeaponQuickSlotsUI(weaponItem, true);
                characterAnimatorManager.anim.runtimeAnimatorController = weaponItem.weaponController;
            }
        }
    }
    
    /// <summary>
    /// 攻击消耗体力 轻
    /// </summary>
    public void DrainStaminaLightAttack()
    {
        playerStatsManager.TakeStamina(Mathf.RoundToInt(playerInventoryManager.currentItemBeingUsed.
                                                            baseStamina * playerInventoryManager.currentItemBeingUsed.lightAttackMultiplier));
    }

    /// <summary>
    /// 攻击消耗体力 重
    /// </summary>
    public void DrainStaminaHeavyAttack()
    {
        playerStatsManager.TakeStamina(Mathf.RoundToInt(playerInventoryManager.currentItemBeingUsed.baseStamina * 
                                                        playerInventoryManager.currentItemBeingUsed.heavyAttackMultiplier));
    }
    
    public void SuccessfullyThrowFireBomb()
    {
        Destroy(playerEffectsManager.instantiatedFXModel);
        BombConsumeableItem fireBombItem = playerInventoryManager.currentConsumableItem as BombConsumeableItem;
        GameObject activeModelBomb = Instantiate(fireBombItem.liveBombModel, rightHandSlot.transform.position,
            cameraHandler.cameraPivotTransform.rotation);
        activeModelBomb.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x,
            playerManager.lockOnTransform.eulerAngles.y, 0);
        
        BombDamageCollider damageCollider = activeModelBomb.GetComponentInChildren<BombDamageCollider>();
        damageCollider.explosionDamage = fireBombItem.baseDamage;
        damageCollider.explosionSplashDamage = fireBombItem.explosiveDamage;
        damageCollider.bombRigidbody.AddForce(activeModelBomb.transform.forward * fireBombItem.forwardVelocity);
        damageCollider.bombRigidbody.AddForce(activeModelBomb.transform.up * fireBombItem.upwardVelocity);
        damageCollider.teamIDNumber = playerStatsManager.teamIDNumber;
        
        LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
    }

}
