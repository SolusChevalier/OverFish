using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat_P1 : MonoBehaviour
{
    #region FIELDS

    public Transform Motor;
    public Player_1 Player_1;
    public float CatchRadius;
    public float SteerPower = 500f;
    public float Power = 5f;
    public float MaxSpeed = 10f;
    public float Drag = 0.1f;
    public ParticleSystem fishParticle;
    protected Rigidbody Rigidbody;

    protected Quaternion StartRotation;
    //protected ParticleSystem ParticleSystem;

    #endregion FIELDS

    #region UNITY METHODS

    public void Awake()
    {
        //Player_1 = GetComponent<Player_1>();
        //Player_1.CatchRadius = CatchRadius;
        //ParticleSystem = GetComponentInChildren<ParticleSystem>();
        Rigidbody = GetComponent<Rigidbody>();
        StartRotation = Motor.localRotation;
        //Camera = Camera.main;
    }

    public void FixedUpdate()
    {
        var forceDirection = transform.forward;
        var steer = 0;

        if (Input.GetKey(KeyCode.A))
            steer = 1;
        if (Input.GetKey(KeyCode.D))
            steer = -1;

        Rigidbody.AddForceAtPosition(steer * transform.right * SteerPower / 100f, Motor.position);

        var forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);
        var targetVel = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            ApplyForceToReachVelocity(Rigidbody, forward * MaxSpeed, Power);
        if (Input.GetKey(KeyCode.S))
            ApplyForceToReachVelocity(Rigidbody, forward * -MaxSpeed, Power);

        /*//Motor Animation // Particle system
        Motor.SetPositionAndRotation(Motor.position, transform.rotation * StartRotation * Quaternion.Euler(0, 30f * steer, 0));
        if (ParticleSystem != null)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                ParticleSystem.Play();
            else
                ParticleSystem.Pause();
        }
*/
        //moving forward
        var movingForward = Vector3.Cross(transform.forward, Rigidbody.velocity).y < 0;

        //move in direction
        Rigidbody.velocity = Quaternion.AngleAxis(Vector3.SignedAngle(Rigidbody.velocity, (movingForward ? 1f : 0f) * transform.forward, Vector3.up) * Drag, Vector3.up) * Rigidbody.velocity;
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        switch (collision.collider)
        {
            case var dock when dock == Player_1.FishFactoryDock.Collider:
                Player_1.HandleFactoryDocking();
                break;

            case var dock when dock == Player_1.BoatSpeedDockDock.Collider:
                Player_1.HandleSpeedDocking();
                break;

            case var dock when dock == Player_1.BoatUpgradeDockDock.Collider:
                Player_1.HandleUpgradeDocking();
                break;

            case var dock when dock == Player_1.CargoSpaceDockDock.Collider:
                Player_1.HandleCargoDocking();
                break;

            case var dock when dock == Player_1.FishingSpeedDockDock.Collider:
                Player_1.HandleFishingRateDocking();
                break;

            default:
                break;
        }
    }*/

    #endregion UNITY METHODS

    #region METHODS

    public void ApplyForceToReachVelocity(Rigidbody rigidbody, Vector3 velocity, float force = 1, ForceMode mode = ForceMode.Force)
    {
        if (force == 0 || velocity.magnitude == 0)
            return;

        velocity = velocity + velocity.normalized * 0.2f * rigidbody.drag;

        force = Mathf.Clamp(force, -rigidbody.mass / Time.fixedDeltaTime, rigidbody.mass / Time.fixedDeltaTime);

        if (rigidbody.velocity.magnitude == 0)
        {
            rigidbody.AddForce(velocity * force, mode);
        }
        else
        {
            var velocityProjectedToTarget = (velocity.normalized * Vector3.Dot(velocity, rigidbody.velocity) / velocity.magnitude);
            rigidbody.AddForce((velocity - velocityProjectedToTarget) * force, mode);
        }
    }

    #endregion METHODS
}