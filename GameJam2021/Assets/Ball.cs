using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum Ball_Size 
{
    teeny,
    tiny,
    small,
    medium,
    large,
    big,
    gigantic
}

public enum Ball_Color
{
    red,
    orange,
    yellow,
    green,
    teal,
    blue,
    purple
}

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidbody2d;
    [SerializeField] GameObject prefab_ball;
    [SerializeField] float force;

    [SerializeField] public Ball_Size size;
    [SerializeField] public Ball_Color color;

    [SerializeField] public Vehicle thisVehicle;
    [SerializeField] MeshRenderer meshRenderer;

    public void OnSpawn(Vector2 dir)
    {
        VehicleManager.Instance.AddBall(this);

        Vector3 direction = Vector3.RotateTowards(dir, new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f), 0.1f, 0);

        rigidbody2d.AddForce(direction * force, ForceMode2D.Force);

        switch (size)
        {
            case Ball_Size.teeny:
                transform.localScale = new Vector3(1f, 1f, 1f);
                thisVehicle.movementSpeed = 60;
                thisVehicle.maxSpeed = 12;
                rigidbody2d.mass = 2f;
                break;
            case Ball_Size.tiny:
                transform.localScale = new Vector3(2f, 2f, 2f);
                thisVehicle.movementSpeed = 55;
                thisVehicle.maxSpeed = 11;
                rigidbody2d.mass = 4f;
                break;
            case Ball_Size.small:
                transform.localScale = new Vector3(3f, 3f, 3f);
                thisVehicle.movementSpeed = 50;
                thisVehicle.maxSpeed = 10;
                rigidbody2d.mass = 6f;
                break;
            case Ball_Size.medium:
                transform.localScale = new Vector3(4f, 4f, 4f);
                thisVehicle.movementSpeed = 45;
                thisVehicle.maxSpeed = 9;
                rigidbody2d.mass = 8f;
                break;
            case Ball_Size.large:
                transform.localScale = new Vector3(5f, 5f, 5f);
                thisVehicle.movementSpeed = 40;
                thisVehicle.maxSpeed = 8;
                rigidbody2d.mass = 10f;
                break;
            case Ball_Size.big:
                transform.localScale = new Vector3(6f, 6f, 6f);
                thisVehicle.movementSpeed = 35;
                thisVehicle.maxSpeed = 7;
                rigidbody2d.mass = 12f;
                break;
            case Ball_Size.gigantic:
                transform.localScale = new Vector3(7f, 7f, 7f);
                thisVehicle.movementSpeed = 30;
                thisVehicle.maxSpeed = 6;
                rigidbody2d.mass = 14f;
                break;
            default:
                break;
        }

        switch (color) 
        {
            case Ball_Color.red:
                meshRenderer.material.SetColor("Ball_Color", new Color(1f, 0f, 0f));
                break;
            case Ball_Color.orange:
                meshRenderer.material.SetColor("Ball_Color", new Color(1f, 0.4f, 0f));
                break;
            case Ball_Color.yellow:
                meshRenderer.material.SetColor("Ball_Color", new Color(1f, 0.6f, 0f));
                break;
            case Ball_Color.green:
                meshRenderer.material.SetColor("Ball_Color", new Color(0.2f, 1f, 0f));
                break;
            case Ball_Color.teal:
                meshRenderer.material.SetColor("Ball_Color", new Color(0.1f, 0.6f, 1f));
                break;
            case Ball_Color.blue:
                meshRenderer.material.SetColor("Ball_Color", new Color(0.1f, 0.2f, 1f));
                break;
            case Ball_Color.purple:
                meshRenderer.material.SetColor("Ball_Color", new Color(0.5f, 0f, 1f));
                break;
            default:
                meshRenderer.material.SetColor("Ball_Color", new Color(1f, 1f, 1f));
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Projectile"))
        {
            if (size > 0)
            {
                GameObject obj;
                Ball newBall;

                // Spawn new balls
                for (int i = 0; i < 4; i++)
                {
                    obj = Instantiate(prefab_ball, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f), Quaternion.identity, null);
                    newBall = obj.GetComponent<Ball>();
                    newBall.size = size - 1;
                    newBall.color = color;
                    newBall.OnSpawn(rigidbody2d.velocity.normalized);
                }
            }
            VehicleManager.Instance.listBalls.Remove(thisVehicle);
            ScoreBoard.Instance.score++;
            Destroy(gameObject);
        }
    }
}
