using System.Collections;
using UnityEngine;


public class OpenChest : Interactable
{
    //Components
    public Transform playerStandingPosition;//open Point
    private Animator animator;
    private OpenChest openChest;
    
    public GameObject itemSpawner;//开启后生成的物品
    public Item itemInChest;//has item

    private void Awake()
    {
        animator = GetComponent<Animator>();
        openChest = GetComponent<OpenChest>();
    }

    public override void Interact(PlayerManager playerManager)
    {
        playerManager.OpenChestInteraction(playerStandingPosition);
        animator.Play("Chest Open");
        
        //设置 方向
        Vector3 rotationDirection = transform.position - playerManager.transform.position;
        rotationDirection.y = 0;
        rotationDirection.Normalize();

        Quaternion tr = Quaternion.LookRotation(rotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 1);
        playerManager.transform.rotation = targetRotation;
        
        //生成世界物品 和删除脚本 
        StartCoroutine(SpawnItemChest(playerManager));

        WeaponPickUp weaponPickUp = itemSpawner.GetComponent<WeaponPickUp>();
        if (weaponPickUp != null)//如果生成出了世界物品，设置世界物品
        {
            weaponPickUp.weapon = itemInChest as WeaponItem;
        }
    }

    private IEnumerator SpawnItemChest(PlayerManager playerManager)
    {
        yield return new WaitForSeconds(3);
        playerManager.DisableSetPosition();
        Instantiate(itemSpawner, transform);
        Destroy(openChest);
    }
}
