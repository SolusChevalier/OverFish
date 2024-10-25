using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    #region FIELDS

    public float Speed = 2f;
    public float RotationSpeed = 2f;
    public float MinSpeed;
    public float MaxSpeed;
    public float PodRadius;
    public GameObject PodOrigin;
    public Bounds PodBounds;
    private bool direction = true;
    public FishPod Pod;
    public bool isInsidePod = true;

    #endregion FIELDS

    #region UNITY METHODS

    private void Start()
    {
        RotationSpeed = Random.Range(5, 10);
        direction = Random.Range(0, 2) == 0;
        InvokeRepeating("BounceOffBounds", 0, 2);
    }

    private void OnDestroy()
    {
        Pod.FishList.Remove(this);
        Pod.FishCount--;
        Pod.fishManager.CurrentFish--;
    }

    #endregion UNITY METHODS

    #region METHODS

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        FluctuateSpeed();
        if (!PodBounds.Contains(transform.position))
        {
            isInsidePod = false;
            Vector3 direction = PodOrigin.transform.position - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, PodOrigin.transform.position, Speed * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, direction, Speed * Time.deltaTime);
        }
        else
        {
            isInsidePod = true;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Speed * Time.deltaTime);
        }
        RotateFish();
    }

    public void BounceOffBounds()
    {
        Vector3 direction = PodOrigin.transform.position - transform.position;
        direction = direction.normalized;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward + direction, Speed * Time.deltaTime);
    }

    public void KeepInBounds()
    {
        if (!PodBounds.Contains(transform.position))
        {
            Vector3 direction = PodOrigin.transform.position - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, direction, Speed * Time.deltaTime);
        }
    }

    public void FluctuateSpeed()
    {
        Speed = Random.Range(MinSpeed, MaxSpeed);
    }

    public void RotateFish()
    {
        Vector3 tangent = Vector3.Cross(PodOrigin.transform.position - transform.position, Vector3.up);

        tangent = (direction) ? tangent : -tangent;

        //Quaternion LookRot = Quaternion.LookRotation(PodOrigin.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(tangent), RotationSpeed * Time.deltaTime);
    }

    public void SetSpeedLimits(float min, float max) => (MinSpeed, MaxSpeed) = (min, max);

    public void SetPodRadius(float radius) => PodRadius = radius;

    public void SetPodBounds(Bounds bounds) => PodBounds = bounds;

    public void SetPodOrigin(GameObject pod) => PodOrigin = pod;

    #endregion METHODS
}