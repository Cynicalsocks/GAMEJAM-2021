using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    public string projectileName = " ";
    Rigidbody2D rb;
    //Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
    }

    public void OnObjectSpawn(ParticleInfo particleInfo)
    {
        //animator.Play(projectileName, -1, 0f);
        
        rb.velocity = Vector2.zero;
        transform.position = new Vector3(particleInfo.pos.x, particleInfo.pos.y, 0f);
        rb.AddForce(particleInfo.dir * particleInfo.force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
    }
}
