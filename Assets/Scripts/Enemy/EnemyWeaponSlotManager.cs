using System;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
{
    public override void GrantWeaponAttackingPoiseBonus()
    {
        characterStatsManager.totalPoiseDefence += characterStatsManager.offensivePoiseBonus;
    }

    public override void ResetWeaponAttackingPoiseBonus()
    {
        characterStatsManager.totalPoiseDefence = characterStatsManager.armorPoiseBonus;
    }
}
