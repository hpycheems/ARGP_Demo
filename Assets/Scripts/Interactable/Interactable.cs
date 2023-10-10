using System;
using UnityEngine;

/// <summary>
/// 可交互物品基类
/// </summary>
public class Interactable : MonoBehaviour
{
    //detection Parameter
    public float radius = .6f;
    public string interactableText;

    private void OnDrawGizmos()//画出检测 区域
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    /// <summary>
    /// 交互方法
    /// </summary>
    /// <param name="playerManager"></param>
    public virtual void Interact(PlayerManager playerManager)
    {
        Debug.Log(interactableText);
    }
}
