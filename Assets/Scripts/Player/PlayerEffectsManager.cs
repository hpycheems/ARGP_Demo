using System;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerEffectsManager : CharacterEffectManager
{
    private PlayerWeaponSlotManager playerWeaponSlotManager;
    
    //当前实例化投射出的FX
    public GameObject currentParticleFX;
    //实例化 的FX mode
    public GameObject instantiatedFXModel;

    public int amountToBeHeal;//恢复的血量

    [Header("中毒状态条")]
    public PoisonBuildUpBar poisonBuildUpBar;
    public PoisonAmountBar poisonAmountBar;

    private void Awake()
    {
        base.Awake();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
    }

    private void Start()
    {
        UIManager uiManager = FindObjectOfType<UIManager>();

        poisonBuildUpBar = uiManager.poisonBuildUpBar;
        poisonAmountBar = uiManager.poisonAmountBar;
    }

    /// <summary>
    /// 魔法回血 动画事件回调
    /// </summary>
    public void HealPlayerFromEffect()
    {
        characterStatsManager.HealPlayer(amountToBeHeal);
        GameObject healParticles = Instantiate(currentParticleFX, characterStatsManager.transform);
        Destroy(instantiatedFXModel);
        playerWeaponSlotManager.LoadBothWeaponOnSlots();
        Destroy(healParticles.gameObject, 2);
    }
    
    public override void HandlePoisonBuildUp()
    {
        if (poisonBuildup <= 0)
        {
            poisonBuildUpBar.gameObject.SetActive(false);
        }
        else
        {
            poisonBuildUpBar.gameObject.SetActive(true);
        }
        base.HandlePoisonBuildUp();
        poisonBuildUpBar.SetPoisonBuildUp(Mathf.RoundToInt(poisonBuildup));
    }

    /// <summary>
    /// 中毒 特效
    /// </summary>
    public override void HandleIsPoisonedEffect()
    {
        if (isPoisoned == false)
        {
            poisonAmountBar.gameObject.SetActive(false);
        }
        else
        {
            poisonAmountBar.gameObject.SetActive(true);
        }
        base.HandleIsPoisonedEffect();
        poisonAmountBar.SetPoisonAmount(Mathf.RoundToInt(poisonAmount));
    }
}
