using UnityEngine;


public class BombDamageCollider : DamageCollider
{
    public int eplosiveRadius = 1;
    public int explosionDamage;
    public int explosionSplashDamage;

    private bool hasCollided = false;
    public Rigidbody bombRigidbody;
    public GameObject impactParticles;
    
    protected override void Awake()
    {
        damageCollider = GetComponent<Collider>();
        bombRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!hasCollided)
        {
            hasCollided = true;
            impactParticles = Instantiate(impactParticles, transform.position, Quaternion.identity);
            Explode();

            CharacterStatsManager character = damageCollider.transform.GetComponent<CharacterStatsManager>();
            if (character)
            {
                //character.TakeDamage(0, explosionDamage);
                if(character.teamIDNumber != teamIDNumber)
                    character.TakeDamage(0, explosionDamage, currentDamageAnimation);
            }
            
            Destroy(impactParticles, 3f);
            Destroy(transform.parent.gameObject);
        }
    }

    void Explode()
    {
        Collider[] characters = Physics.OverlapSphere(transform.position, eplosiveRadius);
        foreach (var objectInExplosion in characters)
        {
            CharacterStatsManager character = objectInExplosion.GetComponent<CharacterStatsManager>();
            if (character != null)
            {
                //character.TakeDamage(0, explosionSplashDamage);
                if(character.teamIDNumber != teamIDNumber)
                    character.TakeDamage(0, explosionDamage,currentDamageAnimation);
            }
        }
    }
}
