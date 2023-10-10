using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    // Components
    private Animator anim;
    public InputHandler inputHandler;
    private CameraHandler cameraHandler;
    private InteractableUI interactableUI;
    public PlayerEquipmentManager playerEquipmentManager;
    public PlayerWeaponSlotManager playerWeaponSlotManager;
    public PlayerCombatManager playerCombatManager;
    public PlayerInventoryManager playerInventoryManager;
    public PlayerStatsManager playerStatsManager;
    public PlayerEffectsManager playerEffectsManager;
    public PlayerLocomotionManager playerLocomotionManager;
    public PlayerAnimatorManager playerAnimatorManager;
    
    [HideInInspector] public GameObject interactableGameObject;
    [HideInInspector] public GameObject itemInteractableGameObject;
    
    protected override void Awake()
    {
        base.Awake();
        
        DontDestroyOnLoad(gameObject);
        
        inputHandler = GetComponent<InputHandler>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerWeaponSlotManager= GetComponent<PlayerWeaponSlotManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            
        anim = GetComponentInChildren<Animator>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();

        interactableUI = FindObjectOfType<InteractableUI>();
        LoadData();
    }
    
    private void Start()
    {
        //获得摄像机控制脚本
        cameraHandler = CameraHandler.singleton;
        UIManager uiManager = FindObjectOfType<UIManager>();
        interactableGameObject = uiManager.interactableOpoUp;
        itemInteractableGameObject = uiManager.itemInteractableOpoUp;
    }

    private void LoadData()
    {
        if (GameManager.Instance.haveFile)
        {
            Debug.Log("加载存档！！");
            transform.rotation = GameManager.Instance.gameFileData.rotation;
            transform.position = GameManager.Instance.gameFileData.position;
        }
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        isInteracting = anim.GetBool("isInteracting");
        canDoCombo = anim.GetBool("canDoCombo");
        isInvulerable = anim.GetBool("isInvulerable");
        isFiringSpell = anim.GetBool("isFiringSpell");
        anim.SetBool("isTwoHandWeapon", isTwoHandingWeapon);
        anim.SetBool("isInAir", isInAir);
        anim.SetBool("isDead", playerStatsManager.isDead);
        anim.SetBool("isBlocking", isBlocking);
        
        
        inputHandler.TickInput(delta);
        playerAnimatorManager.canRotate = anim.GetBool("canRotate");
        playerLocomotionManager.HandleRollingAndSprinting(delta);
        
        //playerLocomotion.HandleJumping();
        playerStatsManager.RegenerateStamina();

        CheckForInteractableObject();
    }
    
    protected override void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        playerLocomotionManager.HandleMovement(delta);
        playerLocomotionManager.HandleFalling(delta, playerLocomotionManager.moveDirection);
        playerLocomotionManager.HandleRotation(delta);
        
        playerEffectsManager.HandleAllBuildUpEffects();
    }

    void LateUpdate()
    {
        inputHandler.d_pda_Right = false;
        inputHandler.d_pda_Left = false;
        inputHandler.d_pda_Up = false;
        inputHandler.d_pda_Down = false;
        inputHandler.a_input = false;
        inputHandler.inventory_input = false;

        float delta = Time.deltaTime;
        
        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
        }
        
        if (isInAir)
        {
            playerLocomotionManager.inAirTimer += Time.deltaTime;
        }
    }

    #region Player Interactions

    /// <summary>
    /// 物品检测 
    /// </summary>
    void CheckForInteractableObject()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.3f, transform.forward
                , out hit, 1f, cameraHandler.ignoreLayers))
        {
            if (hit.collider.tag == "Interactable" || hit.collider.tag == "NPC" || hit.collider.tag == "Bonfire")
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();
                if (interactableObject != null)
                {
                    string interactableText = interactableObject.interactableText;
                    //TODO: 显示UI
                    interactableUI.interactableText.text = interactableText;
                    interactableGameObject.SetActive(true);
                    if (inputHandler.a_input)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
        }
        else
        {
            if (interactableGameObject != null)
            {
                interactableGameObject.SetActive(false);
            }

            if (itemInteractableGameObject != null && inputHandler.a_input)
            {
                itemInteractableGameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 打开宝箱时 设置位置和播放动画 
    /// </summary>
    /// <param name="playerStandsHereWhenOpeningChest"></param>
    public void OpenChestInteraction(Transform playerStandsHereWhenOpeningChest)
    {
        playerLocomotionManager.rigidbody.velocity = Vector3.zero;
        playerLocomotionManager.EnableSetTransformPosition();
        transform.position = playerStandsHereWhenOpeningChest.position;
        playerAnimatorManager.PlayTargetAnimation("Open Chest", true);
    }

    public void EnableSetPosition()
    {
        playerLocomotionManager.EnableSetTransformPosition();
    }
    
    public void DisableSetPosition()
    {
        playerLocomotionManager.DisableSetTransformPosition();
    }

    #endregion
    
    public void ResetPlayerStats()
    {
        playerStatsManager.ResetPlayerStats();
    }

    public void ResetPoisonState()
    {
        playerEffectsManager.poisonBuildup = 0;
        playerEffectsManager.poisonAmount = 100;
        playerEffectsManager.isPoisoned = false;
        if (playerEffectsManager.currentPoisonParticleFX != null)
        {
            Destroy(playerEffectsManager.currentPoisonParticleFX);
        }
    }
    
}
