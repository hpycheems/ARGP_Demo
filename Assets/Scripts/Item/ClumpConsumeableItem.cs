using UnityEngine;


[CreateAssetMenu(menuName = "Items/Consumable/Clump")]
public class ClumpConsumeableItem : ConsumableItem
{
    public GameObject clumpConsumeFX;
    
    public bool curePoison;

    public override void AttemptToConsumeItem(PlayerAnimatorManager animatorManager, 
        PlayerWeaponSlotManager playerWeaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        base.AttemptToConsumeItem(animatorManager, playerWeaponSlotManager, playerEffectsManager);
        GameObject clump = Instantiate(itemModel, playerWeaponSlotManager.rightHandSlot.transform);
        playerEffectsManager.currentParticleFX = clumpConsumeFX;
        playerEffectsManager.instantiatedFXModel = clump;
        if (curePoison)
        {
            playerEffectsManager.poisonBuildup = 0;
            playerEffectsManager.poisonAmount = 100;
            playerEffectsManager.isPoisoned = false;

            if (playerEffectsManager.currentPoisonParticleFX != null)
            {
                Destroy(playerEffectsManager.currentPoisonParticleFX);
            }
        }
        playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
    }
}
