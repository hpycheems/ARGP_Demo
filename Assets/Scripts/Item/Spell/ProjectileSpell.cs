using UnityEngine;


/// <summary>
/// 阉割
/// </summary>
[CreateAssetMenu(menuName = "Spells/Projectile Spell")]
public class ProjectileSpell : SpellItem
{
    public float baseDamage;
    
    public float projectileForwardVelocity;
    public float projectileUpwardVelocity;
    public float projectileMess;
    public bool isEffectedByGravity;
    private Rigidbody _rigidbody;

    public override void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, 
        PlayerStatsManager playerStatsManager, 
        PlayerWeaponSlotManager playerWeaponSlotManager,
        bool isLeftHanded)
    {
        base.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager,isLeftHanded);
        if (isLeftHanded)
        {
            GameObject instantiateWarmUpSpell = Instantiate(spellWarmUpFX, playerWeaponSlotManager.rightHandSlot.transform);
            instantiateWarmUpSpell.gameObject.transform.localScale = new Vector3(100, 100, 100);
            playerAnimatorManager.PlayTargetAnimation(spellAnimation, true,0.2f,false,isLeftHanded);
        }
        else
        {
            GameObject instantiateWarmUpSpell = Instantiate(spellWarmUpFX, playerWeaponSlotManager.rightHandSlot.transform);
            instantiateWarmUpSpell.gameObject.transform.localScale = new Vector3(100, 100, 100);
            playerAnimatorManager.PlayTargetAnimation(spellAnimation, true,0.2f,false,isLeftHanded);
        }
    }

    public override void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, 
        PlayerStatsManager playerStatsManager, 
        PlayerWeaponSlotManager playerWeaponSlotManager,
        CameraHandler cameraHandler, bool isLeftHanded)
    {
        base.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager, cameraHandler,isLeftHanded);
        if (isLeftHanded)
        {
            GameObject instantiateSpellFX = Instantiate(spellCastFX, playerWeaponSlotManager.rightHandSlot.transform.position,
                cameraHandler.cameraPivotTransform.rotation);
            SpellDamageCollider spellDamageCollider = instantiateSpellFX.GetComponent<SpellDamageCollider>();
            spellDamageCollider.teamIDNumber = playerStatsManager.teamIDNumber;
            _rigidbody = instantiateSpellFX.GetComponent<Rigidbody>();
            
            if (cameraHandler.currentLockOnTarget != null)
            {
                instantiateSpellFX.transform.LookAt(cameraHandler.currentLockOnTarget.transform);
            }
            else
            {
                instantiateSpellFX.transform.rotation = Quaternion.Euler(
                    cameraHandler.cameraPivotTransform.eulerAngles.x, playerStatsManager.transform.eulerAngles.y, 0);
            }
        
            _rigidbody.AddForce(instantiateSpellFX.transform.forward * projectileForwardVelocity);
            _rigidbody.AddForce(instantiateSpellFX.transform.up * projectileUpwardVelocity);
            _rigidbody.useGravity = isEffectedByGravity;
            _rigidbody.mass = projectileMess;
            instantiateSpellFX.transform.parent = null;
        }
        else
        {
            GameObject instantiateSpellFX = Instantiate(spellCastFX, playerWeaponSlotManager.rightHandSlot.transform.position,
                cameraHandler.cameraPivotTransform.rotation);
            SpellDamageCollider spellDamageCollider = instantiateSpellFX.GetComponent<SpellDamageCollider>();
            spellDamageCollider.teamIDNumber = playerStatsManager.teamIDNumber;
            _rigidbody = instantiateSpellFX.GetComponent<Rigidbody>();
            if (cameraHandler.currentLockOnTarget != null)
            {
                instantiateSpellFX.transform.LookAt(cameraHandler.currentLockOnTarget.transform);
            }
            else
            {
                instantiateSpellFX.transform.rotation = Quaternion.Euler(
                    cameraHandler.cameraPivotTransform.eulerAngles.x, playerStatsManager.transform.eulerAngles.y, 0);
            }
        
            _rigidbody.AddForce(instantiateSpellFX.transform.forward * projectileForwardVelocity);
            _rigidbody.AddForce(instantiateSpellFX.transform.up * projectileUpwardVelocity);
            _rigidbody.useGravity = isEffectedByGravity;
            _rigidbody.mass = projectileMess;
            instantiateSpellFX.transform.parent = null;
        }
        
        //SpellDamageCollider spellDamageCollider = instantiateSpellFX.GetComponent<SpellDamageCollider>();
        //spellDamageCollider.teamIDNumber = playerStatsManager.teamIDNumber;
    }
}
