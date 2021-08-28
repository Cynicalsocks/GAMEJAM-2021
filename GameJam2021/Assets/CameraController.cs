using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float cameraSpeed = 1f;
    // Update is called once per frame
    void Update()
    {
        Vector2 movement = GetCameraMovement();
        transform.position = new Vector3(
            movement.x, 
            movement.y, 
            -100f);
    }

    public Vector2 GetCameraMovement()
    {
        return Vector2.MoveTowards(transform.position, player.position, cameraSpeed * Time.deltaTime);
    }
}
