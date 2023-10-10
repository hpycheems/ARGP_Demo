using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

/// <summary>
/// Character 基类
/// </summary>
public class CharacterManager : MonoBehaviour
{
    private CharacterAnimatorManager characterAnimatorManager;
    private CharacterWeaponSlotManager characterWeaponSlotManager;
    
    [Header("Lock On Transform")]
    public Transform lockOnTransform;

    [Header("Combat Colliders")]
    public CriticalDamageCollider backStabDamageCollider;
    public CriticalDamageCollider riposteDamageCollider;

    [Header("Interaction")]
    public bool isInteracting;//是否正在交互
    
    [Header("Combat Flags")]
    public bool canBeRiposte;
    public bool canBeParried;
    public bool canDoCombo;
    public bool isParrying;
    public bool isParried;
    public bool isBlocking;
    public bool isInvulerable;
    public bool isUsingRightHand;
    public bool isUsingLeftHand;
    public bool isTwoHandingWeapon;
    
    [Header("Movement Flags")]
    public bool isRotatingWithRootMotion;
    public bool canRotate;
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    
    [Header("Spells")]
    public bool isFiringSpell;
    
    //刺击伤害
    public int pendingCriticalDamage;

    protected virtual void Awake()
    {
        characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
        
        characterAnimatorManager = GetComponentInChildren<CharacterAnimatorManager>();
    }

    protected virtual void FixedUpdate()
    {
        characterAnimatorManager.CheckHandIKWeight(characterWeaponSlotManager.rightHandIKTarget,
            characterWeaponSlotManager.leftHandIKTarget, isTwoHandingWeapon);
    }

    public virtual void UpdateWhichHandCharacterIsUsing(bool usingLeftHand)
    {
        if (usingLeftHand)
        {
            isUsingLeftHand = true;
            isUsingRightHand = false;
        }
        else
        {
            isUsingLeftHand = false;
            isUsingRightHand = true;
        }
    }
    
}
