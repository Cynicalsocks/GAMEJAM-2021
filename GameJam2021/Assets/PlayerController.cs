using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Keybindings
    public KeyCode MoveLeft;
    public KeyCode MoveRight;
    public KeyCode MoveUp;
    public KeyCode MoveDown;

    public KeyCode ShootLeft;
    public KeyCode ShootRight;
    public KeyCode ShootUp;
    public KeyCode ShootDown;

    [SerializeField] float speed;
    [SerializeField] Rigidbody2D rigidbody;

    [SerializeField] Camera camera1;
    [SerializeField] Camera camera2;
    [SerializeField] CameraController cameraController;

    Vector2 moveDir;
    Vector2 shootDir;
    void FixedUpdate()
    {
        moveDir = Vector2.zero;
        shootDir = Vector2.zero;

        if (Input.GetKey(MoveLeft))
            moveDir += Vector2.left;

        if (Input.GetKey(MoveRight))
            moveDir += Vector2.right;

        if (Input.GetKey(MoveUp))
            moveDir += Vector2.up;

        if (Input.GetKey(MoveDown))
            moveDir += Vector2.down;

        if (moveDir.magnitude > 0)
            rigidbody.AddForce(moveDir.normalized * speed, ForceMode2D.Force);


        if (Input.GetKey(ShootLeft))
            shootDir += Vector2.left;

        if (Input.GetKey(ShootRight))
            shootDir += Vector2.right;

        if (Input.GetKey(ShootUp))
            shootDir += Vector2.up;

        if (Input.GetKey(ShootDown))
            shootDir += Vector2.down;

        if (shootDir.magnitude > 0)
            Fire(shootDir.normalized);
    }

    // For Weapons (might change the way this works? using a new class for weapon types)
    const float fireInterval = 0.05f;
    private float fireIntervalCounter = 0.0f;

    public void Fire(Vector3 dir)
    {
        if (fireIntervalCounter < Time.time)
        {
            ProjectileSpawner.Instance.SpawnProjectile("Basic_Projectile", new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity, dir, 40f);

            // Play blast sound
            //AudioManager.Instance.Play2DSound("Blast_01");

            fireIntervalCounter = Time.time + fireInterval;
        }
    }

    //// Do Collision Detection with Triggers
    public void OnTriggerEnter2D(Collider2D collider)
    {
        // If we are colliding with an enemy
        if (collider.CompareTag("Enemy"))
        {
        }
    }


    Vector2 GetMousePosition()
    {
        return camera2.ScreenToWorldPoint(Input.mousePosition);
    }

    Vector2 GetScreenCenter()
    {
        return camera1.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
    }

    Vector2 GetMouseDirection()
    {
        return Vector3.Normalize(GetMousePosition() - GetPlayerScreenPosition());
    }

    Vector2 GetPlayerScreenPosition()
    {
        return new Vector2(transform.position.x, transform.position.y) - GetScreenCenter();
    }
}
