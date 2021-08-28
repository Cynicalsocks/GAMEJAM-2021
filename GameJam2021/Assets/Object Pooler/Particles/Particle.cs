using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine;

public class Particle : MonoBehaviour, IPooledObject
{
    public Color[] colors;
    public Sprite[] sprites;
    public string particleName = " ";
    public float decayTime;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Color currentColor;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void OnObjectSpawn(ParticleInfo particleInfo)
    {
        // Set random sprite from array of sprites
        if (sprites.Length > 0)
        {
            int i = Random.Range(0, sprites.Length);
            spriteRenderer.sprite = sprites[i];
        }
        // Set random color from array of colors
        if (colors.Length > 0)
        {
            int i = Random.Range(0, colors.Length);
            currentColor = colors[i];
        }
        else
            currentColor = spriteRenderer.color;

        // Give Velocity
        rb.velocity = particleInfo.dir * particleInfo.force;

        // Reset alpha
        spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1.0f);

        // Start fade out
        StartCoroutine("StartFadingOut");
    }

    IEnumerator StartFadingOut()
    {
        float fadeTime = Time.time + decayTime;
        float alpha = 1.0f;

        while(fadeTime > Time.time)
        {
            yield return new WaitForSeconds(0.0f);
        }

        while(alpha > 0.0f)
        {
            spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            alpha -= 1.0f * Time.deltaTime;

            yield return new WaitForSeconds(0.0f);
        }
        
        gameObject.SetActive(false);
    }
}
