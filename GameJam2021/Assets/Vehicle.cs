using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public bool faceHeading;
    public float movementSpeed;
    public float maxSpeed;
    public float m_fBoundingRadius;

    [HideInInspector] public SteeringBehavior steeringBehavior;

    [HideInInspector] public Vector2 position;
    [HideInInspector] public Rigidbody2D rigidBody;
    [HideInInspector] public new Collider2D collider2D;
    [HideInInspector] public Vehicle thisVehicle;

    [HideInInspector] public Vector2 m_vSteeringForce;
    [HideInInspector] public Vector2 m_vHeading;
    [HideInInspector] public Vector2 m_vVelocity;
    [HideInInspector] public Vector2 m_vSide;

    public VehicleType vehicleType;

    #region Debugging
    //      FOR DEBUGGING       //
    [HideInInspector] public Vector2 m_vNearestObstacle;
    
    [HideInInspector] public Vector2 m_vAvoidanceForce;
    [HideInInspector] public Vector2 m_vSeparationForce;
    [HideInInspector] public Vector2 m_vCohesionForce;
    [HideInInspector] public Vector2 m_vAlignmentForce;
    [HideInInspector] public Vector2 m_vSeekForce;
    //      FOR DEBUGGING       //
    #endregion

    public virtual void Awake()
    {
        steeringBehavior = GetComponent<SteeringBehavior>();
        rigidBody = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        thisVehicle = this;
    }

    virtual public void FixedUpdate()
    {
        if (steeringBehavior != null)
        {
            steeringBehavior.CalculateSteering();
            Move(m_vSteeringForce);
        }

        m_vVelocity = rigidBody.velocity;
        position = transform.position;

        // Cap the speed at maxSpeed
        if (rigidBody.velocity.magnitude > maxSpeed)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
        }

        if (m_vVelocity.sqrMagnitude > 0.00000001)
        {
            m_vHeading = m_vVelocity.normalized;
            m_vSide = Vector2.Perpendicular(m_vHeading);
        }
    }

    public void Move(Vector2 force)
    {
        rigidBody.AddForce(force, ForceMode2D.Force);
    }
}