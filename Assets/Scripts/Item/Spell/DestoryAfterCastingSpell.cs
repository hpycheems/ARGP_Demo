using UnityEngine;

/// <summary>
/// 法术生成的物品销毁脚本
/// </summary>
public class DestoryAfterCastingSpell : MonoBehaviour
{
    [SerializeField] private CharacterManager characterManager;

    private void Awake()
    {
        characterManager = GetComponentInParent<CharacterManager>();
    }

    private void Update()
    {
        if (characterManager.isFiringSpell)
        {
            Destroy(gameObject);
        }
    }
}
