using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 武器物品类
/// </summary>
[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Animator Replacer")]
    public AnimatorOverrideController weaponController;
    public string offHandIdleAnimation = "Left_Arm_Idle01";

    [Header("Weapon Type")] 
    public WeaponType weaponType;
    
    [Header("Damage")]
    public int physicalDamage = 25;
    public int fireDamage;
    public int criticalDamageMultiplier = 4;
    
    [Header("Poise")] 
    public float poiseBreak;
    public float offensivePoiseBonus;
    
    [Header("Absorption")]
    public float physicalDamageAbsorption;
    
    [Header("Stamina Costs")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;

    [Header("Item Action")] 
    public ItemAction tap_RB_Action;
    public ItemAction tap_RT_Action;
    public ItemAction tap_LB_Action;
    public ItemAction tap_LT_Action;
    
    public ItemAction hold_RB_Action;
    public ItemAction hold_RT_Action;
    public ItemAction hold_LB_Action;
    public ItemAction hold_LT_Action;
}
