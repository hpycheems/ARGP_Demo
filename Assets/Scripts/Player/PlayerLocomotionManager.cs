using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerLocomotionManager : MonoBehaviour
{
    //Components
    private PlayerManager playerManager;
    private PlayerStatsManager playerStatsManager;
    private CameraHandler cameraHandler;
    private InputHandler inputHandler;
    public Transform cameraObject;//玩家移动依赖与摄像机

    //Stamina Costs
    private int rollStaminaCost = 15;
    private int backstepStaminaCost = 12;
    private int springStaminaCost = 1;

    //Collision 防止 玩家 和 敌人相互 挤压
    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlockerCollider;
    
    public Vector3 moveDirection;//移动方向
    
    [HideInInspector] public Transform myTransform;
    public PlayerAnimatorManager playerAnimatorManager;
    
    public new Rigidbody rigidbody;//刚体
    public GameObject normalCamera;

    [Header("Detection Parameter")]
    private float groundDetectionRayStartPoint = 0.5f;
    private float minimumDistanceNeededToBeginFall = 1f;
    private float groundDirectionRayDistance = 0.1f;
    public LayerMask ignoreForGroundCheck;
    public float inAirTimer;
    
    [Header("Movement Stats")] 
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float sprintSpeed = 7;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private float  walkingSpeed = 3;
    [SerializeField] private float fallingSpeed = 45f;

    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerManager = GetComponent<PlayerManager>();
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        
        
        myTransform = transform;
    }

    private void Start()
    {
        playerAnimatorManager.Initialize();
        
        cameraHandler= FindObjectOfType<CameraHandler>();
        cameraObject = cameraHandler.transform;
        
        playerManager.isGrounded = true;
        ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
        Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider);
    }

    #region Movement

    private Vector3 normalVector;
    private Vector3 targetPosition;
    
    /// <summary>
    /// 玩家旋转
    /// </summary>
    /// <param name="delta"></param>
    public void HandleRotation(float delta)
    {
        if (playerAnimatorManager.canRotate)//使 播放动画前 通过 event 让玩家可以旋转
        {
            if (inputHandler.lockOnFlag)
            {
                if (inputHandler.sprintFlag || inputHandler.rollFlag)
                {
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                    targetDirection += cameraHandler.cameraTransform.right * inputHandler.horizontal;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if (targetDirection == Vector3.zero)
                    {
                        targetDirection = transform.forward;
                    }

                    Quaternion rt = Quaternion.LookRotation(targetDirection);
                    Quaternion targetRotation =
                        Quaternion.Slerp(transform.rotation, rt, rotationSpeed * Time.deltaTime);

                    transform.rotation = targetRotation;
                }
                else
                {
                    Vector3 rotationDirection = moveDirection;
                    rotationDirection = cameraHandler.currentLockOnTarget.transform.position - transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation =
                        Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                    transform.rotation = targetRotation;
                }
            }
            else
            {
                Vector3 targetDir = Vector3.zero;
                float moveOverride = inputHandler.moveAmount;
                //旋转方向 相机的forward
                targetDir = cameraObject.transform.forward * inputHandler.vertical;
                targetDir += cameraObject.transform.right * inputHandler.horizontal;
                targetDir.Normalize();
                targetDir.y = 0;
                if (targetDir == Vector3.zero)
                {
                    //如果没有控制旋转方向，朝玩家forward旋转
                    targetDir = myTransform.forward;
                }

                float rs = rotationSpeed;

                Quaternion tr = Quaternion.LookRotation(targetDir);
                Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);
                myTransform.rotation = targetRotation;
            }
        }
    }
    /// <summary>
    /// 玩家移动
    /// </summary>
    /// <param name="delta"></param>
    public void HandleMovement(float delta)
    {
        if (inputHandler.rollFlag)//翻滚中不能移动
            return;

        if (playerManager.isInteracting)//交互中不能移动
            return;
        
        //计算移动方向
        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = movementSpeed;

        //是否冲刺
        if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f)
        {
            speed = sprintSpeed;
             playerManager.isSprinting = true;
            moveDirection *= speed;
            playerStatsManager.TakeStamina(springStaminaCost);
        }
        else
        {
            //行走
            if (inputHandler.moveAmount < 0.5f)
            {
                moveDirection *= walkingSpeed;
                playerManager.isSprinting = false;
            }
            //跑步
            else
            {
                moveDirection *= speed;
                playerManager.isSprinting = false;
            }
            
        }
        
        //此步骤 不明 其实可以直接 使速度等于 moveDirection
        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;
        
        //更新播放移动动画
        if (inputHandler.lockOnFlag && inputHandler.sprintFlag == false)
        {
            playerAnimatorManager.UpdateAnimatorValues(inputHandler.vertical, inputHandler.horizontal, playerManager.isSprinting);
        }
        else
        {
            playerAnimatorManager.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);
        }
        
        //更新动画
        //animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);
        
    }

    /// <summary>
    /// 玩家翻滚 和冲刺动画更新
    /// </summary>
    /// <param name="delta"></param>
    public void HandleRollingAndSprinting(float delta)
    {
        if (playerAnimatorManager.anim.GetBool("isInteracting"))
        {
            inputHandler.rollFlag = false;
            return;
        }

        if (playerStatsManager.currentStamina <= 0){
            inputHandler.rollFlag = false;
            return;//没有体力时停止
        }

        if (inputHandler.rollFlag)
        {
            inputHandler.rollFlag = false;
            //计算翻滚方向
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;

            if (inputHandler.moveAmount > 0)
            {
                playerAnimatorManager.PlayTargetAnimation("Rolling", true);
                playerAnimatorManager.EraseHandIKForWeapon();
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
                playerStatsManager.TakeStamina(rollStaminaCost);
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation("BackStep", true);
                playerAnimatorManager.EraseHandIKForWeapon();
                playerStatsManager.TakeStamina(backstepStaminaCost);
            }
        }
    }

    /// <summary>
    /// 玩家掉落
    /// </summary>
    /// <param name="delta"></param>
    /// <param name="moveDirection"></param>
    public void HandleFalling(float delta, Vector3 moveDirection)
    {
        //当玩家掉落 isground 为false
        playerManager.isGrounded = false;
        
        RaycastHit hit;
        //计算射线发射原点
        Vector3 origin = myTransform.position;
        //射线比脚底高一些 依此来走上阶梯
        origin.y += groundDetectionRayStartPoint;

        //检测到前方 有走不上的阶梯 则停止
        if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
        {
            moveDirection = Vector3.zero;
        }

        //当玩家在空中 则掉落
        if (playerManager.isInAir)
        {
            rigidbody.AddForce(-Vector3.up * fallingSpeed);
            rigidbody.AddForce(moveDirection * fallingSpeed / 10);
        }

        //计算 检测掉落 origin 在玩家移动方向 半径周围 检测
        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * groundDirectionRayDistance;

        targetPosition = myTransform.position;

        Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, 
            Color.red, 0.1f, false);
        
        //检测地面
        if (Physics.Raycast(origin, -Vector3.up, 
                out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
        {
            normalVector = hit.normal;
            //碰撞到的点
            Vector3 tp = hit.point;
            playerManager.isGrounded = true;
            //使玩家不要陷入地面
            targetPosition.y = tp.y;

            //如果是从 空中掉落下来的 
            if (playerManager.isInAir)
            {
                //在空中的时间超过 0.5f 则 播放不同的动画
                if (inAirTimer > 0.5f)
                {
                    Debug.Log("You were in the air for"+ inAirTimer);
                    playerAnimatorManager.PlayTargetAnimation("Land", true);
                    inAirTimer = 0;
                }
                else
                {
                    playerAnimatorManager.PlayTargetAnimation("Land", false);
                    inAirTimer = 0;
                }
            }
            
            playerManager.isInAir = false;
        }
        else//检测不到地面时
        {
            if (playerManager.isGrounded)
            {
                playerManager.isGrounded = false;
            }
            
            if (playerManager.isInAir == false)
            {
                if (playerManager.isInteracting == false)
                {
                   playerAnimatorManager.PlayTargetAnimation("Falling", true); 
                }
                
                //玩家掉落
                Vector3 vel = rigidbody.velocity;
                vel.Normalize();
                rigidbody.velocity = vel * (movementSpeed / 2);
                playerManager.isInAir = true;
            }
        }

        if (playerManager.isInteracting || inputHandler.moveAmount > 0)
        {
            //Debug.Log(targetPosition);
            myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime);
        }
        else
        {
            myTransform.position = targetPosition;
        }
        
    }

    /// <summary>
    /// 玩家跳跃
    /// </summary>
    public void HandleJumping()
    {
        if (playerManager.isInteracting)
            return;

        if (inputHandler.jump_input)
        {
            if (inputHandler.moveAmount > 0)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;
                playerAnimatorManager.PlayTargetAnimation("Jump", true);
                playerAnimatorManager.EraseHandIKForWeapon();
                moveDirection.y = 0;
                Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = jumpRotation;
            }
        }
    }

    public void EnableSetTransformPosition()
    {
        rigidbody.interpolation = RigidbodyInterpolation.None;
    }
    public void DisableSetTransformPosition()
    {
        rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    #endregion
}
