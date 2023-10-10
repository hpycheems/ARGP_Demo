using System;
using UnityEngine;
using UnityEngine.Serialization;

public class InputHandler : MonoBehaviour
{
    private PlayerAnimatorManager playerAnimatorManager;
    private PlayerEffectsManager playerEffectsManager;
    private BlockingCollider blockingCollider;
    private PlayerWeaponSlotManager playerWeaponSlotManager;
    private PlayerControls inputActions;
    private PlayerCombatManager playerCombatManager;
    private PlayerInventoryManager playerInventoryManager;
    private PlayerManager playerManager;
    private CameraHandler cameraHandler;
    private PlayerStatsManager playerStatsManager;
    private UIManager uiManager;

    public Transform criticalAttackRayCastStarPoint;
    
    [Header("Movement And Viewable Signel")]
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;
    public float rollInputTimer;

    [Header("Player Flags")]
    public bool rollFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public bool twoHandFlag;

    [Header("Input Button")] 
    public bool y_input;
    public bool b_Input;
    public bool a_input;
    public bool x_input;
    
    public bool tap_rb_Input;
    public bool hold_rb_Input;
    public bool tap_rt_Input;
    public bool hold_rt_Input;
    public bool tap_lb_Input;
    public bool hold_lb_Input;
    public bool tap_lt_Input;
    public bool hold_lt_Input;
    
    public bool d_pda_Up;
    public bool d_pda_Down;
    public bool d_pda_Right;
    public bool d_pda_Left;
    public bool lockOnInput;
    public bool jump_input;
   
    public bool inventory_input;
    
    //public bool critical_attack_Input;
    public bool right_Stick_Right_Input;
    public bool right_Stick_Left_Input;

    public bool inventoryFlag;
    public bool lockOnFlag;
    
    private Vector2 movementInput;
    private Vector2 cameraInput;

    private void Awake()
    {
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerManager = GetComponent<PlayerManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        
        blockingCollider = GetComponentInChildren<BlockingCollider>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        cameraHandler = CameraHandler.singleton;
    }

    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            
            inputActions.PlayerActions.TapRB.performed += i => tap_rb_Input = true;
            inputActions.PlayerActions.HoldRB.performed += i => hold_rb_Input = true;
            inputActions.PlayerActions.HoldRB.canceled += i => hold_rb_Input = false;
            inputActions.PlayerActions.TapRT.performed += i => tap_rt_Input = true;
            inputActions.PlayerActions.HoldRT.performed += i => hold_rt_Input = true;
            inputActions.PlayerActions.HoldRT.canceled += i => hold_rt_Input = false;
            
            inputActions.PlayerActions.TapLB.canceled += i => tap_lb_Input = false;
            inputActions.PlayerActions.HoldLB.performed += i => hold_lb_Input = true;
            inputActions.PlayerActions.HoldLB.canceled += i => hold_lb_Input = false;
            inputActions.PlayerActions.TapLT.performed += i => tap_lt_Input = true;
            inputActions.PlayerActions.HoldLT.performed += i => hold_lt_Input = true;
            inputActions.PlayerActions.HoldLT.canceled += i => hold_lt_Input = false;
            
            inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
            inputActions.PlayerActions.Roll.performed += i => b_Input = true;
            
            inputActions.PlayerQuickSlots.DPad_Up.performed += i => d_pda_Up = true;
            inputActions.PlayerQuickSlots.DPad_Dowm.performed += i => d_pda_Down = true;
            inputActions.PlayerQuickSlots.DPad_Right.performed += i => d_pda_Right = true;
            inputActions.PlayerQuickSlots.DPad_Left.performed += i => d_pda_Left = true;
            
            inputActions.PlayerActions.InventoryButton.performed += i => inventory_input = true;
            inputActions.PlayerActions.A.performed += i => a_input = true;
            inputActions.PlayerActions.X.performed += i => x_input = true;
            inputActions.PlayerActions.Y.performed += i => y_input = true;
            inputActions.PlayerActions.Jump.performed += i => jump_input = true;
            
            inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
            inputActions.PlayerMovement.LockOnLeft.performed += i => right_Stick_Left_Input = true;
            inputActions.PlayerMovement.LockOnRight.performed += i => right_Stick_Right_Input = true;
        }
        inputActions.Enable();
    }

    public void DisableInput()
    {
        inputActions.Disable();
    }

    public void EnableInput()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    /// <summary>
    /// 输入更新
    /// </summary>
    /// <param name="delta"></param>
    public void TickInput(float delta)
    {
        if (playerStatsManager.isDead)
            return;
        
        InputPerform();
        
        HandleMovementInput(delta);
        HandleRollInput(delta);
 
        HandleQuickSlotsInput();
        HandleInventoryInput();
        HandleLockOnInput();
        HandleTwoHandInput();
        HandleConsumableInput();
        
        HandleTapRBInput(delta);
        HandleTapRTInput();
        HandleTapLTInput();
        HandleTapLBInput();
        
        HandleHoldRBInput();
        HandleHoldLBInput();
    }
    void InputPerform()
    {
        movementInput = inputActions.PlayerMovement.Movement.ReadValue<Vector2>();
        cameraInput = inputActions.PlayerMovement.Camera.ReadValue<Vector2>();
    }

    /// <summary>
    /// 移动信息输入
    /// </summary>
    /// <param name="delta"></param>
    void HandleMovementInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
    }
    
    /// <summary>
    /// 翻滚输入检测
    /// </summary>
    /// <param name="delta"></param>
    void HandleRollInput(float delta)
    {
        sprintFlag = b_Input;
        
        if (b_Input)
        {
            rollInputTimer += delta;

            if (playerStatsManager.currentStamina <= 0)//没有体力时
            {
                b_Input = false;
                sprintFlag = false;
            }

            if (moveAmount > 0.5f && playerStatsManager.currentStamina > 0)//奔跑
            {
                sprintFlag = true;
            }
        }
        else//翻滚
        {
            sprintFlag = false;
            if (rollInputTimer > 0 && rollInputTimer < 0.25f)
            {
                rollFlag = true;
            }

            rollInputTimer = 0;
        }
    }

    /// <summary>
    /// 攻击输入检测
    /// </summary>
    /// <param name="delta"></param>
    void HandleTapRBInput(float delta)
    {
        //右 轻功击
        if (tap_rb_Input)
        {
            tap_rb_Input = false;
            playerManager.UpdateWhichHandCharacterIsUsing(false);
            playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
            playerInventoryManager.rightWeapon.tap_RB_Action.PerformAction(playerManager);
        }
    }
    void HandleTapRTInput()
    {
        //右  重攻击
        if (tap_rt_Input)
        {
            tap_rt_Input = false;
            playerManager.UpdateWhichHandCharacterIsUsing(false);
            playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
            playerInventoryManager.rightWeapon.tap_RT_Action.PerformAction(playerManager);
        }
    }
    void HandleTapLTInput()
    {
        //左 盾反/攻击
        if (tap_lt_Input)
        {
            tap_lt_Input = false;
            if (playerManager.isTwoHandingWeapon)
            {
                playerManager.UpdateWhichHandCharacterIsUsing(false);
                playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                playerInventoryManager.rightWeapon.tap_LT_Action.PerformAction(playerManager);
            }
            else
            {
                playerManager.UpdateWhichHandCharacterIsUsing(true);
                playerInventoryManager.currentItemBeingUsed = playerInventoryManager.leftWeapon;
                playerInventoryManager.leftWeapon.tap_LT_Action.PerformAction(playerManager);
            }
        }
    }
    void HandleTapLBInput()
    {
        if (tap_lb_Input)
        {
            tap_lb_Input = false;
            if (playerManager.isTwoHandingWeapon)
            {
                playerManager.UpdateWhichHandCharacterIsUsing(false);
                playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                playerInventoryManager.rightWeapon.tap_LB_Action.PerformAction(playerManager);
            }
            else
            {
                playerManager.UpdateWhichHandCharacterIsUsing(true);
                playerInventoryManager.currentItemBeingUsed = playerInventoryManager.leftWeapon;
                playerInventoryManager.leftWeapon.tap_LB_Action.PerformAction(playerManager);
            }
        }
    }

    
    void HandleHoldLBInput()
    {
        if (playerManager.isInAir ||
            playerManager.isSprinting ||
            playerManager.isFiringSpell)
        {
            hold_lb_Input = false;
            return;
        }
        
        //左 举盾
        if (hold_lb_Input)
        {
            if (playerManager.isTwoHandingWeapon)
            {
                playerManager.UpdateWhichHandCharacterIsUsing(false);
                playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                playerInventoryManager.rightWeapon.hold_LB_Action.PerformAction(playerManager);
            }
            else
            {
                playerManager.UpdateWhichHandCharacterIsUsing(true);
                playerInventoryManager.currentItemBeingUsed = playerInventoryManager.leftWeapon;
                playerInventoryManager.leftWeapon.hold_LB_Action.PerformAction(playerManager);
            }
        }
        else if(hold_lb_Input == false)
        {
            //弓箭
            
            //盾
            if (blockingCollider.blockingCollider.enabled)//放下盾牌 解除碰撞
            {
                playerManager.isBlocking = false;
                blockingCollider.DisableBlockingCollider();
            }
        }
    }
    void HandleHoldRBInput()
    {
        if (hold_rb_Input)
        {
            playerManager.UpdateWhichHandCharacterIsUsing(false);
            playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
            playerInventoryManager.rightWeapon.hold_RB_Action.PerformAction(playerManager);
        }
    }
    void HandleHoldLTInput()
    {
        
    }
    void HandleHoldRTInput()
    {
        
    }
    
    
    /// <summary>
    /// 快捷栏 输入检测
    /// </summary>
    void HandleQuickSlotsInput()
    {
        if (d_pda_Right)
        {
            playerInventoryManager.ChangeRightWeapon();
        }
        
        if (d_pda_Left)
        {
            playerInventoryManager.ChangeLeftWeapon();
        }

        if (d_pda_Down)
        {
            playerInventoryManager.ChangeConsumable();
        }
    }

    /// <summary>
    /// 工具栏输入检测
    /// </summary>
    void HandleInventoryInput()
    {
        if (inventory_input)//开启/关闭 设置面板
        {
            inventoryFlag = !inventoryFlag;
            if (inventoryFlag)//开启
            {
                uiManager.OpenSelectWindow();
            }
            else//关闭
            {
                uiManager.CloseSelectWindow();
                uiManager.CloseAllWindow();
            }
        }
    }

    /// <summary>
    /// LockOn输入检测
    /// </summary>
    void HandleLockOnInput()
    {
        if (lockOnInput && lockOnFlag == false)//如果没有镜头锁定
        {
            lockOnInput = false;
            cameraHandler.HandleLockOn();
            if (cameraHandler.nearestLockOnTarget != null)//可进行锁定 则锁定
            {
                cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                lockOnFlag = true;
            }
        }
        else if (lockOnInput && lockOnFlag)//解除锁定
        {
            lockOnInput = false;
            lockOnFlag = false;
            cameraHandler.ClearLockOnTargets();
        }
        
        if (lockOnFlag && right_Stick_Right_Input)//切换向右锁定的敌人
        {
            right_Stick_Right_Input = false;
            cameraHandler.HandleLockOn();
            if (cameraHandler.leftLockTarget != null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
            }
        }

        if (lockOnFlag && right_Stick_Left_Input)//切换向左锁定的敌人
        {
            right_Stick_Left_Input = false;
            cameraHandler.HandleLockOn();
            if (cameraHandler.rightLockTarget != null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
            }
        }
        
        cameraHandler.SetCameraHeight();//锁定和解除锁定时 设置视角高度
    }

    /// <summary>
    /// 使用双手系统 输入检测
    /// </summary>
    void HandleTwoHandInput()
    {
        if (y_input)
        {
            y_input = false;
            
            twoHandFlag = !twoHandFlag;
            if (twoHandFlag)
            {
                playerManager.isTwoHandingWeapon = true;
                playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                playerWeaponSlotManager.LoadTwoHandIKTargets(true);
            }
            else
            {
                playerManager.isTwoHandingWeapon = false;
                playerWeaponSlotManager.LoadBothWeaponOnSlots();
                playerWeaponSlotManager.LoadTwoHandIKTargets(false);
            }
        }
    }
    
    /// <summary>
    /// 使用道具输入检测
    /// </summary>
    void HandleConsumableInput()
    {
        if (x_input)
        {
            x_input = false;
            playerInventoryManager.currentConsumableItem.AttemptToConsumeItem(playerAnimatorManager, playerWeaponSlotManager,
                playerEffectsManager);
        }
    }
}
