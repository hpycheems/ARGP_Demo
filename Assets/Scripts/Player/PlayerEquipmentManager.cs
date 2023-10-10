using System;
using UnityEngine;


/// <summary>
/// 为举盾服务
/// </summary>
public class PlayerEquipmentManager : MonoBehaviour
{
    //Components
    private InputHandler inputHandler;
    private PlayerInventoryManager playerInventoryManager;
    private PlayerStatsManager playerStatsManager;

    //Helmet
    private HelmetModelChanger helmetModelChanger;
    //Torso
    private TorsoModelChanger torsoModelChanger;
    private UpperRightArmModelChanger upperRightArmModelChanger;
    private UpperLeftArmModelChanger upperLeftArmModelChanger;
    //Leg 
    private HipModelChanger hipModelChanger;
    private RightLegModelChanger rightLegModelChanger;
    private LeftLegModelChanger leftLegModelChanger;
    //Hand
    private LowerRightArmModelChanger lowerRightArmModelChanger;
    private LowerLeftArmModelChanger lowerLeftArmModelChanger;
    private RightHandModelChanger rightHandModelChanger;
    private LeftHandModelChanger leftHandModelChanger;
    
    //防御 collider 脚本
    public BlockingCollider blockingCollider;
    
    //默认皮肤
    //helmet
    public GameObject nakedHeadModel;
    //Torso
    public string nakedTorsoModel;
    public string nakedUpperRightArmModel;
    public string nakedUpperLeftArmModel;
    //Leg
    public string nakedHipModel;
    public string nakedRightLegModel;
    public string nakedLeftLegModel;
    //hand
    public string nakedLowerRightArmModel;
    public string nakedLowerLeftArmModel;
    public string nakedRightHandModel;
    public string nakedLeftHandModel;

    private void Awake()
    {
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        inputHandler = GetComponent<InputHandler>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        
        //helmet
        helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
        //Torso
        torsoModelChanger= GetComponentInChildren<TorsoModelChanger>();
        upperRightArmModelChanger = GetComponentInChildren<UpperRightArmModelChanger>();
        upperLeftArmModelChanger= GetComponentInChildren<UpperLeftArmModelChanger>();
        //Leg
        hipModelChanger = GetComponentInChildren<HipModelChanger>();
        rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
        leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
        //hand
        rightHandModelChanger =  GetComponentInChildren<RightHandModelChanger>();
        leftHandModelChanger= GetComponentInChildren<LeftHandModelChanger>();
        lowerRightArmModelChanger = GetComponentInChildren<LowerRightArmModelChanger>();
        lowerLeftArmModelChanger= GetComponentInChildren<LowerLeftArmModelChanger>();
    }

    //private void Start()
    //{
    //    EquipAllEquipmentModelsOnStart();
    //}
    
    public void EquipAllEquipmentModelsOnStart()
    {
        //Helmet
        ChangeHelmet();

        //Torso
        ChangeTorso();

        //Leg
        ChangeHip();

        //Hand
        ChangeHand();
    }

    public void ChangeHand()
    {
        lowerRightArmModelChanger.UnEquipAllLowerArmModels();
        lowerLeftArmModelChanger.UnEquipAllLowerArmModels();
        rightHandModelChanger.UnEquipAllHandModels();
        leftHandModelChanger.UnEquipAllHandModels();
        if (playerInventoryManager.currentHandEquipment != null)
        {
            lowerRightArmModelChanger.EquipmentLowerArmModelByName(playerInventoryManager.currentHandEquipment
                .lowerRightArmModelName);
            lowerLeftArmModelChanger.EquipmentLowerArmModelByName(playerInventoryManager.currentHandEquipment
                .lowerLeftArmModelName);
            rightHandModelChanger.EquipmentHandModelByName(playerInventoryManager.currentHandEquipment.rightHandModelName);
            leftHandModelChanger.EquipmentHandModelByName(playerInventoryManager.currentHandEquipment.leftHandModelName);
            playerStatsManager.physicalDamageAbsorptionHands = playerInventoryManager.currentHandEquipment.physicalDefense;
        }
        else
        {
            lowerRightArmModelChanger.EquipmentLowerArmModelByName(nakedLowerRightArmModel);
            lowerLeftArmModelChanger.EquipmentLowerArmModelByName(nakedLowerLeftArmModel);
            rightHandModelChanger.EquipmentHandModelByName(nakedRightHandModel);
            leftHandModelChanger.EquipmentHandModelByName(nakedLeftHandModel);
            playerStatsManager.physicalDamageAbsorptionHands = 0;
        }
    }

