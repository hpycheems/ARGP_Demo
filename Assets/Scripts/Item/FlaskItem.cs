using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumable/Flask")]
public class FlaskItem : ConsumableItem
{
    public bool estusFlask;
    public bool ashenFlask;

    //加血量
    public int healthRecoverAmount;
    public int focusPointsRecoverAmount;

    //使用道具时的 FX
    public GameObject recoverFX;

    public override void AttemptToConsumeItem(PlayerAnimatorManager animatorManager, 
        PlayerWeaponSlotManager playerWeaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        base.AttemptToConsumeItem(animatorManager, playerWeaponSlotManager, playerEffectsManager);
        GameObject flask = Instantiate(itemModel, playerWeaponSlotManager.rightHandSlot.transform);
        playerEffectsManager.currentParticleFX = recoverFX;
        playerEffectsManager.amountToBeHeal = healthRecoverAmount;
        playerEffectsManager.instantiatedFXModel = flask;
        playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
    }
}
