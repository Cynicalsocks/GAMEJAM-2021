using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingAlgorithms : MonoBehaviour
{
    #region Singleton
    public static FlockingAlgorithms Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public Vector2 CenterOfMass = Vector2.zero;

    private void FixedUpdate()
    {
        CalculateCenterOfMass();
    }

    void CalculateCenterOfMass()
    {
        //first find the center of mass of all the agents
        var balls = VehicleManager.Instance.listBalls;

        int NeighborCount = 0;

        foreach (Vehicle neighbor in balls)
        {
            CenterOfMass += neighbor.position + neighbor.m_vVelocity.normalized;
            ++NeighborCount;
        }

        if (NeighborCount > 0)
        {
            //the center of mass is the average of the sum of positions
            CenterOfMass.x /= NeighborCount;
            CenterOfMass.y /= NeighborCount;
        }
    }
}
