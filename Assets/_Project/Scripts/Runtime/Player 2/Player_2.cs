using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_2 : MonoBehaviour
{
    #region FIELDS

    public Map Map;
    public ParticleSystem FishSellParticles, BoatSpeedParticles, BoatUpgradeParticles, CargoSpaceParticles, FishingSpeedParticles;
    public GameManager GameManager;
    public FishManager FishManager;
    public GameObject[] BoatPrefabs;
    private int BoatIndex = 1;
    public GameObject Boat;
    public Boat_P2 BoatScript;
    public Float FloatScript;
    public Collider FishFactory, BoatSpeedDock, BoatUpgradeDock, CargoSpaceDock, FishingSpeedDock;
    public DockSt FishFactoryDock, BoatSpeedDockDock, BoatUpgradeDockDock, CargoSpaceDockDock, FishingSpeedDockDock;
    public int Money = 0;
    public int FishCount = 0;
    public int CargoSpace = 20;
    public float CatchRate = 1;
    public float CatchRadius = 5;
    public int FishingSpeedUpgradeCost = 50, BoatSpeedUpgradeCost = 50, BoatUpgradeCost = 100, CargoSpaceUpgradeCost = 50;
    private float timeSinceLastCatch = 0f;
    private float timeSinceLastUpdate = 0f;

    #endregion FIELDS

    #region UNITY METHODS

    private void Start()
    {
        //BoatScript = GetComponentInChildren<Boat_P2>();
        //FloatScript = GetComponentInChildren<Float>();
        Map = FindObjectOfType<Map>();
        FishFactory = Map.GetDock("FishFactory", 2);
        BoatSpeedDock = Map.GetDock("BoatSpeedDock", 2);
        BoatUpgradeDock = Map.GetDock("BoatUpgradeDock", 2);
        CargoSpaceDock = Map.GetDock("CargoSpaceDock", 2);
        FishingSpeedDock = Map.GetDock("FishingSpeedDock", 2);
        FishFactoryDock = new DockSt("FishFactory", 2, FishFactory);
        BoatSpeedDockDock = new DockSt("BoatSpeedDock", 2, BoatSpeedDock);
        BoatUpgradeDockDock = new DockSt("BoatUpgradeDock", 2, BoatUpgradeDock);
        CargoSpaceDockDock = new DockSt("CargoSpaceDock", 2, CargoSpaceDock);
        FishingSpeedDockDock = new DockSt("FishingSpeedDock", 2, FishingSpeedDock);
        CatchRadius = BoatScript.CatchRadius;
    }

    private void LateUpdate()
    {
        if (FishFactoryDock.IsDocked(FloatScript.Center))
        {
            HandleFactoryDocking();
        }
        if (timeSinceLastUpdate >= 2)
        {
            if (BoatSpeedDockDock.IsDocked(FloatScript.Center))
            {
                HandleSpeedDocking();
            }
            if (BoatUpgradeDockDock.IsDocked(FloatScript.Center))
            {
                HandleUpgradeDocking();
            }
            if (CargoSpaceDockDock.IsDocked(FloatScript.Center))
            {
                HandleCargoDocking();
            }
            if (FishingSpeedDockDock.IsDocked(FloatScript.Center))
            {
                HandleFishingRateDocking();
            }
            timeSinceLastUpdate = 0f;
        }
    }

    private void FixedUpdate()
    {
        timeSinceLastCatch += Time.deltaTime;
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastCatch >= CatchRate & FishCount < CargoSpace)
        {
            foreach (FishPod pod in FishManager.PodList)
            {
                if (pod.FishCount > 0)
                {
                    foreach (Fish fish in pod.FishList)
                    {
                        if (Vector3.Distance(fish.transform.position, FloatScript.Center) < CatchRadius)
                        {
                            FishCount++;
                            BoatScript.fishParticle.Play();
                            SFXManager.Instance.PlaySFX(SFXManager.Instance.CatchFishSFX);
                            Destroy(fish.gameObject);
                            timeSinceLastCatch = 0f;
                            return;
                        }
                    }
                }
            }
        }
    }

    #endregion UNITY METHODS

    #region DOCK_METHODS

    public void HandleFactoryDocking()
    {
        if (FishCount == 0)
            return;
        FishSellParticles.Play();
        SFXManager.Instance.PlaySFX(SFXManager.Instance.FishSellSfx);
        addMoney(FishCount * GameManager.CostPerFish);
        FishCount = 0;
    }

    public void HandleSpeedDocking()
    {
        if (Money >= BoatSpeedUpgradeCost & Money > 0)
        {
            BoatSpeedParticles.Play();
            SFXManager.Instance.PlaySFX(SFXManager.Instance.BoatSpeedUpgradeSFX);
            removeMoney(BoatSpeedUpgradeCost);
            BoatSpeedUpgradeCost += 2;
            BoatScript.MaxSpeed += 2;
        }
    }

    public void HandleUpgradeDocking()
    {
        if (Money >= BoatUpgradeCost & BoatIndex <= 2 & Money > 0)
        {
            BoatUpgradeParticles.Play();
            SFXManager.Instance.PlaySFX(SFXManager.Instance.BoatUpgradeSFX);
            removeMoney(BoatUpgradeCost);
            BoatUpgradeCost *= 2;
            Transform CurrentBoatRef = Boat.transform;
            Destroy(Boat);
            Boat = Instantiate(BoatPrefabs[BoatIndex], CurrentBoatRef.position, CurrentBoatRef.rotation);
            CatchRadius = BoatScript.CatchRadius;
            BoatScript = Boat.GetComponent<Boat_P2>();
            FloatScript = Boat.GetComponent<Float>();
            BoatScript.Player_2 = this;
            CargoSpace *= 2;
            CatchRate -= 0.5f;
            BoatIndex++;
        }
    }

    public void HandleCargoDocking()
    {
        if (Money >= CargoSpaceUpgradeCost & Money > 0)
        {
            CargoSpaceParticles.Play();
            SFXManager.Instance.PlaySFX(SFXManager.Instance.CargoUpgradeSFX);
            removeMoney(CargoSpaceUpgradeCost);
            CargoSpaceUpgradeCost *= 2;
            CargoSpace += ((int)CargoSpace / 2);
        }
    }

    public void HandleFishingRateDocking()
    {
        if (Money >= FishingSpeedUpgradeCost & Money > 0)
        {
            FishingSpeedParticles.Play();
            SFXManager.Instance.PlaySFX(SFXManager.Instance.FishingSpeedUpgradeSFX);
            removeMoney(FishingSpeedUpgradeCost);
            FishingSpeedUpgradeCost *= 2;
            CatchRate -= 0.2f;
        }
    }

    public void addMoney(int money)
    {
        Money += money;
    }

    public void removeMoney(int money)
    {
        Money -= money;

        if (Money < 0)
        {
            Money = 0;
        }
    }

    #endregion DOCK_METHODS
}