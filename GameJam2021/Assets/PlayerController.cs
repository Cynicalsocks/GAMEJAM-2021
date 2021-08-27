using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Keybindings
    public KeyCode TurnLeft;
    public KeyCode TurnRight;
    public KeyCode BoostKey;
    public KeyCode FireKey;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKey(BoostKey))
        {
        }
        else
        {
        }

        if (Input.GetKey(TurnLeft))
        {
        }
        else if (Input.GetKey(TurnRight))
        {
        }

        if (Input.GetMouseButton(0))
        {
        }

        if (Input.GetMouseButton(1))
        {
        }
        else
        {
        }

    }

    Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    Vector3 GetScreenCenter()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0.0f));
    }

    Vector3 GetMouseDirection()
    {
        return (GetMousePosition() - GetPlayerScreenPosition(Vector2.zero, 0)).normalized;
    }

    Vector3 GetPlayerScreenPosition(Vector2 vel, float maxSpeed)
    {
        float posx = Mathf.RoundToInt(Mathf.Min(vel.x / 6, maxSpeed) * 2) / 1.0f;
        float posy = Mathf.RoundToInt(Mathf.Min(vel.y / 6, maxSpeed) * 2) / 1.0f;
        return GetScreenCenter() - new Vector3(posx, posy, 0.0f);
    }
}
