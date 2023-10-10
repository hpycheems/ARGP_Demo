using System;
using UnityEngine;



public class PlayerStatsManager : CharacterStatsManager
{
    //Components
    private PlayerManager playerManager;
    private PlayerAnimatorManager playerAnimatorManager;

    public float staminaRegenTimer = 0;//恢复计时时间
    public float staminaRegenerateAmount = 15;//体力恢复数量
    
    //获取的灵魂数
    public int soulCount = 0;
    
    protected override void Awake()
    {
        base.Awake();
        playerManager = GetComponent<PlayerManager>();

        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        
        healthBar = FindObjectOfType<HealthBar>();
        staminaBar = FindObjectOfType<StaminaBar>();
        focusPointBar = FindObjectOfType<FocusPointBar>();
    }

    private void Start()
    {
        if (GameManager.Instance.haveFile)
        {
            LoadStatsData(GameManager.Instance.gameFileData.statsData);
        }
        else
        {
            LoadStatsData();
        }

        
        
        //初始化 health bar and StaminaBar
        maxHealth = SetMaxHealthFromHealthLevel();
        maxStamina = SetMaxStaminaFromStaminaLevel();
        maxFocusPoints = SetMaxFocusFromFocusLevel();
        
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);

        currentFocusPoints = maxFocusPoints;
        focusPointBar.SetMaxFocusPoints(Mathf.RoundToInt( maxFocusPoints));
    }

    void LoadStatsData(PlayerStateFileData data = null)
    {
        if (data == null)
        {
            return;
        }
        healthLevel = data.healthLevel;
        staminaLevel = data.staminaLevel;
        focusLevel = data.focusLevel;
        poiseLevel = data.poiseLevel;
        strengthLevel = data.strengthLevel;
        dexterityLevel = data.dexterityLevel;
        intelligenceLevel = data.intelligenceLevel;
        faithLevel = data.faithLevel;
    }


    /// <summary>
    /// 超过时间 重置 状态条
    /// </summary>
    public override void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer -= Time.deltaTime;
        }
        else if(poiseResetTimer <= 0 && !playerManager.isInteracting)
        {
            totalPoiseDefence = armorPoiseBonus;
        }
    }

    /// <summary>
    /// 初始化血量
    /// </summary>
    /// <returns></returns>
    int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }
    
    int SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }

    float SetMaxFocusFromFocusLevel()
    {
        maxFocusPoints = focusLevel * 10;
        return maxFocusPoints;
    }

    /// <summary>
    /// 受伤血量扣除
    /// </summary>
    /// <param name="physicalDamage">收到得伤害</param>
    public override void TakeDamage(int physicalDamage, int fireDamge, string damageAnimation)
    {
        
        if (playerManager.isInvulerable)
            return;
        base.TakeDamage(physicalDamage, fireDamge,damageAnimation);
        healthBar.SetCurrentHealth(currentHealth);
        playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);
        
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            playerAnimatorManager.PlayTargetAnimation("Death_01",true);
        }
    }
    
    public override void TakeDamageNoAnimation(int damage, int fireDamage)
    {
        base.TakeDamageNoAnimation(damage,fireDamage);
        if (isDead) return;
        healthBar.SetCurrentHealth(currentHealth);
    }

    public override void TakePoisonDamage(int damage)
    {
        if (isDead) return;

        base.TakePoisonDamage(damage);
        healthBar.SetCurrentHealth(currentHealth);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            playerAnimatorManager.PlayTargetAnimation("Death_01",true);
        }
    }

    //消耗体力
    public void TakeStamina(int damage)
    {
        currentStamina -= damage;
        staminaBar.SetCurrentStamina(currentStamina);
    }
    
    /// <summary>
    /// 恢复体力
    /// </summary>
    public void RegenerateStamina()
    {
        if (playerManager.isInteracting)
        {
            staminaRegenTimer = 0;
        }
        else
        {
            if (currentStamina < maxStamina)
            {
                staminaRegenTimer += Time.deltaTime;
            }

            if (currentStamina < maxStamina && staminaRegenTimer > 1)
            {
                currentStamina += staminaRegenerateAmount * Time.deltaTime;
                staminaBar.SetCurrentStamina(currentStamina);
            }
        }
    }

    /// <summary>
    /// 消耗魔法
    /// </summary>
    /// <param name="focusPoints"></param>
    public void DeductFocusPoints(int focusPoints)
    {
        currentFocusPoints -= focusPoints;
        focusPointBar.SetCurrentFocusPoints(currentFocusPoints);
    }

    /// <summary>
    /// 击败敌人时 获取添加灵魂
    /// </summary>
    /// <param name="soulCount"></param>
    public void AddSouls(int soulCount)
    {
        this.soulCount += soulCount;
    }
    
    public void ResetPlayerStats()
    {
        currentHealth = maxHealth;
        healthBar.SetCurrentHealth(currentHealth);
        currentStamina = maxStamina;
        staminaBar.SetCurrentStamina(currentHealth);
    }


    public PlayerStateFileData ExportData()
    {
        PlayerStateFileData data = new PlayerStateFileData();
        data.healthLevel = this.healthLevel;
        data.staminaLevel = this.staminaLevel;
        data.focusLevel = this.focusLevel;
        data.poiseLevel = this.poiseLevel;
        data.strengthLevel = this.staminaLevel;
        data.dexterityLevel = this.dexterityLevel;
        data.intelligenceLevel = this.intelligenceLevel;
        data.faithLevel = this.faithLevel;
        
        return data;
    }
}