    public void ChangeHip()
    {
        hipModelChanger.UnEquipAllHipModels();
        rightLegModelChanger.UnEquipAllLegModels();
        leftLegModelChanger.UnEquipAllLegModels();
        if (playerInventoryManager.currentHipEquipment != null)
        {
            hipModelChanger.EquipmentHipModelByName(playerInventoryManager.currentHipEquipment.hipModelName);
            rightLegModelChanger.EquipmentLegModelByName(playerInventoryManager.currentHipEquipment.rightLefName);
            leftLegModelChanger.EquipmentLegModelByName(playerInventoryManager.currentHipEquipment.leftLegName);
            playerStatsManager.physicalDamageAbsorptionLegs = playerInventoryManager.currentHipEquipment.physicalDefense;
        }
        else
        {
            hipModelChanger.EquipmentHipModelByName(nakedHipModel);
            rightLegModelChanger.EquipmentLegModelByName(nakedRightLegModel);
            leftLegModelChanger.EquipmentLegModelByName(nakedLeftLegModel);
            playerStatsManager.physicalDamageAbsorptionLegs = 0;
        }
    }

    public void ChangeTorso()
    {
        torsoModelChanger.UnEquipAllTorsoModels();
        upperRightArmModelChanger.UnEquipAllUpperArmModels();
        upperLeftArmModelChanger.UnEquipAllUpperArmModels();
        if (playerInventoryManager.currentTorsoEquipment != null)
        {
            torsoModelChanger.EquipmentTorsoModelByName(playerInventoryManager.currentTorsoEquipment.torsoModelName);
            upperRightArmModelChanger.EquipmentUpperArmModelByName(playerInventoryManager.currentTorsoEquipment
                .upperRightArmModelName);
            upperLeftArmModelChanger.EquipmentUpperArmModelByName(playerInventoryManager.currentTorsoEquipment
                .upperLeftArmModelName);
            playerStatsManager.physicalDamageAbsorptionBody = playerInventoryManager.currentTorsoEquipment.physicalDefense;
        }
        else
        {
            torsoModelChanger.EquipmentTorsoModelByName(nakedTorsoModel);
            upperRightArmModelChanger.EquipmentUpperArmModelByName(nakedUpperRightArmModel);
            upperLeftArmModelChanger.EquipmentUpperArmModelByName(nakedUpperLeftArmModel);
            playerStatsManager.physicalDamageAbsorptionBody = 0;
        }
    }

    public void ChangeHelmet()
    {
        helmetModelChanger.UnEquipAllHelmetModels();
        if (playerInventoryManager.currentHelmetEquipment != null)
        {
            nakedHeadModel.SetActive(false);
            helmetModelChanger.EquipmentHelmetModelByName(playerInventoryManager.currentHelmetEquipment.helmetModelName);
            playerStatsManager.physicalDamageAbsorptionHead = playerInventoryManager.currentHelmetEquipment.physicalDefense;
        }
        else
        {
            nakedHeadModel.SetActive(true);
            playerStatsManager.physicalDamageAbsorptionHead = 0; //playerInventory.currentHelmetEquipment.physicalDefense;
        }
    }

    public void OpenBlockingCollider()//打开防御碰撞体
    {
        if (inputHandler.twoHandFlag)
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.rightWeapon);
        }
        else
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.leftWeapon);
        }
        blockingCollider.EnableBlockingCollider();
    }

    public void CloseBlockingCollider()//关闭防御碰撞体
    {
        blockingCollider.DisableBlockingCollider();
    }
}

