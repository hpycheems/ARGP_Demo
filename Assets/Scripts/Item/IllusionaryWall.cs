using System;
using UnityEngine;


public class IllusionaryWall : MonoBehaviour
{
    public bool wallHasBeenHit;
    public Material IllusionaryWallMaterial;
    public float alpha;
    public float fadeTime = 2.5f;
    public BoxCollider wallCollider;

    public AudioSource audioSource;
    public AudioClip illusionaryWallSound;
    private void Update()
    {
        if (wallHasBeenHit)
        {
            FadeIllusionaryWall();
        }
    }

    void FadeIllusionaryWall()
    {
        alpha = IllusionaryWallMaterial.color.a;
        alpha = alpha - Time.deltaTime / fadeTime;
        Color fadeWallColor = new Color(1, 1, 1, alpha);
        
        if (wallCollider.enabled)
        {
            wallCollider.enabled = false;
            audioSource?.PlayOneShot(illusionaryWallSound);
        }
        
        if (alpha <= 0)
        {
            Destroy(this);
        }
        
        IllusionaryWallMaterial.color = fadeWallColor;
    }
}
