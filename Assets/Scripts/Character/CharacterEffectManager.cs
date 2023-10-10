using UnityEngine;
using UnityEngine.Serialization;


public class CharacterEffectManager : MonoBehaviour
{
    [Header("组件")]
    public CharacterStatsManager characterStatsManager;
    public Transform buildUpTransform;
    
    [Header("Weapon FX")]
    public WeaponFX rightWeaponFX;
    public WeaponFX leftWeaponFX;

    [Header("Poison")] 
    public GameObject defaultPoisonParticleFX;
    public GameObject currentPoisonParticleFX;
    public bool isPoisoned;
    public float poisonBuildup = 0;
    public float poisonAmount = 100;
    public float defaultPoisonAmount = 100;
    public float poisonTimer = 2;
    public int poisonDamage;
    public float timer = 0;
    
    [Header("Damage FX")]
    public GameObject bloodSplatterFX;

    protected virtual void Awake()
    {
        characterStatsManager = GetComponent<CharacterStatsManager>();
    }

    /// <summary>
    /// 武器攻击 拖尾效果
    /// </summary>
    /// <param name="isLeft"></param>
    public virtual void PlayWeaponFX(bool isLeft)
    {
        if (isLeft)
        {
            leftWeaponFX?.PlayWeaponFX();
        }
        else
        {
            rightWeaponFX?.PlayWeaponFX();
        }
    }
    
    /// <summary>
    /// 受到攻击 效果
    /// </summary>
    /// <param name="bloodSplatterLocation"></param>
    public virtual void PlayBloodSplatterFX(Vector3 bloodSplatterLocation)
    {
        GameObject blood = Instantiate(bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
    }

    /// <summary>
    /// 中毒 FX
    /// </summary>
    public virtual void HandleAllBuildUpEffects()
    {
        if(characterStatsManager.isDead)
            return;
        
        HandlePoisonBuildUp();
        HandleIsPoisonedEffect();
    }

    /// <summary>
    /// 中毒累积计时
    /// </summary>
    public virtual void HandlePoisonBuildUp()
    {
        if (isPoisoned) return;

        if (characterStatsManager.isDead) return;
        
        if (poisonBuildup > 0 && poisonBuildup < 100)
        {
            poisonBuildup -= 1 * Time.deltaTime;
        }
        else if (poisonBuildup >= 100)
        {
            isPoisoned = true;
            poisonBuildup = 0;

            if (buildUpTransform != null)
            {
                currentPoisonParticleFX = 
                    Instantiate(defaultPoisonParticleFX, buildUpTransform.transform);
            }
            else
            {
                currentPoisonParticleFX = Instantiate(defaultPoisonParticleFX, characterStatsManager.transform);
            }
        }
    }
    /// <summary>
    /// 中毒中
    /// </summary>
    public virtual void HandleIsPoisonedEffect()
    {
        if (isPoisoned)
        {
            if (poisonAmount > 0)
            {
                timer += Time.deltaTime;
                if (timer >= poisonTimer)
                {
                    characterStatsManager.TakePoisonDamage(poisonDamage);
                    timer = 0;
                }
                poisonAmount -= 1 * Time.deltaTime;
            }
            else
            {
                isPoisoned = false;
                poisonBuildup = defaultPoisonAmount;
                Destroy(currentPoisonParticleFX);
            }
        }
    }
}
