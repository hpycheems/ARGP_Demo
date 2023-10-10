using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    //Components
    public static CameraHandler singleton;
    private InputHandler inputHandler;
    private PlayerManager playerManager;
    
    /// <summary>
    /// 摄像机跟随目标
    /// </summary>
    public Transform targetTransform;
    /// <summary>
    /// 摄像机
    /// </summary>
    public Transform cameraTransform;
    /// <summary>
    /// 摄像机旋转轴
    /// </summary>
    public Transform cameraPivotTransform;
    private Transform myTransform;//Camera Handler
    
    /// <summary>
    /// 碰撞忽略Layer
    /// </summary>
    public LayerMask ignoreLayers;
    /// <summary>
    /// 环境层 不能透过环境 锁定敌人
    /// </summary>
    public LayerMask environmentLayer;
    private Vector3 cameraTransformPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    
    //左右旋转速度
    public float lookSpeed = 0.1f;
    //跟随速度
    public float followSpeed = 0.1f;
    //上下旋转速度
    public float pivotSpeed = 0.03f;

    private float targetPosition;
    private float defaultPostion;
    private float lookAngle;
    private float pivotAngle;
    public float minimumPivot = -35;
    public float maximumPivot = 35;

    //摄像机碰撞参数
    public float cameraSphereRadius =.2f;
    public float cameraCollisionOffset =.2f;
    public float minimumCollisionOffset =.2f;

    //Lock On Parameter
    private List<CharacterManager> availbleTargets = new List<CharacterManager>();
    public float maximumLockOnDistance = 30;
    public float lockedPivotPosition = 2.25f;
    public float unlockedPivotPosition = 1.65f;
    [Header("Lock On Target And Left Right")]
    public CharacterManager nearestLockOnTarget;
    public CharacterManager currentLockOnTarget;
    public CharacterManager leftLockTarget;
    public CharacterManager rightLockTarget;

    #region Unity Callback

    private void Awake()
    {
        singleton = this;
        myTransform = transform;
        defaultPostion = cameraTransform.localPosition.z;
        ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10 | 1 << 12 | 1 << 13 | 1 << 14);

        playerManager = FindObjectOfType<PlayerManager>();
    }
    private void Start()
    {
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        inputHandler = FindObjectOfType<InputHandler>();
        environmentLayer = LayerMask.NameToLayer("Environment");
    }

    #endregion
    

    /// <summary>
    /// 摄像机跟随目标
    /// </summary>
    /// <param name="delta"></param>
    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position,
            ref cameraFollowVelocity, delta / followSpeed);
        myTransform.position = targetPosition;

        HandleCameraCollisions(delta);
    }
    
    /// <summary>
    /// 摄像机碰撞
    /// </summary>
    /// <param name="delta"></param>
    private void HandleCameraCollisions(float delta)
    {
        if (inputHandler.lockOnFlag) return;
        targetPosition = defaultPostion;
        RaycastHit hit;
        //获得pivot 到 摄像机的方向
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        //给摄像机方向发射一个 Sphere碰撞体并检测
        if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, 
                direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
        {
            //如果它们之间有障碍物体， 则计算出pivot与障碍物之间的距离 
            float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
            //并减去 半径
            targetPosition = -(dis - cameraCollisionOffset);
        }

        //如果 比 最小限度的半径距离 则让其等于最小限度
        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = -minimumCollisionOffset;
        }

        //targetPosition 一直为负数 因为 摄像机为pivot 子物体 
        cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, 
            targetPosition, delta / 0.2f);
        cameraTransform.localPosition = cameraTransformPosition;
    }
    
    /// <summary>
    /// 摄像机旋转
    /// </summary>
    /// <param name="delta"></param>
    /// <param name="mouseXInput"></param>
    /// <param name="mouseYInput"></param>
    public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
    {
        if (inputHandler.lockOnFlag == false)
        {
            //水平旋转
            lookAngle += mouseXInput * lookSpeed * delta;
            //竖直旋转
            pivotAngle -= mouseYInput * pivotSpeed * delta;
            //限制竖直旋转
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

            //左右旋转
            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTransform.rotation = targetRotation;

            //上下旋转
            rotation = Vector3.zero;
            rotation.x = pivotAngle;
            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }
        else// 当锁定视角时， 
        {
            float velocity = 0;
            Vector3 dir = currentLockOnTarget.transform.position - transform.position;
            dir.Normalize();
            dir.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = targetRotation;

            dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
            dir.Normalize();

            targetRotation = Quaternion.LookRotation(dir);
            Vector3 eulerAngle = targetRotation.eulerAngles;
            eulerAngle.y = 0;
            cameraPivotTransform.localEulerAngles = eulerAngle;
        }
    }

    /// <summary>
    /// 锁定 敌人
    /// </summary>
    public void HandleLockOn()
    {
        float shortestDistance = Mathf.Infinity;
        float shortestDistanceOfLeftTarget = -Mathf.Infinity;
        float shortestDistanceOfRightTarget = Mathf.Infinity;
        
        //检测 玩家forward 半径为26米敌人
        Collider[] colliders = Physics.OverlapSphere(targetTransform.forward, 26);
        for (int i = 0; i < colliders.Length; i++)//遍历
        {
            CharacterManager character = colliders[i].GetComponent<CharacterManager>();
            if (character != null)//如果时 敌人
            {
                //计算方向
                Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                //计算距离
                float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                //计算角度
                float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                
                RaycastHit hit;

                if (character.transform.root != targetTransform.transform.root && viewableAngle > -50 &&
                    viewableAngle < 50 && distanceFromTarget <= maximumLockOnDistance)//如果在玩家的前方左右100度之内
                {
                    if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position,
                            out hit))
                    {
                        Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);
                        if (hit.transform.gameObject.layer == environmentLayer)
                        {
                            //如果是 环境物体 则 无反应
                        }
                        else
                        {
                            availbleTargets.Add(character);//添加到锁定列表
                        }
                    }
                }
            }
        }
        
        for (int j = 0; j < availbleTargets.Count; j++)//遍历锁定列表
        {
            //计算距离
            float distanceFormTarget =
                Vector3.Distance(targetTransform.position, availbleTargets[j].transform.position);
            //获得距离最近的敌人
            if (distanceFormTarget < shortestDistance)
            {
                shortestDistance = distanceFormTarget;
                nearestLockOnTarget = availbleTargets[j];
            }

            if (inputHandler.lockOnFlag)
            {
                Vector3 relativeEnemyPosition =
                    inputHandler.transform.InverseTransformPoint(availbleTargets[j].transform.position);
                var distanceFromLeftTarget = relativeEnemyPosition.x;
                var distanceFromRightTarget = relativeEnemyPosition.x;
                
                if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget && 
                    availbleTargets[j] != currentLockOnTarget)
                {
                    shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                    leftLockTarget = availbleTargets[j];
                }
                else if (relativeEnemyPosition.x >= 0.00 &&  distanceFromRightTarget < shortestDistanceOfRightTarget && 
                    availbleTargets[j] != currentLockOnTarget)
                {
                    shortestDistanceOfRightTarget = distanceFromRightTarget;
                    rightLockTarget = availbleTargets[j];
                }
            }
        }
    }
    
    /// <summary>
    /// 清空 上次检测到的视角锁定目标
    /// </summary>
    public void ClearLockOnTargets()
    {
        availbleTargets.Clear();
        nearestLockOnTarget = null;
        currentLockOnTarget = null;
        leftLockTarget = null;
        rightLockTarget = null;
    }
    
    /// <summary>
    /// 设置 视角锁定时的视角高度
    /// </summary>
    public void SetCameraHeight()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
        Vector3 newUnLockedPosition = new Vector3(0, unlockedPivotPosition);

        if (currentLockOnTarget != null)
        {
            cameraPivotTransform.transform.localPosition =
                Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition,ref velocity, Time.deltaTime);
        }
        else
        {
            cameraPivotTransform.transform.localPosition =
                Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnLockedPosition,ref velocity, Time.deltaTime);
        }
    }
}
