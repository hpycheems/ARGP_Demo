using UnityEngine;

/// <summary>
/// 敌人状态
/// </summary>
public class EnemyStatsManager : CharacterStatsManager
{
    //Components
    private UIEnemyHealthBar enemyHealthBar;
    private CharacterAnimatorManager enemyCharacterAnimatorManager;

    /// <summary>
    /// 死亡后 玩家获得的灵魂
    /// </summary>
    public int soulsAwardedOnDeath = 50;

    /// <summary>
    /// 是否是Boss
    /// </summary>
    public bool isBoss;
    
    protected override void Awake()
    {
        base.Awake();
        enemyHealthBar = FindObjectOfType<UIEnemyHealthBar>();
        
        enemyCharacterAnimatorManager = GetComponentInChildren<CharacterAnimatorManager>();
    }

    private void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        
        enemyHealthBar.SetMaxHealth(maxHealth);
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
    
    /// <summary>
    /// 受伤血量扣除
    /// </summary>
    /// <param name="damage">收到得伤害</param>
    public override void TakeDamage(int physicalDamage,int fireDamage, string damageAnimation)
    {
        base.TakeDamage(physicalDamage,fireDamage, damageAnimation);
        enemyHealthBar.SetHealth(currentHealth);
        enemyCharacterAnimatorManager.PlayTargetAnimation(damageAnimation,true);
        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }
    /// <summary>
    /// 不使用 死亡动画的 受伤方法
    /// 用于背刺 或 反击
    /// </summary>
    /// <param name="damage"></param>
    public override void TakeDamageNoAnimation(int damage, int fireDamage)
    {
        base.TakeDamageNoAnimation(damage,fireDamage);
        enemyHealthBar.SetHealth(currentHealth);
    }
    
    public override void TakePoisonDamage(int damage)
    {
        if (isDead) return;

        base.TakePoisonDamage(damage);
        if (isBoss)
        {
            
        }
        else
        {
            healthBar.SetCurrentHealth(currentHealth);
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            enemyCharacterAnimatorManager.PlayTargetAnimation("Death_01",true);
        }
    }

    public void BreakGuard()
    {
        enemyCharacterAnimatorManager.PlayTargetAnimation("Break Guard",true);
    }

    /// <summary>
    /// 死亡 回调
    /// </summary>
    void HandleDeath()
    {
        currentHealth = 0;
        enemyCharacterAnimatorManager.PlayTargetAnimation("Death_01",true);
        isDead = true;
    }
}
