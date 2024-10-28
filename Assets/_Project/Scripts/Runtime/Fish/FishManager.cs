using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FishManager : MonoBehaviour
{
    public GameObject FishPodPrefab;
    public int PodCount;
    public int MaxPodCount;
    public List<FishPod> PodList;
    public int MaxFishCapacity;
    public int CurrentFish;
    public float MinFishSpeed;
    public float MaxFishSpeed;

    private void Awake()
    {
        //PodList = new List<FishPod>();
        //Debug.Log("Starting Pods1");
        //StartCoroutine(PodStartSpawn());
        //Invoke("StartPods", 2);
        //for (int i = 0; i < PodCount; i++)
        //{
        //SpawnPod();
        //}
        InvokeRepeating("MorePods", 5, 10);
        Invoke("EndGameGood", 300);
    }

    private void Update()
    {
        if (PodCount == 0 | CurrentFish <= 0 | MaxFishCapacity <= 0)
        {
            Debug.Log("Game Over: " + PodCount + " " + CurrentFish + " " + MaxFishCapacity);
            SceneManager.LoadScene("BadEnd");
        }
    }

    public void EndGameGood()
    {
        SceneManager.LoadScene("GoodEnd");
    }

    public void MorePods()
    {
        if (CurrentFish >= MaxFishCapacity - 10)
            MaxFishCapacity += 2;

        if (CurrentFish < MaxFishCapacity)
        {
            MaxFishCapacity = CurrentFish + 5;
        }
    }
}

/*
 *
 * number of fish currently in the scene
 * number of fish pods
 * number of fish the environment can sustain
 *
 * rate at which fish pods are spawned
 * rate at which fish pods replenish fish - calc here then send that number into the fish pod when you instatntiate it then use it to spawn fish in the FishPod script
 *
 * total number of fish a pod can spawn before it dies
 *
 * actually.. the fish pods should be able to spawn fish at a rate that is dependent on the number of fish in the pod
 *
 *
 * the more fish are caught the more fish pods spawn
 *
 */