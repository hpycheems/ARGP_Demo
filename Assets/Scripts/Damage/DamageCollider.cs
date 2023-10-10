using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 伤害碰撞器脚本 一般放置在能造成伤害的武器或者其他物体上
/// </summary>
public class DamageCollider : MonoBehaviour
{
    [Header("team ID")]
    public int teamIDNumber = 0;
    
    /// <summary>
    /// 碰撞器
    /// </summary>
    [SerializeField] protected Collider damageCollider;
    //当攻击者被反击时 使用其来获取AnimatorManager 播放被反击动画
    public CharacterManager targetCharacterManager;

    public bool enabledDamageColliderOnStartUp;
    public CharacterStatsManager spellTarget;

    public float poiseBreak;
    public float offensivePoiseDefence;
    
    
    /// <summary>
    /// 当前武器的伤害值
    /// </summary>
    public int physicalDamage;
    public int fireDamage;
    public int magicDamage;
    public int lightningDamage;
    public int darkDamage;

    private bool shieldHasBeenHit;
    private bool hasBeenParried;
    protected string currentDamageAnimation;
    
    protected virtual void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = enabledDamageColliderOnStartUp;
    }

    /// <summary>
    /// 打开伤害碰撞器
    /// </summary>
    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }
    /// <summary>
    /// 关闭伤害碰撞器
    /// </summary>
    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }

    /// <summary>
    /// 武器碰撞到物体时
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")//碰撞到玩家受伤
        {
            shieldHasBeenHit = false;
            hasBeenParried = false;
            
            //Get Components
            CharacterStatsManager characterStatsManger = other.GetComponent<CharacterStatsManager>();
            CharacterManager characterManager = other.GetComponent<CharacterManager>();
            CharacterEffectManager characterEffectManager = other.GetComponent<CharacterEffectManager>();
            BlockingCollider shield = other.transform.GetComponentInChildren<BlockingCollider>();
            
            if (characterManager != null)//如果存在
            {
                if (characterStatsManger.teamIDNumber == teamIDNumber)
                    return;
                CheckForParry(characterManager);
                CheckForBlock(characterManager, characterStatsManger, shield);
            }
            
            if (characterStatsManger != null)
            {
                if (characterStatsManger.teamIDNumber == teamIDNumber)
                    return;

                if (hasBeenParried)
                    return;

                if (shieldHasBeenHit)
                    return;
                
                //计算硬直 
                characterStatsManger.poiseResetTimer = characterStatsManger.totalPoiseResetTime;
                characterStatsManger.totalPoiseDefence -= poiseBreak;
                
                //生成伤害特效
                Vector3 contactPoint =
                    other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                float directionHitFrom = Vector3.SignedAngle(targetCharacterManager.transform.forward,
                    characterManager.transform.forward, Vector3.up);
                ChooseWhichDirectionDamageCameFrom(directionHitFrom);
                characterEffectManager.PlayBloodSplatterFX(contactPoint);
                
                if (characterStatsManger.totalPoiseDefence > poiseBreak)
                {
                    characterStatsManger.TakeDamageNoAnimation(physicalDamage,0);
                }
                else
                {
                    characterStatsManger.TakeDamage(physicalDamage,0, currentDamageAnimation);
                }
            }
        }
        
        if (other.CompareTag("Illusionary Wall"))
        {
            IllusionaryWall illusionaryWall = other.GetComponent<IllusionaryWall>();
            
            illusionaryWall.wallHasBeenHit = true;
        }
    }

    protected virtual void CheckForBlock(CharacterManager characterManager, 
        CharacterStatsManager characterStatsManger, BlockingCollider shield)
    {
        if (shield != null && characterManager.isBlocking)//防御
        {
            //计算防御受到的伤害
            float physicalDamageAfterBlock =
                physicalDamage - (physicalDamage * shield.blockingPhysicalDamageAbsorption) / 100;
            float fireDamageAfterBlock = fireDamage - (fireDamage * shield.blockingFireDamageAbsorption) / 100;
                    
            if (characterStatsManger != null)
            {
                characterStatsManger.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock),Mathf.RoundToInt(fireDamageAfterBlock), "Block Guard");
                shieldHasBeenHit = true;
            }
        }
    }

    protected virtual void CheckForParry(CharacterManager characterManager)
    {
        if (characterManager.isParrying)//并且可以反击
        {
            targetCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().
                PlayTargetAnimation("Parried", true);
            hasBeenParried = true;
        }
    }
    
    protected virtual void ChooseWhichDirectionDamageCameFrom(float direction)
    {
        if (direction >= 145 && direction <= 180)
        {
            currentDamageAnimation = "Damage_Forward_01";
        }
        else if (direction <= -145 && direction >= -180)
        {
            currentDamageAnimation = "Damage_Forward_01";
        }
        else if (direction >= -45 && direction <= 45)
        {
            currentDamageAnimation = "Damage_Back_01";
        }
        else if (direction >= -144 && direction <= -45)
        {
            currentDamageAnimation = "Damage_Left_01";
        }
        else if (direction >= 45 && direction <= 144)
        {
            currentDamageAnimation = "Damage_Right_01";
        }
    }
}
