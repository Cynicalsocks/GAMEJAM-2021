using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum type_behavior
{
    flock,
    pursue,
    avoid_obstacles,
    wander,
    flee
}

public class SteeringBehavior : MonoBehaviour
{
    public type_behavior[] behaviors;

    const float STEERING_FORCE = 5f;
    const float COLLISION_DETECTION_DISTANCE = 20f;
    const float COLLISION_PUSH_FORCE = 1.8f;

    const float SEPARATION_DISTANCE = 8f;
    const float COHESION_DISTANCE = 18f;
    const float ALIGNMENT_DISTANCE = 10f;
    const float PURSUE_LOOK_AHEAD = 10f;

    const float FEELER_POINT_SIZE = 3f;
    const float FEELER_LENGTH = 15f;
    const int NUM_FEELERS = 11;
    const int NUM_FEELER_POINTS = 5;

    Vector2[] feelers;
    bool[] feelers_collisions;

    Vehicle thisVehicle;

    void Start()
    {
        thisVehicle = GetComponent<Vehicle>();
        CreateFeelers();
    }

    public Vector2 CalculateSteering()
    {
        Vector2 FinalForce = Vector2.zero;
        thisVehicle.m_vSteeringForce = Vector2.zero;
        foreach (type_behavior behavior in behaviors)
        {
            switch (behavior)
            {
                case type_behavior.pursue:
                    {
                        thisVehicle.m_vSteeringForce += Pursue(VehicleManager.Instance.playerTransform.position, VehicleManager.Instance.playerRigidbody2D.velocity);
                    }
                    break;
                case type_behavior.flock:
                    {
                        //thisVehicle.m_vSteeringForce += Separation() * 1.5f;
                        //thisVehicle.m_vSteeringForce += Cohesion();
                        thisVehicle.m_vSteeringForce += Alignment();
                    }
                    break;
                default:
                    break;
            }
        }

        return FinalForce;
    }

    public Vector2 Seek(Vector2 target)
    {
        Vector2 DesiredVelocity = (target - thisVehicle.position).normalized * thisVehicle.movementSpeed;

        Vector2 currentMovement = (thisVehicle.m_vVelocity);

        //DEBUGGING
        thisVehicle.m_vSeekForce = DesiredVelocity - currentMovement;

        return DesiredVelocity - currentMovement;
    }

    public Vector2 Pursue(Vector2 target, Vector2 targetVelocity)
    {
        Vector2 targetFuturePosition = target + targetVelocity.normalized * PURSUE_LOOK_AHEAD;

        return Seek(targetFuturePosition);
    }

    public Vector2 Separation()
    {
        Vector2 SteeringForce = Vector2.zero;

        foreach (Vehicle neighbor in VehicleManager.Instance.listVehicles)
        {
            if (Vector2.Distance(neighbor.position, thisVehicle.position) < SEPARATION_DISTANCE)
            {
                if (neighbor != thisVehicle && neighbor.vehicleType == thisVehicle.vehicleType)
                {
                    Vector2 FromNeighbor = thisVehicle.position - neighbor.position;
                    SteeringForce += FromNeighbor;
                }
            }
        }

        thisVehicle.m_vSeparationForce = SteeringForce;
        return SteeringForce;
    }

    public Vector2 Cohesion()
    {
        //first find the center of mass of all the agents
        Vector2 CenterOfMass = thisVehicle.position;

        int NeighborCount = 0;

        //iterate through the neighbors and sum up all the position vectors
        foreach (Vehicle neighbor in VehicleManager.Instance.listVehicles)
        {
            if (Vector2.Distance(neighbor.position, thisVehicle.position) < COHESION_DISTANCE)
            {
                //make sure *this* agent isn't included in the calculations and that
                //the agent being examined is close enough
                if (neighbor != thisVehicle && neighbor.vehicleType == thisVehicle.vehicleType)
                {
                    CenterOfMass += neighbor.position + neighbor.m_vVelocity.normalized;

                    ++NeighborCount;
                }
            }
        }

        if (NeighborCount > 0)
        {
            //the center of mass is the average of the sum of positions
            CenterOfMass.x /= NeighborCount;
            CenterOfMass.y /= NeighborCount;

            //now seek towards that position
            thisVehicle.m_vCohesionForce = Seek(CenterOfMass);
            return Seek(CenterOfMass);
        }

        //the magnitude of cohesion is usually much larger than separation or
        //allignment so it usually helps to normalize it.
        thisVehicle.m_vCohesionForce = Vector2.zero;
        return Vector2.zero;
    }

    public Vector2 Alignment()
    {
        //This will record the average heading of the neighbors
        Vector2 AverageHeading = Vector2.zero;

        //This count the number of vehicles in the neighborhood
        float NeighborCount = 0.0f;

        //iterate through the neighbors and sum up all the position vectors
        foreach (Vehicle neighbor in VehicleManager.Instance.listVehicles)
        {
            if (Vector2.Distance(neighbor.position, thisVehicle.position) < ALIGNMENT_DISTANCE)
            {
                //make sure *this* agent isn't included in the calculations and that
                //the agent being examined  is close enough
                if (neighbor != thisVehicle && neighbor.vehicleType == thisVehicle.vehicleType)
                {
                    AverageHeading += neighbor.rigidBody.velocity.normalized;

                    ++NeighborCount;
                }
            }
        }

        //if the neighborhood contained one or more vehicles, average their
        //heading vectors.
        if (NeighborCount > 0.0f)
        {
            AverageHeading.x /= NeighborCount;
            AverageHeading.y /= NeighborCount;

            AverageHeading -= thisVehicle.rigidBody.velocity.normalized;
        }

        if (AverageHeading.magnitude <= 0)
            return Vector2.zero;

        return ((AverageHeading.normalized) - (thisVehicle.m_vVelocity.normalized)).normalized * 0.1f;
    }


    void CreateFeelers()
    {
        feelers = new Vector2[NUM_FEELERS];
        feelers_collisions = new bool[NUM_FEELERS];

        for (int i = 0; i < NUM_FEELERS; i++)
        {
            float angle = 360 / NUM_FEELERS * i;
            feelers[i] = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * FEELER_LENGTH;
            feelers_collisions[i] = false;
        }
    }

    private void OnDrawGizmos()
    {
        // Display Feelers
        for (int i = 0; i < NUM_FEELERS; i++)
        {
            //Feeler Points
            for (int p = 0; p < NUM_FEELER_POINTS; p++)
            {
                Vector2 point = thisVehicle.position + (feelers[i] / NUM_FEELER_POINTS * p);
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(point, 0.5f);
            }

            if (feelers_collisions[i] == true)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, (Vector2)transform.position + feelers[i]);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, (Vector2)transform.position + feelers[i]);
            }
        }
    }
}
