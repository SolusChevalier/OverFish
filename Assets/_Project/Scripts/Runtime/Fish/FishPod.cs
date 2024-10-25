using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPod : MonoBehaviour
{
    #region FIELDS

    public GameObject[] FishPrefab;
    public int FishCount;
    public int FishCapacity;
    public float spawnRate;
    public List<Fish> FishList;
    public Vector3 PodPosition;
    public float PodRadius;
    public FishManager fishManager;
    private GameObject _Origin;
    private List<Bounds> _PodBounds;
    private float timeSinceLastSpawn = 0f;

    #endregion FIELDS

    #region UNITY METHODS

    private void Start()
    {
        _Origin = GetComponentInChildren<Transform>().gameObject;
        _PodBounds = new List<Bounds>();
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            _PodBounds.Add(col.bounds);
            col.isTrigger = true;
        }
        FishList = new List<Fish>();
        PodPosition = transform.position;
        for (int i = 0; i < FishCount; i++)
        {
            SpawnFish();
        }
    }

    private void FixedUpdate()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (FishCount < FishCapacity & timeSinceLastSpawn >= spawnRate & FishCount > 2)
        {
            SpawnFish();
            timeSinceLastSpawn = 0f;
        }
        if (FishCount < 2)
        {
            fishManager.CurrentFish -= FishCount;
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        fishManager.PodList.Remove(this);
        fishManager.MaxFishCapacity -= FishCapacity;
        fishManager.PodCount--;
    }

    #endregion UNITY METHODS

    #region METHODS

    public void MovePod(Vector3 direction)
    {
        PodPosition += direction;
        transform.position = PodPosition;
    }

    public void SpawnFish()
    {
        if (fishManager.CurrentFish < fishManager.MaxFishCapacity)
        {
            int randomFish = Random.Range(0, FishPrefab.Length);
            Vector3 randomDirection = new Vector3(Random.Range(-PodRadius, PodRadius), 0, Random.Range(-PodRadius, PodRadius));
            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            GameObject fish = Instantiate(FishPrefab[randomFish], _Origin.transform.position + randomDirection, randomRotation);
            fish.transform.SetParent(transform);
            Fish fishRef = fish.GetComponent<Fish>();
            fishRef.SetSpeedLimits(fishManager.MinFishSpeed, fishManager.MaxFishSpeed);
            fishRef.SetPodRadius(PodRadius);
            fishRef.Pod = this;
            fishRef.SetPodBounds(_PodBounds[Random.Range(0, _PodBounds.Count)]);
            fishRef.PodOrigin = _Origin;
            FishList.Add(fishRef);
            fishManager.CurrentFish++;
        }
    }

    #endregion METHODS
}