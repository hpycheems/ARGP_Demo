using UnityEngine;


public class DestroyAfterTime : MonoBehaviour
{
    public float timeUntilDestroy = 2;

    private void Awake()
    {
        Destroy(gameObject,timeUntilDestroy);
    }
}
