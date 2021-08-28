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

    [SerializeField] Vehicle firstvehicle;

    public List<Vehicle> listVehicles;

    public Transform playerTransform;
    public Rigidbody2D playerRigidbody2D;

    private void Start()
    {
        listVehicles = new List<Vehicle>();
        listVehicles.Add(firstvehicle);
    }
}
