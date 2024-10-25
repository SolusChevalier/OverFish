using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    #region FIELDS

    public Player_1 Player1;
    public Player_2 Player2;
    public Camera Cam;

    #endregion FIELDS

    #region UNITY METHODS

    private void Start()
    {
        Player1 = GameObject.Find("Player1").GetComponent<Player_1>();
        Player2 = GameObject.Find("Player2").GetComponent<Player_2>();
        Cam = GetComponent<Camera>();
        if (ActivateGame())
        {
            Debug.Log("Game Activated");
        }
        else
        {
            Debug.Log("Game Not Activated");
            ;
        }
    }

    #endregion UNITY METHODS

    #region METHODS

    public bool ActivateGame()
    {
        if (Player1 != null | Player2 != null | Cam != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion METHODS
}