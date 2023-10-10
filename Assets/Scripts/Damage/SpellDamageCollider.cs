using System;
using UnityEngine;


/// <summary>
/// 魔法球 投射 碰撞器脚本 阉割
/// </summary>
public class SpellDamageCollider : DamageCollider
{
    public GameObject impactParticles;//碰撞 爆炸特效
    public GameObject projectileParticles;
    public GameObject muzzleParticles;
    private bool hasCollider = false;
    
    private Rigidbody rigidbody;

    private Vector3 impactNormal;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        projectileParticles = Instantiate(projectileParticles, transform.position, transform.rotation);
        projectileParticles.transform.parent = transform;

        if (muzzleParticles != null)
        {
            muzzleParticles = Instantiate(muzzleParticles, transform.position, transform.rotation);
            Destroy(muzzleParticles, 2f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        if (!hasCollider)
        {
            spellTarget = other.transform.GetComponent<CharacterStatsManager>();
            
            if (spellTarget != null && spellTarget.teamIDNumber != teamIDNumber)
            {
                spellTarget.TakeDamage(physicalDamage,fireDamage ,currentDamageAnimation);
            }
            
            hasCollider = true;
            impactParticles = Instantiate(impactParticles, transform.position, transform.rotation);
            
            Destroy(projectileParticles);
            Destroy(impactParticles, 3);
            Destroy(gameObject);
        }
    }
}
