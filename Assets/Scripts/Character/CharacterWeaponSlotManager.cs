using System;
using UnityEngine;


public class CharacterWeaponSlotManager : MonoBehaviour
{
    //Components
    protected CharacterManager characterManager;
    protected CharacterStatsManager characterStatsManager;
    protected CharacterEffectManager characterEffectManager;
    protected CharacterAnimatorManager characterAnimatorManager;
    protected CharacterInventoryManager characterInventoryManager;

    [Header("空手")]
    public WeaponItem unarmedWeapon;

    [Header("武器槽")]
    public WeaponHolderSlot leftHandSlot;
    public WeaponHolderSlot rightHandSlot;
    public WeaponHolderSlot backSlot;//武器放到背后位置
    
    [Header("武器伤害碰撞器")]
    public DamageCollider leftHandDamageCollider;
    public DamageCollider rightHandDamageCollider;

    [Header("Hand IK")] 
    public RightHandIKTarget rightHandIKTarget;
    public LeftHandIKTarget leftHandIKTarget;
    
    protected virtual void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
        characterEffectManager = GetComponent<CharacterEffectManager>();
        characterInventoryManager = GetComponent<CharacterInventoryManager>();
        characterAnimatorManager = GetComponentInChildren<CharacterAnimatorManager>();

        LoadWeaponHolderSlots();
    }
    
    /// <summary>
    /// 获取武器槽
    /// </summary>
    public virtual void LoadWeaponHolderSlots()
    {
        //获得 武器槽
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandleSlot)
            {
                leftHandSlot = weaponSlot;
            }
            else if (weaponSlot.isRightHandleSlot)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.isBackSlot)
            {
                backSlot = weaponSlot;
            }
        }
    }

    /// <summary>
    /// 启动IK
    /// </summary>
    /// <param name="isTwoHandingWeapon"></param>
    public virtual void LoadTwoHandIKTargets(bool isTwoHandingWeapon)
    {
        leftHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
        rightHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();

        characterAnimatorManager.SetHandIKForWeapon(rightHandIKTarget, leftHandIKTarget, isTwoHandingWeapon);
    }

    
    /// <summary>
    /// 加载左右手武器到 武器槽
    /// </summary>
    public virtual void LoadBothWeaponOnSlots()
    {
        //设置下标
        characterInventoryManager.rightWeapon =
            characterInventoryManager.weaponsInRightHandSlots[characterInventoryManager.currentRightWeaponIndex];
        characterInventoryManager.leftWeapon =
            characterInventoryManager.weaponsInLeftHandSlots[characterInventoryManager.currentLeftWeaponIndex];
        
        //加载武器
        LoadWeaponOnSlot(characterInventoryManager.rightWeapon, false);
        LoadWeaponOnSlot(characterInventoryManager.leftWeapon, true);
    }
    
    /// <summary>
    /// 加载武器到武器槽
    /// </summary>
    /// <param name="weaponItem"></param>
    /// <param name="isLeft"></param>
    public virtual void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (weaponItem != null)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                if(!weaponItem.isUnarmed)
                    LoadLeftWeaponDamageCollider();
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
                LoadTwoHandIKTargets(characterManager.isTwoHandingWeapon);
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
                characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, 0.2f,true);
            }
            else
            {
                characterAnimatorManager.anim.CrossFade("Right_Arm_Empty", 0.2f);
                characterInventoryManager.rightWeapon = weaponItem;
                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                //LoadRightWeaponDamageCollider();
                characterAnimatorManager.anim.runtimeAnimatorController = weaponItem.weaponController;
            }
        }
    }
    
    /// <summary>
    /// 加载左手武器伤害碰撞器
    /// </summary>
    public virtual void LoadLeftWeaponDamageCollider()
    {
        //获取组件
        leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            
        //设置武器伤害
        leftHandDamageCollider.physicalDamage = characterInventoryManager.leftWeapon.physicalDamage;//设置武器基础伤害
        leftHandDamageCollider.fireDamage = characterInventoryManager.leftWeapon.fireDamage;
        leftHandDamageCollider.targetCharacterManager = characterManager;    
        
        //设置伤害ID
        leftHandDamageCollider.teamIDNumber = characterStatsManager.teamIDNumber;
            
        leftHandDamageCollider.poiseBreak = characterInventoryManager.leftWeapon.poiseBreak;
        characterEffectManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
    }

    /// <summary>
    /// 加载右手武器伤害碰撞器
    /// </summary>
    public virtual void LoadRightWeaponDamageCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            
        rightHandDamageCollider.physicalDamage = characterInventoryManager.rightWeapon.physicalDamage;//设置武器基础伤害
        rightHandDamageCollider.fireDamage = characterInventoryManager.rightWeapon.fireDamage;
        rightHandDamageCollider.targetCharacterManager = characterManager;        
        
        rightHandDamageCollider.teamIDNumber = characterStatsManager.teamIDNumber;
            
        rightHandDamageCollider.poiseBreak = characterInventoryManager.rightWeapon.poiseBreak;
        characterEffectManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
    }
    
    /// <summary>
    /// 打开武器伤害碰撞器
    /// </summary>
    public virtual void OpenDamageCollider()
    {
        if (characterManager.isUsingRightHand)
        {
            rightHandDamageCollider.EnableDamageCollider();
        }
        else if(characterManager.isUsingLeftHand)
        {
            leftHandDamageCollider.EnableDamageCollider();
        }
    }

    /// <summary>
    /// 关闭武器伤害碰撞器
    /// </summary>
    public virtual void CloseDamageCollider()
    {
        if (rightHandDamageCollider != null)
        {
            rightHandDamageCollider.DisableDamageCollider();
        }

        if (leftHandDamageCollider != null)
        {
            leftHandDamageCollider.DisableDamageCollider();
        }
    }

    /// <summary>
    /// 攻击时 设置状态条 攻击动画事件
    /// </summary>
    public virtual void GrantWeaponAttackingPoiseBonus()
    {
        WeaponItem currentWeaponBeingUsed = characterInventoryManager.currentItemBeingUsed as WeaponItem;
        characterStatsManager.totalPoiseDefence += currentWeaponBeingUsed.offensivePoiseBonus;
    }

    /// <summary>
    /// 攻击动画结束 重置 状态条
    /// </summary>
    public virtual void ResetWeaponAttackingPoiseBonus()
    {
        characterStatsManager.totalPoiseDefence = characterStatsManager.armorPoiseBonus;
    }
    
}
