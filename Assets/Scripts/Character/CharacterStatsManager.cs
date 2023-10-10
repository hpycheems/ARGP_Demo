using System;
using UnityEngine;


/// <summary>
/// Character stats 基类
/// </summary>
public class CharacterStatsManager : MonoBehaviour
{
    private CharacterAnimatorManager characterAnimatorManager;
    
    [Header("team ID")]
    public int teamIDNumber = 0;
    
    //状态条
    protected StaminaBar staminaBar;
    public int maxStamina;
    public float currentStamina;
    
    //血条
    protected HealthBar healthBar;
    public int maxHealth;
    public int currentHealth;
    
    //focus
    [Header("魔法值")]
    protected FocusPointBar focusPointBar;
    public float maxFocusPoints;
    public float currentFocusPoints;

    [Header("LEVELS")]
    public int healthLevel = 10;
    public int staminaLevel = 10;
    public int focusLevel = 10;
    public int poiseLevel = 10;
    public int strengthLevel = 10;
    public int dexterityLevel = 10;
    public int intelligenceLevel = 10;
    public int faithLevel = 10;
    
    [Header("Poise")]//硬直
    public float totalPoiseDefence;
    public float offensivePoiseBonus; //攻击姿态加成
    public float armorPoiseBonus;//防御姿态加成
    [HideInInspector] public float totalPoiseResetTime = 15;
    [HideInInspector] public float poiseResetTimer = 0;
    
    
    //装备吸收伤害
    [Header("Physical Absorption")]
    public float physicalDamageAbsorptionHead;
    public float physicalDamageAbsorptionBody;
    public float physicalDamageAbsorptionLegs;
    public float physicalDamageAbsorptionHands;
    
    [Header("Fire Absorption")]
    public float fireDamageAbsorptionHead;
    public float fireDamageAbsorptionBody;
    public float fireDamageAbsorptionLegs;
    public float fireDamageAbsorptionHands;

    //是否死亡
    public bool isDead;

    protected virtual void Awake()
    {
        characterAnimatorManager = GetComponentInChildren<CharacterAnimatorManager>();
    }
    
    private void Update()
    {
        HandlePoiseResetTimer();
    }

    private void Start()
    {
        totalPoiseDefence = armorPoiseBonus;
    }

    public virtual void TakeDamage(int physicalDamage,int fireDamage, string damageAnimation)
    {
        if (isDead) return;

        characterAnimatorManager.EraseHandIKForWeapon();
        
        float totalPhysicalDamageAbsorption = 1 -
                                              (1 - physicalDamageAbsorptionHead / 100) *
                                              (1 - physicalDamageAbsorptionBody / 100) *
                                              (1 - physicalDamageAbsorptionHands / 100) *
                                              (1 - physicalDamageAbsorptionLegs / 100);

        physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));
        
        float totalFireDamageAbsorption = 1 - 
                                          (1 - fireDamageAbsorptionHead / 100) *
                                          (1 - fireDamageAbsorptionBody / 100) *
                                          (1 - fireDamageAbsorptionLegs / 100) *
                                          (1 - fireDamageAbsorptionHands / 100);

        fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));
        
        float finalDamage = physicalDamage + fireDamage;
        
        currentHealth -= Mathf.RoundToInt(finalDamage);
        

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public virtual void TakeDamageNoAnimation(int physicalDamage,int fireDamage)
    {
        if (isDead) return;

        float totalPhysicalDamageAbsorption = 1 -
                                              (1 - physicalDamageAbsorptionHead / 100) *
                                              (1 - physicalDamageAbsorptionBody / 100) *
                                              (1 - physicalDamageAbsorptionHands / 100) *
                                              (1 - physicalDamageAbsorptionLegs / 100);

        physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));
        
        float totalFireDamageAbsorption = 1 - 
                                          (1 - fireDamageAbsorptionHead / 100) *
                                          (1 - fireDamageAbsorptionBody / 100) *
                                          (1 - fireDamageAbsorptionLegs / 100) *
                                          (1 - fireDamageAbsorptionHands / 100);

        fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));
        
        float finalDamage = physicalDamage + fireDamage;
        
        currentHealth -= Mathf.RoundToInt(finalDamage);
        
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }
    
    /// <summary>
    /// 触发中毒伤害
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakePoisonDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }
    
    /// <summary>
    /// 硬直时间 重置
    /// </summary>
    public virtual void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer -= Time.deltaTime;
        }
        else
        {
            totalPoiseDefence = armorPoiseBonus;
        }
    }
    
    /// <summary>
    /// 恢复血量
    /// </summary>
    /// <param name="healAmount"></param>
    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > 0)
        {
            currentHealth = maxHealth;
        }

        healthBar.SetCurrentHealth(currentHealth);
    }

    public void DrainStaminaBasedOnAttackType()
    {
        
    }
}
