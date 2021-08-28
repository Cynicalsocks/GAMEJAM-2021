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

    const float SEPARATION_DISTANCE = 8f;
    const float ALIGNMENT_DISTANCE = 10f;
    const float PURSUE_LOOK_AHEAD = 10f;

    Ball thisBall;

    void Start()
    {
        thisBall = GetComponent<Ball>();
    }

    public Vector2 CalculateSteering()
    {
        Vector2 FinalForce = Vector2.zero;
        thisBall.thisVehicle.m_vSteeringForce = Vector2.zero;
        foreach (type_behavior behavior in behaviors)
        {
            switch (behavior)
            {
                case type_behavior.pursue:
                    {
                        thisBall.thisVehicle.m_vSteeringForce += Pursue(VehicleManager.Instance.playerTransform.position, VehicleManager.Instance.playerRigidbody2D.velocity);
                    }
                    break;
                case type_behavior.flock:
                    {
                        //thisVehicle.m_vSteeringForce += Separation() * 1.5f;
                        thisBall.thisVehicle.m_vSteeringForce += Cohesion();
                        //thisVehicle.m_vSteeringForce += Alignment();
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
        Vector2 DesiredVelocity = (target - thisBall.thisVehicle.position).normalized * thisBall.thisVehicle.movementSpeed;

        Vector2 currentMovement = (thisBall.thisVehicle.m_vVelocity);

        return DesiredVelocity - currentMovement;
    }

    public Vector2 Pursue(Vector2 target, Vector2 targetVelocity)
    {
        Vector2 targetFuturePosition = target + targetVelocity.normalized * PURSUE_LOOK_AHEAD;

        return Seek(targetFuturePosition);
    }

    public Vector2 Separation(List<Vehicle> balls)
    {
        Vector2 SteeringForce = Vector2.zero;

        foreach (Vehicle neighbor in balls)
        {
            if (Vector2.Distance(neighbor.position, thisBall.thisVehicle.position) < SEPARATION_DISTANCE)
            {
                if (neighbor != thisBall.thisVehicle && neighbor.vehicleType == thisBall.thisVehicle.vehicleType)
                {
                    Vector2 FromNeighbor = thisBall.thisVehicle.position - neighbor.position;
                    SteeringForce += FromNeighbor;
                }
            }
        }

        thisBall.thisVehicle.m_vSeparationForce = SteeringForce;
        return SteeringForce;
    }

    public Vector2 Cohesion()
    {
        return Seek(FlockingAlgorithms.Instance.CenterOfMass);
    }

    public Vector2 Alignment(List<Vehicle> balls)
    {
        //This will record the average heading of the neighbors
        Vector2 AverageHeading = Vector2.zero;

        //This count the number of vehicles in the neighborhood
        float NeighborCount = 0.0f;

        //iterate through the neighbors and sum up all the position vectors
        foreach (Vehicle neighbor in balls)
        {
            if (Vector2.Distance(neighbor.position, thisBall.thisVehicle.position) < ALIGNMENT_DISTANCE)
            {
                //make sure *this* agent isn't included in the calculations and that
                //the agent being examined  is close enough
                if (neighbor != thisBall.thisVehicle && neighbor.vehicleType == thisBall.thisVehicle.vehicleType)
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

            AverageHeading -= thisBall.thisVehicle.rigidBody.velocity.normalized;
        }

        if (AverageHeading.magnitude <= 0)
            return Vector2.zero;

        return ((AverageHeading.normalized) - (thisBall.thisVehicle.m_vVelocity.normalized)).normalized * 0.1f;
    }
}
