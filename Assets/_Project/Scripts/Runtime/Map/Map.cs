using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    #region FIELDS

    public Collider P1_FishFactory, P1_BoatSpeedDock, P1_BoatUpgradeDock, P1_CargoSpaceDock, P1_FishingSpeedDock;
    public Collider P2_FishFactory, P2_BoatSpeedDock, P2_BoatUpgradeDock, P2_CargoSpaceDock, P2_FishingSpeedDock;

    #endregion FIELDS

    #region METHODS

    public Collider GetDock(string dockName, int playerNumber)
    {
        switch (dockName)
        {
            case "FishFactory":
                return playerNumber == 1 ? P1_FishFactory : P2_FishFactory;

            case "BoatSpeedDock":
                return playerNumber == 1 ? P1_BoatSpeedDock : P2_BoatSpeedDock;

            case "BoatUpgradeDock":
                return playerNumber == 1 ? P1_BoatUpgradeDock : P2_BoatUpgradeDock;

            case "CargoSpaceDock":
                return playerNumber == 1 ? P1_CargoSpaceDock : P2_CargoSpaceDock;

            case "FishingSpeedDock":
                return playerNumber == 1 ? P1_FishingSpeedDock : P2_FishingSpeedDock;

            default:
                return null;
        }
    }

    #endregion METHODS
}