using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Healing Spell")]
public class HealingSpell : SpellItem
{
    //加血的数量
    public int healAmount;

    /// <summary>
    /// 尝试施法
    /// </summary>
    /// <param name="playerAnimatorManager"></param>
    /// <param name="playerStatsManager"></param>
    public override void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, 
        PlayerStatsManager playerStatsManager, 
        PlayerWeaponSlotManager playerWeaponSlotManager,
        bool isLeftHanded)
    {
        base.AttemptToCastSpell(playerAnimatorManager,playerStatsManager,playerWeaponSlotManager,isLeftHanded);
        GameObject instantiateWarmUpSpellFX = Instantiate(spellWarmUpFX, playerAnimatorManager.transform);
        playerAnimatorManager.PlayTargetAnimation(spellAnimation, true,0.2f,false,isLeftHanded);
    }

    /// <summary>
    /// 成功施法
    /// </summary>
    /// <param name="playerAnimatorManager"></param>
    /// <param name="playerStatsManager"></param>
    public override void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, 
        PlayerStatsManager playerStatsManager, 
        PlayerWeaponSlotManager playerWeaponSlotManager,
        CameraHandler cameraHandler,bool isLeftHanded)
    {
        base.SuccessfullyCastSpell(playerAnimatorManager,playerStatsManager,playerWeaponSlotManager,cameraHandler,isLeftHanded);
        GameObject instantiatedSpellFX = Instantiate(spellCastFX, playerAnimatorManager.transform);
        playerStatsManager.HealPlayer(healAmount);
    }
}
