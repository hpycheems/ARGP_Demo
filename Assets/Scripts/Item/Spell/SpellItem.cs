using UnityEngine;

/// <summary>
/// 咒语 Item
/// </summary>
public class SpellItem : Item
{
    //生成的特效
    public GameObject spellWarmUpFX;
    public GameObject spellCastFX;
    //念咒语 动画
    public string spellAnimation;

    [Header("Spell Cost")]
    public int focusPointCost;
    
    [Header("Spell Type")] 
    public bool isFaithSpell;
    public bool isMagicSpell;
    public bool isPyroSpell;
    
    [Header("Spell Description")] 
    [TextArea]
    public string spellDescription;
    
    /// <summary>
    /// 尝试 施咒
    /// </summary>
    /// <param name="playerAnimatorManager"></param>
    /// <param name="playerStatsManager"></param>
    public virtual void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, 
        PlayerStatsManager playerStatsManager, 
        PlayerWeaponSlotManager playerWeaponSlotManager, bool isLeftHanded)
    {
        
    }
    /// <summary>
    /// 释放成功
    /// </summary>
    /// <param name="playerAnimatorManager"></param>
    /// <param name="playerStatsManager"></param>
    public virtual void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, 
        PlayerStatsManager playerStatsManager, 
        PlayerWeaponSlotManager playerWeaponSlotManager,
        CameraHandler cameraHandler,
        bool isLeftHanded)
    {
        playerStatsManager.DeductFocusPoints(focusPointCost);
        
    }
}
