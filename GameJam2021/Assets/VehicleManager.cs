using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VehicleType
{
    player,
    enemy
}

public class VehicleManager : MonoBehaviour
{
    #region Singleton
    public static VehicleManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<Vehicle> listBalls;

    public Transform playerTransform;
    public Rigidbody2D playerRigidbody2D;

    private void Start()
    {
        listBalls = new List<Vehicle>();
    }

    public void AddBall(Ball ball)
    {
        listBalls.Add(ball.thisVehicle);
    }
}
